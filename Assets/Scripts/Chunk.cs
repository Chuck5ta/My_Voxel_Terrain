﻿/*
 * A chunk contains quads that make up a part of the world's terrain.
 * 
 * 14 Feb 2020 - Terrain generation is now working, but it is in dire need of improving, as it is far too slow.
 * 16 Feb 2010 - Vastly improved the terrain gen algorithm. Now many, many times faster. Threading still to be implemented. 
 *             - Threading implemented, reducing the terrain gen time by more than 50%
 * 
 * 
 * Useful links
 * ============
 * Courses
 * https://holistic3d.com/
 * https://www.gamedev.tv/
 * 
 * Empyrion terrain generation information
 * https://docs.google.com/document/d/1MCvuCMtFvnCV8UHAglEi-IhLJgcD60TzNBjVbAZCptw/edit#heading=h.ygrn9c9eg1fd
 * 
 * Blending textures
 * https://answers.unity.com/questions/1351772/how-to-blend-two-textures.html
 *       
 * Threads
 * https://www.tutorialspoint.com/csharp/csharp_multithreading.htm
 * C# Job System
 * https://docs.unity3d.com/Manual/JobSystem.html?_ga=2.217102486.185736627.1581613830-307768343.1578037165
 * 
 */

using System.Collections;
using UnityEngine;
using System.Threading;

public class Chunk
{
    private Material quadMaterial;

    public Quad[,] chunkData; // 2D array to hold information on all of the quads in the chunk
                              // I may have to build the world in blocks, if this quad attempt does not work out,
                              // then chunkData will become a 3D array
    public Vector3[,] chunkVertices; // 2D array to hold all the coordinates of the vertices that make up the terrain in the chunk
    public GameObject chunk;

    public int chunkLengthX = 4;
    public int chunkLengthZ = 4;
    public int maxHeight = 10; // where 1 square = 1 metre wide. 
                               // maxHeight = 100 means a world that is 100 metres high

    public float perlinXScale = 0.8f;
    public float perlinZScale = 0.9f;
    public int perlinOffsetX = 0;
    public int perlinOffsetY = 0;
    public int perlinOctaves = 3;
    public int perlinPersistance = 8;
    public float perlinHeightScale = 0.9f;

    private int chunkZIndex, chunkXIndex;



    /*
     * Constructor
     * chunkIndex is the chunk we are currently working on
     * 
     * chunk 0 will be based at 0,0,0
     */
    public Chunk(int chunkZIndex, int chunkXIndex, Vector3 position)
    {
        this.chunkZIndex = chunkZIndex;
        this.chunkXIndex = chunkXIndex;

        Debug.Log("Building chunk name: " + World.BuildChunkName(position));

        chunk = new GameObject(World.BuildChunkName(position));
        chunk.transform.position = position;
        BuildChunk();
    }

    /*
     * fBM returns a value below 1, therefore we need this function to turn it into a value
     * in the game world of between 0 and maxHeight (e.g. 0 and 150 (1 unit/square = 1 metre wide/high/deep - 1m^2))
     */
    float Map(float newmin, int newmax, float origmin, float origmax, float value)
    {        
        return Mathf.Lerp(newmin, newmax, Mathf.InverseLerp(origmin, origmax, value));
    }

    /*
     * Fractal Brownian Motion
     * this function is used to generate the height (Y position in Unity) of each vertex
     */
    float fBM(float x, float z, int octave, float persistance)
    {
        float total = 0;
        float frequency = 0.2f;
        float amplitude = 0.4f;
        float maxValue = 0;
        for (int i = 0; i < octave; i++)
        {
            total += Mathf.PerlinNoise(x * frequency,
                                       z * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistance;
            frequency *= 2;
        }

        return total / maxValue;
    }

    /*
     * Pick a material to add to the quad
     */
    Material SetMaterial(Vector3 vertex0)
    {
        if (vertex0.y > maxHeight*0.40)
        {
            return World.rock;
        }        
        else if (vertex0.y > maxHeight * 0.25)
        {
            return World.dirt;
        }
        return World.grass;
    }

    /*
     * Display the quads - a row at a time, to create the terrain in a chunk
     * Unable to use the C# thread for this, therefore used Unity's Coroutine
     * May revisit this, as I would like to use C# thread throughout
     */
    void GenerateRowOfQuads(int z, int sizeX)
    {
        for (int x = 1; x <= sizeX; x++)
        {
            quadMaterial = SetMaterial(chunkVertices[x - 1, z]); // not ideal!!!
            Vector3 locationInChunk = new Vector3(x, z);
            // vertex0 - chunkVertices[x-1, z];
            // vertex1 - chunkVertices[x, z]
            // vertex2 - chunkVertices[x-1, z-1]
            // vertex3 - chunkVertices[x, z-1]
            chunkData[x - 1, z - 1] = new Quad(locationInChunk,
                                           chunkVertices[x - 1, z],
                                           chunkVertices[x, z],
                                           chunkVertices[x - 1, z - 1],
                                           chunkVertices[x, z - 1],
                                           chunk.gameObject, 
                                           this,
                                           quadMaterial);
            chunkData[x - 1, z - 1].Draw();
        }
    }

    /*
     * Draw the quads in the chunk
     */
    public void DrawChunk()
    {
        for (int z = 1; z <= World.chunkSize; z++)
        {
            // place the generation of a row of quads in its own thread
            GenerateRowOfQuads(z, World.chunkSize);
        }
    }

    /*
     * Generate the vertices that make up a row of a terrain in a chunk
     * C# thread is used to speed up the generation of these
     * ====================
     * Need to test if the location is at an end and there is a chunk next to it
     * 
     * 
     */
    void GenerateRowOfVertices(int z)
    {
        for (int x = 0; x < World.chunkSize * 2; x++)
        {

            // DO WE NEED TO GENERATE THIS ROW?
            //    If at an end and we have a chunk neighbour then get there row store it here
            // ELSE 
            // generate Y coordinate
            float yPos = Map(0, maxHeight, 0, 1, fBM((x + perlinXScale) * perlinXScale,
               (z + perlinZScale) * perlinZScale,
               perlinOctaves,
               perlinPersistance) * perlinHeightScale);

            // Store cordinates of this vertex
            chunkVertices[x, z] = new Vector3(x + (chunkXIndex * (World.chunkSize)), yPos, z + (chunkZIndex * (World.chunkSize)));

            Debug.Log("VERTS FOR ROW : " + z + " : " + chunkVertices[x, z]);
        }
    }


    public Chunk negativeXchunk, positiveXchunk, negativeZchunk, positiveZchunk;
    public bool isNegativeX_Chunk = false;
    public bool isPositiveX_Chunk = false;
    public bool isNegativeZ_Chunk = false;
    public bool isPositiveZ_Chunk = false;
    /*
     * This check to see if there are neighbouring chunks along the edges.
     * It retrieves all neighbours to be used later
     */
    private void CheckForNeighbourChunks()
    {
        // Check to see if we have neighbouring chunks
        // NegativeZ - back
        Vector3 negativeZPos = new Vector3(chunk.transform.position.x,
                                            chunk.transform.position.y,
                                            chunk.transform.position.z - World.chunkSize);
        string chunkName = World.BuildChunkName(negativeZPos);

  //      Debug.Log("Check for neighbour: " + chunkName);
        if (World.chunks.TryGetValue(chunkName, out negativeZchunk))
        {
 //           Debug.Log("Neg Z Neighbour found");
            isNegativeZ_Chunk = true;
        }
        // PositiveZ - forward
        Vector3 positiveZPos = new Vector3(chunk.transform.position.x,
                                            chunk.transform.position.y,
                                            chunk.transform.position.z + World.chunkSize);
        chunkName = World.BuildChunkName(positiveZPos);
 //       Debug.Log("Check for neighbour: " + chunkName);
        if (World.chunks.TryGetValue(chunkName, out positiveZchunk))
        {
  //          Debug.Log("Pos Z Neighbour found");
            isPositiveZ_Chunk = true;
        }
        // NegativeX - left
        Vector3 negativeXPos = new Vector3(chunk.transform.position.x - World.chunkSize,
                                            chunk.transform.position.y,
                                            chunk.transform.position.z);
        chunkName = World.BuildChunkName(negativeXPos);
 //       Debug.Log("Check for neighbour: " + chunkName);
        if (World.chunks.TryGetValue(chunkName, out negativeXchunk))
        {
  //          Debug.Log("Neg X Neighbour found");
            isNegativeX_Chunk = true;
        }
        // PositiveX - right
        Vector3 positiveXPos = new Vector3(chunk.transform.position.x + World.chunkSize,
                                            chunk.transform.position.y,
                                            chunk.transform.position.z);
        chunkName = World.BuildChunkName(positiveXPos);
  //      Debug.Log("Check for neighbour: " + chunkName);
        if (World.chunks.TryGetValue(chunkName, out positiveXchunk))
        {
   //         Debug.Log("Pos X Neighbour found");
            isPositiveX_Chunk = true;
        }
    }

    /*
     * This retrieves the vertices from the neighboring chunk and uses them in the row of the current chunk
     * This will result in the 2 chunks linking up visually
     * z is the current row number (Z axis position)
     * negativeZchunk is the neighbouring chunk we wish to get the vertices from
     */
    private void GetVerticesFromNegZneighbour(int z, Chunk negativeZchunk)
    {
        Debug.Log("*************************************************");
        Debug.Log("************************Z************************* " + z);
        // if negativeX
        //     Z = (World.chunkSize*2)-1
        // if positiveX
        //     Z = Z
        for (int x = 0; x < World.chunkSize * 2; x++)
        {
            chunkVertices[x, z].x = x + (chunkXIndex * (World.chunkSize)); // - (World.chunkSize * 2 + 1);
  //          chunkVertices[x, z].y = negativeZchunk.chunkVertices[x, (World.chunkSize * 2) - 5].y;
            chunkVertices[x, z].y = negativeZchunk.chunkVertices[x, World.chunkSize].y;
            chunkVertices[x, z].z = z + (chunkZIndex * (World.chunkSize));
            Debug.Log("NEG Z VERTS: @ " + x + " " + z + " : " + chunkVertices[x, z]);
        }
    }

    /*
     * This retrieves the vertices from the neighboring chunk and uses them in the row of the current chunk
     * This will result in the 2 chunks linking up visually
     * z is the current row number (Z axis position)
     * positiveZchunk is the neighbouring chunk we wish to get the vertices from
     */
    private void GetVerticesFromPosZneighbour(int z, Chunk positiveZchunk)
    {
        // if negativeX
        //     Z = (World.chunkSize*2)-1
        // if positiveX
        //     Z = Z
        for (int x = 0; x < World.chunkSize * 2; x++)
        {
            chunkVertices[x, z] = positiveZchunk.chunkVertices[x, 0]; // we need the verts from row 0 of the neighbour
        }
    }

    /* 
     * This function builds a chunk, which is used to contain quads of a part of the world. 
     * Results in improved efficiency in dealing with the quads.
     * A world can contain many chunks.
     * 
     * IDEA!!!! I have now implemented this - still need to include multi-processing/threads
     * generate the vertices then the quads - no more need to check neighboring quads to get their vertices
     * - something like this needs to be done, as current system is far too slow to generate the terrain
     * 
     */
    void BuildChunk()
    {
        // holds quad info within the chunk
        chunkData = new Quad[World.chunkSize, World.chunkSize];
        // holds vertices coordinates within the chunk
        chunkVertices = new Vector3[World.chunkSize * 2, World.chunkSize * 2];

        // find and retrieve neighbouring chunks
        CheckForNeighbourChunks();

        //      Thread[] rowOfVerts = new Thread[World.chunkSize*2];
        // generate all vertex coordinates
        for (int z = 0; z < World.chunkSize * 2; z++)
        {
            // rows at position 0 and World.chunkSize-1, do not generate new coordinates if they have a neighbour chunk
            //     retrieve them from the chunk neighbour 
            // RETRIEVE ALL VERTS FROM NEIGHBOURING CHUNK FOR ROW 0
            if (z == 0 && isNegativeZ_Chunk)
            {
    //            Debug.Log("Z = 0 and isNegativeZ_Chunk");
                // retrieve coords from neighbour
                GetVerticesFromNegZneighbour(z, negativeZchunk);
                continue;
            }
            else if (z == ((World.chunkSize * 2) - 1) && isPositiveZ_Chunk)
            {
 //               Debug.Log("Z = ((World.chunkSize * 2) - 1) and isPositiveZ_Chunk");
                // retrieve coords from neighbour
                GetVerticesFromPosZneighbour(z, positiveZchunk);
                continue;
            }
            else
            //      int index = z;
            // place the generation of a row of coordinates in its own thread
            //      rowOfVerts[z] = new Thread(() => GenerateRowOfVertices(index, World.chunkSize));
            //      rowOfVerts[z].Start();
                GenerateRowOfVertices(z);
        }

        // CombineQuads();
        //     yield return null; // yield must be included in an IEnumerator function
    }

    // Combine all the quads in the chunk
    void CombineQuads()
    {
        // combine all children meshes

        MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        // Total quads = meshFilters.Length
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        // Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter)chunk.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        mf.mesh.CombineMeshes(combine);

        // Delete all children (quad meshes)
        foreach (Transform childMesh in chunk.transform)
        {
             GameObject.Destroy(childMesh.gameObject);
        }
    }
}
