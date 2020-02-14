/*
 * 14 FEB 2020
 * A chunk contains quads that make up a part of the world's terrain.
 * 
 * 14 Feb 2020 - Terrain generation is now working, but it is in dire need of improving, as it is far too slow.
 * 
 * 
 */

using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Material quadMaterial;
    public Quad[,] chunkData; // 2D array to hold information on all of the quads in the chunk
                              // I may have to build the world in blocks, if this quad attempt does not work out,
                              // then chunkData will become a 3D array

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

    const int VERTEX0 = 0;
    const int VERTEX1 = 1;
    const int VERTEX2 = 2;
    const int VERTEX3 = 3;

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
     * this function returns the height (Y position in Unity) of each quad
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

    private float GenerateYpos(int vertexLocation, int x, int z)
    {
        float yPos = Map(0, maxHeight, 0, 1, fBM((x + perlinXScale) * perlinXScale + vertexLocation,
           (z + perlinZScale) * perlinZScale + vertexLocation,
           perlinOctaves,
           perlinPersistance) * perlinHeightScale);
        return yPos;
    }
    private Vector3 GetVertexFromQuad(GameObject quad, int vertexPos)
    {
        Vector3[] meshVerts = quad.GetComponent<MeshFilter>().mesh.vertices;
        Vector3 vertex = quad.transform.position + transform.TransformPoint(meshVerts[vertexPos]);
        Debug.Log(" ");
        Debug.Log(vertex);
        Debug.Log(" ");
        return vertex;
    }

    Vector3 GetVertex(int vertexLocation, int x, int z)
    {
        Vector3 vertex = new Vector3(0,0,0);
        GameObject quad;
        float yPos = 0f;

        // Vertex 0 = top-left 
        // Vertex 1 = top-right
        // Vertex 2 = bottom-left
        // Vertex 3 = bottom-right
        //   0 --- 1
        //   |     | vertices
        //   2 --- 3

        // Vertex 0
        // --------
        // IF PositiveZ neighbour || NegativeX neighbour
        //     GET vertex (either PositiveZ's bottom left vertex OR NegativeX's top-right vertex)
        // ELSE
        //     fBM vertex 0
        if (vertexLocation == 0)
        {
            // is there a quad on the PositiveZ
            if (quad = GameObject.Find("Quad_" + x + "_" + (z + 1)))
            {
                Debug.Log("Quad does exist at  " + x + "_" + (z + 1));
                return GetVertexFromQuad(quad, VERTEX2);
            }
            // is there a quad on the NegativeX
            else if (quad = GameObject.Find("Quad_" + (x - 1) + "_" + z))
            {
                Debug.Log("Quad does exist at  " + (x - 1) + "_" + z);
                return GetVertexFromQuad(quad, VERTEX1);
            }
            else
            {
                print("fBM the vertex!");
                // fBM the vertex
                yPos = GenerateYpos(vertexLocation, x, z);
            }
            vertex = new Vector3(x - 0.5f, yPos, z + 0.5f);
        }
        //   0 --- 1
        //   |     | vertices
        //   2 --- 3
        // Vertex 1
        // --------
        // IF PositiveZ neighbour || PositiveX neighbour
        //     GET vertex (either PositiveZ's bottom right vertex OR PositiveX's top-left vertex)
        // ELSE
        //     fBM vertex 1
        else if (vertexLocation == 1)
        {
            // is there a quad on the PositiveZ
            if (quad = GameObject.Find("Quad_" + x + "_" + (z+1)))
            {
                Debug.Log("Quad does exist at  " + x + "_" + (z + 1));
                return GetVertexFromQuad(quad, VERTEX3);
            }
            // is there a quad on the PositiveX
            else if (quad = GameObject.Find("Quad_" + (x+1) + "_" + z))
            {
                Debug.Log("Quad does exist at  " + (x + 1) + "_" + z);
                return GetVertexFromQuad(quad, VERTEX0);
            }
            else
            {
                print("fBM the vertex!");
                // fBM the vertex
                yPos = GenerateYpos(vertexLocation, x, z);
            }
            vertex = new Vector3(x + 0.5f, yPos, z + 0.5f);
        }
        //   0 --- 1
        //   |     | vertices
        //   2 --- 3
        // Vertex 2
        // --------
        // IF NegativeZ neighbour || NegativeX neighbour
        //     GET vertex (either NegativeZ's bottom right vertex OR NegativeX's top-left vertex)
        // ELSE
        //     fBM vertex 2
        else if (vertexLocation == 2)
        {
            // is there a quad on the NegativeZ
            if (quad = GameObject.Find("Quad_" + x + "_" + (z-1)))
            {
                Debug.Log("Quad does exist at  " + x + "_" + (z - 1));
                return GetVertexFromQuad(quad, VERTEX0);
            }
            // is there a quad on the NegativeX
            else if (quad = GameObject.Find("Quad_" + (x-1) + "_" + z))
            {
                Debug.Log("Quad does exist at  " + (x - 1) + "_" + z);
                return GetVertexFromQuad(quad, VERTEX3);
            }
            else
            {
                print("fBM the vertex!");
                // fBM the vertex
                yPos = GenerateYpos(vertexLocation, x, z);
            }
            vertex = new Vector3(x-0.5f, yPos, z-0.5f);
        }
        //   0 --- 1
        //   |     | vertices
        //   2 --- 3
        // Vertex 3
        // --------
        // IF NegativeZ neighbour || PositiveX neighbour
        //     GET vertex (either NegativeZ's top-right vertex OR PositiveX's bottom-left vertex)
        // ELSE
        //     fBM vertex 3
        else // vertexLocation == 3
        {
            // is there a quad on the NegativeZ
            if (quad = GameObject.Find("Quad_" + x + "_" + (z-1)))
            {
                Debug.Log("Quad does exist at  " + x + "_" + (z - 1));
                return GetVertexFromQuad(quad, VERTEX1);
            }
            // is there a quad on the PositiveX
            else if (quad = GameObject.Find("Quad_" + (x+1) + "_" + z))
            {
                Debug.Log("Quad does exist at  " + (x + 1) + "_" + z);
                return GetVertexFromQuad(quad, VERTEX2);
            }
            else
            {
                print("fBM the vertex!");
                // fBM the vertex
                yPos = GenerateYpos(vertexLocation, x, z);
            }
            vertex = new Vector3(x+0.5f, yPos, z-0.5f);
        }

        Debug.Log("New vertex: " + vertex);

        return vertex;
    }

    /* 
     * This function builds a chunk, which is used to contain quads of a part of the world. 
     * Results in improved efficiency in dealing with the quads.
     * A world can contain many chunks.
     * 
     * IDEA!!!!
     * generate the vertices then the quads - no more need to check neighboring quads to get their vertices
     * - something like this needs to be done, as current system is far too slow to generate the terrain
     * 
     */
    IEnumerator BuildChunk(int sizeX, int sizeZ)
    {
        chunkData = new Quad[sizeX, sizeZ];

        // create quads
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                print(" ");
                print(" ");
                print("QUAD: " + x + " " + z);
                print("===========");
                // work out which vertices need to be generated
                // Vertices that are connected to quads that already exist can use the vertex of those quads at
                // the point the quads meet.

                // Vertex 0 = top-left 
                // Vertex 1 = top-right
                // Vertex 2 = bottom-left
                // Vertex 3 = bottom-right
                //   0 --- 1
                //   |     | vertices
                //   2 --- 3

                // Vertex 0
                // --------
                // IF PositiveZ neighbour || NegativeX neighbour
                //     GET vertex (either PositiveZ's bottom left vertex OR NegativeX's top-right vertex)
                // ELSE
                //     fBM vertex 0
                print("Vertex 0");
                Vector3 vertex0 = GetVertex(0, x, z);
                print("Vertex 0: " + vertex0);

                // Vertex 1
                // --------
                // IF PositiveZ neighbour || PositiveX neighbour
                //     GET vertex (either PositiveZ's bottom right vertex OR PositiveX's top-left vertex)
                // ELSE
                //     fBM vertex 1
                print("Vertex 1");
                Vector3 vertex1 = GetVertex(1, x, z);
                print("Vertex 1: " + vertex1);

                // Vertex 2
                // --------
                // IF NegativeZ neighbour || NegativeX neighbour
                //     GET vertex (either NegativeZ's bottom right vertex OR NegativeX's top-left vertex)
                // ELSE
                //     fBM vertex 2
                print("Vertex 2");
                Vector3 vertex2 = GetVertex(2, x, z);
                print("Vertex 2: " + vertex2);

                // Vertex 3
                // --------
                // IF NegativeZ neighbour || PositiveX neighbour
                //     GET vertex (either NegativeZ's top-right vertex OR PositiveX's bottom-left vertex)
                // ELSE
                //     fBM vertex 3
                print("Vertex 3");
                Vector3 vertex3 = GetVertex(3, x, z);
                print("Vertex 3: " + vertex0);

                Vector3 locationInChunk = new Vector3(x, z);

                chunkData[x, z] = new Quad(locationInChunk, vertex0, vertex1, vertex2, vertex3, this.gameObject, quadMaterial);
                chunkData[x, z].Draw();
            }
        }

   //     CombineQuads();
        yield return null; // yield must be included in an IEnumerator function
    }

    // Combine all the quads in the chunk
    void CombineQuads()
    {
        // combine all children meshes

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
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
        MeshFilter mf = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        mf.mesh.CombineMeshes(combine);

        // Delete all children (quad meshes)
        foreach (Transform childMesh in this.transform)
        {
            Destroy(childMesh.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildChunk(chunkLengthX, chunkLengthZ));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
