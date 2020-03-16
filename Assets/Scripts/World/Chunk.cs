/*
 * A chunk contains quads that make up a part of the Universe's terrain.
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
 * Shaders
 * https://www.google.com/search?q=unity+write+a+shader&rlz=1C1CHBD_en-GBGB888GB888&oq=unity+write+a+&aqs=chrome.2.0j69i57j0l6.5959j0j8&sourceid=chrome&ie=UTF-8#kpvalbx=_hMxTXrCWLMqd1fAPvt6p8AM32
 * http://guidohenkel.com/2013/04/a-simple-cross-fade-shader-for-unity/
 *       
 * Threads
 * https://www.tutorialspoint.com/csharp/csharp_multithreading.htm
 * C# Job System
 * https://docs.unity3d.com/Manual/JobSystem.html?_ga=2.217102486.185736627.1581613830-307768343.1578037165
 * 
 */

using System.Collections;
using System.Threading;
using UnityEngine;

public class Chunk
{
    Texturing texturing;
    TextureBlending textureBlending;

    private Material quadMaterial;

    public Quad[,] chunkData; // 2D array to hold information on all of the quads in the chunk
                              // I may have to build the Universe in blocks, if this quad attempt does not work out,
                              // then chunkData will become a 3D array
    public Vector3[,] chunkVertices; // 2D array to hold all the coordinates of the vertices that make up the terrain in the chunk
    public GameObject chunk;

    public int chunkLengthX = 4;
    public int chunkLengthZ = 4;
    public int maxTerrainHeight = 10; // where 1 square = 1 metre wide. 
                                      // maxHeight = 100 means a Universe that is 100 metres high

    // These are for the perlin noise method of Y axis value generation
    public float perlinXScale = 0.4f;
    public float perlinZScale = 0.7f;
    public int perlinOffsetX = 0; // where to start the curve along the X axis
    public int perlinOffsetZ = 0; // where to start the curve along the Z axis
    public int perlinOctaves = 4; // the number of curves we wish to use to generate the Y axis position
    public float perlinPersistance = 0.5f; // the amount of change in each successive curve (mulitplied by perlinPersistance)
                                           // each successibe curve is smaller that the previous one
    public float perlinHeightScale = 0.9f; // affects the height of the vertex (Y axis location)


    // represents current chunk being worked on
    private int chunkZIndex, chunkXIndex;

    // negativeXchunk is the chunk to the left of the current chunk, along the negative X axis
    // positiveXchunk is the chunk to the right of the current chunk, along the positive X axis
    // negativeZchunk is the chunk behind/back of the current chunk, along the negative Z axis
    // positiveZchunk is the chunk to the in front/forward of the current chunk, along the positive Z axis
    public Chunk negativeXchunk, positiveXchunk, negativeZchunk, positiveZchunk;
    public bool isNegativeX_Chunk = false;
    public bool isPositiveX_Chunk = false;
    public bool isNegativeZ_Chunk = false;
    public bool isPositiveZ_Chunk = false;


    /*
     * Constructor
     * chunkZIndex, chunkXIndex is the location of the chunk we are currently working on
     * 
     * e.g. chunk 0 will be based at 0,0,0 in the Universe
     */
    public Chunk(int chunkZIndex, int chunkXIndex, Vector3 position)
    {
        this.chunkZIndex = chunkZIndex;
        this.chunkXIndex = chunkXIndex;
        chunk = new GameObject(Universe.BuildChunkName(position));
        chunk.transform.position = position;
   //     BuildChunk();
    }


    // END OF TERRAIN BLENDING IN THE GAME Universe
    // ******************************************

    /*
     * make the terrain look more natural 
     * e.g. by applying blending of the textures where needed
     */
    public void MakeTerrainLookReal()
    {
        // go through the quads
        for (int z = 0; z < Universe.chunkSize; z++)
        {
            for (int x = 0; x < Universe.chunkSize; x++)
            {
                // Blend the tectures where needed
                TextureBlending.GradientBlend(chunkData, x, z);
            }
        }
    }
    
    // ********************************************
    // START OF TERRAIN BLENDING IN THE GAME Universe


    // END OF QUAD CREATION IN THE GAME Universe
    // ***************************************

    /*
     * Display the quads - a row at a time, to create the terrain in a chunk
     * Unable to use the C# thread for this, therefore used Unity's Coroutine
     * May revisit this, as I would like to use C# thread throughout
     */
    void GenerateRowOfQuads(int z, int sizeX)
    {
        int terrainType;
        for (int x = 1; x <= sizeX; x++)
        {
 /*           quadMaterial = Texturing.SetMaterial(chunkVertices[x - 1, z], maxTerrainHeight, out terrainType); // not ideal!!!
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
                                           quadMaterial,
                                           terrainType);
            chunkData[x - 1, z - 1].Draw();
        } */
    }

    /*
     * Draw the quads in the chunk
     * 
     * Unity is not thread safe yet - 
     * look at https://docs.unity3d.com/Manual/JobSystem.html?_ga=2.149032254.968692378.1582283703-307768343.1578037165
     */
 /*   public void DrawChunk()
    {
        for (int z = 1; z <= Universe.chunkSize; z++)
        {
            // place the generation of a row of quads in its own thread
            GenerateRowOfQuads(z, Universe.chunkSize);
        }
    } */

    // *****************************************
    // START OF QUAD CREATION IN THE GAME Universe




    /*
     * This retrieves the vertices from the neighboring chunk and uses them in the current chunk
     * This will result in the 2 chunks linking up visually
     * z is the current row number (Z axis position)
     * negativeZchunk is the neighbouring chunk we wish to get the vertices from
     */
/*    private void GetVerticesFromNegXneighbour(int z, Chunk negativeXchunk)
    {
        chunkVertices[0, z].x = chunkXIndex * Universe.chunkSize; // - (Universe.chunkSize * 2 + 1);
        chunkVertices[0, z].y = negativeXchunk.chunkVertices[Universe.chunkSize, z].y;
        chunkVertices[0, z].z = z + (chunkZIndex * Universe.chunkSize);
    } */

    /*
     * This retrieves the vertices from the neighboring chunk and uses them in the current chunk
     * This will result in the 2 chunks linking up visually
     * z is the current row number (Z axis position)
     * negativeZchunk is the neighbouring chunk we wish to get the vertices from
     */
 /*   private void GetVerticesFromPosXneighbour(int z, Chunk positiveXchunk)
    {
        chunkVertices[(Universe.chunkSize * 2) - 1, z].x = ((Universe.chunkSize * 2) - 1) + (chunkXIndex * Universe.chunkSize); // - (Universe.chunkSize * 2 + 1);
        chunkVertices[(Universe.chunkSize * 2) - 1, z].y = positiveXchunk.chunkVertices[0, z].y;
        chunkVertices[(Universe.chunkSize * 2) - 1, z].z = z + (chunkZIndex * Universe.chunkSize);
    } */

    /*
     * Generate the vertices that make up a row of a terrain in a chunk
     * C# thread is used to speed up the generation of these
     * ====================
     * Need to test if the location is at an end and there is a chunk next to it
     * 
     */
 /*   void GenerateRowOfVertices(int z)
    {
        for (int x = 0; x < Universe.chunkSize * 2; x++)
        {
            // IF x = 0, and have neighbour, then use their coords
            // rows at position 0 and Universe.chunkSize-1, do not generate new coordinates if they have a neighbour chunk
            //     retrieve them from the chunk neighbour 
            // RETRIEVE ALL VERTS FROM NEIGHBOURING CHUNK FOR ROW 0
            if (x == 0 && isNegativeX_Chunk)
            {
                // retrieve coords from neighbour
                GetVerticesFromNegXneighbour(z, negativeXchunk);
                continue;
            }
            // RETRIEVE ALL VERTS FROM NEIGHBOURING CHUNK FOR ROW (Universe.chunkSize * 2) - 1
            else if (x == ((Universe.chunkSize * 2) - 1) && isPositiveX_Chunk)
            {
                // retrieve coords from neighbour
                GetVerticesFromPosXneighbour(z, positiveXchunk);
                continue;
            }

            // DO WE NEED TO GENERATE THIS ROW?
            //    If at an end and we have a chunk neighbour then get their row and store it here
            // ELSE 
            // generate Y coordinate
            float yPos = NoiseGenerator.Map(0, maxTerrainHeight, 0, 1, NoiseGenerator.fBM((x + perlinOffsetX) * perlinXScale,
                                                           (z + perlinOffsetZ) * perlinZScale,
                                                           perlinOctaves,
                                                           perlinPersistance) * perlinHeightScale);

            // Store cordinates of this vertex
            chunkVertices[x, z] = new Vector3(x + (chunkXIndex * (Universe.chunkSize)), yPos, z + (chunkZIndex * (Universe.chunkSize)));
        } */
    }

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
                                            chunk.transform.position.z - Universe.chunkSize);
        string chunkName = Universe.BuildChunkName(negativeZPos);

        if (Universe.chunks.TryGetValue(chunkName, out negativeZchunk))
        {
            isNegativeZ_Chunk = true;
        }
        // PositiveZ - forward
        Vector3 positiveZPos = new Vector3(chunk.transform.position.x,
                                            chunk.transform.position.y,
                                            chunk.transform.position.z + Universe.chunkSize);
        chunkName = Universe.BuildChunkName(positiveZPos);
        if (Universe.chunks.TryGetValue(chunkName, out positiveZchunk))
        {
            isPositiveZ_Chunk = true;
        }
        // NegativeX - left
        Vector3 negativeXPos = new Vector3(chunk.transform.position.x - Universe.chunkSize,
                                            chunk.transform.position.y,
                                            chunk.transform.position.z);
        chunkName = Universe.BuildChunkName(negativeXPos);
        if (Universe.chunks.TryGetValue(chunkName, out negativeXchunk))
        {
            isNegativeX_Chunk = true;
        }
        // PositiveX - right
        Vector3 positiveXPos = new Vector3(chunk.transform.position.x + Universe.chunkSize,
                                            chunk.transform.position.y,
                                            chunk.transform.position.z);
        chunkName = Universe.BuildChunkName(positiveXPos);
        if (Universe.chunks.TryGetValue(chunkName, out positiveXchunk))
        {
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
        for (int x = 0; x < Universe.chunkSize * 2; x++)
        {
            chunkVertices[x, z].x = x + (chunkXIndex * Universe.chunkSize); // - (Universe.chunkSize * 2 + 1);
            chunkVertices[x, z].y = negativeZchunk.chunkVertices[x, Universe.chunkSize].y;
            chunkVertices[x, z].z = z + (chunkZIndex * Universe.chunkSize);
        }
    }

    /*
     * This retrieves the vertices from the neighboring chunk and uses them in the row of the current chunk
     * This will result in the 2 chunks linking up visually
     * z is the current row number (Z axis position)
     * positiveZchunk is the neighbouring chunk we wish to get the vertices from
     * 
     * NOTE: THIS MIGHT NOT WORK!!!!!
     * 
     */
    private void GetVerticesFromPosZneighbour(int z, Chunk positiveZchunk)
    {
        for (int x = 0; x < Universe.chunkSize * 2; x++)
        {
            chunkVertices[x, z].x = x + (chunkXIndex * Universe.chunkSize); // - (Universe.chunkSize * 2 + 1);
            chunkVertices[x, z].y = positiveZchunk.chunkVertices[x, 0].y;
            chunkVertices[x, z].z = z + (chunkZIndex * Universe.chunkSize);
        }
    }

    /* 
     * This function builds a chunk, which is used to contain quads of a part of the Universe. 
     * Results in improved efficiency in dealing with the quads.
     * A Universe can contain many chunks.
     * 
     */
    void BuildChunk()
    {
        // holds quad info within the chunk
        chunkData = new Quad[Universe.chunkSize, Universe.chunkSize];
        // holds vertices coordinates within the chunk
        chunkVertices = new Vector3[Universe.chunkSize * 2, Universe.chunkSize * 2];

        // find and retrieve neighbouring chunks
        CheckForNeighbourChunks();

        //      Thread[] rowOfVerts = new Thread[Universe.chunkSize*2];
        // generate all vertex coordinates
        for (int z = 0; z < Universe.chunkSize * 2; z++)
        {
            // rows at position 0 and Universe.chunkSize-1, do not generate new coordinates if they have a neighbour chunk
            //     retrieve them from the chunk neighbour 
            // RETRIEVE ALL VERTS FROM NEIGHBOURING CHUNK FOR ROW 0
            if (z == 0 && isNegativeZ_Chunk)
            {
                // retrieve coords from neighbour
                GetVerticesFromNegZneighbour(z, negativeZchunk);
                continue;
            }
            else if (z == ((Universe.chunkSize * 2) - 1) && isPositiveZ_Chunk)
            {
                // retrieve coords from neighbour
                GetVerticesFromPosZneighbour(z, positiveZchunk);
                continue;
            }

            //      int index = z;
            // place the generation of a row of coordinates in its own thread
            //      rowOfVerts[z] = new Thread(() => GenerateRowOfVertices(index, Universe.chunkSize));
            //      rowOfVerts[z].Start();
  //          GenerateRowOfVertices(z);
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
