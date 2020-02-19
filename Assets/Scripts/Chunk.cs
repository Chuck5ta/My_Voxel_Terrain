/*
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
        for (int x = 1; x < sizeX; x++)
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
     * Generate the vertices that make up a row of a terrain in a chunk
     * C# thread is used to speed up the generation of these
     */
    void GenerateRowOfVertices(int z)
    {
        for (int x = 0; x < World.chunkSize * 2; x++)
        {
            // generate Y coordinate
            float yPos = Map(0, maxHeight, 0, 1, fBM((x + perlinXScale) * perlinXScale,
               (z + perlinZScale) * perlinZScale,
               perlinOctaves,
               perlinPersistance) * perlinHeightScale);

            // Store cordinates of this vertex
            chunkVertices[x, z] = new Vector3(x + (chunkXIndex * (World.chunkSize - 1)), yPos, z + (chunkZIndex * (World.chunkSize - 1)));
        }
    }

    /*
     * Draw the quads in the chunk
     */
    public void DrawChunk()
    {
        for (int z = 1; z < World.chunkSize; z++)
        {
            // place the generation of a row of quads in its own thread
            GenerateRowOfQuads(z, World.chunkSize);
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
        chunkVertices = new Vector3[World.chunkSize*2, World.chunkSize*2];

  //      Thread[] rowOfVerts = new Thread[World.chunkSize*2];
        // generate all vertex coordinates
        for (int z = 0; z < World.chunkSize*2; z++)
        {
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
