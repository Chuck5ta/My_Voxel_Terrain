﻿/*
 * A chunk contains quads that make up a part of the world's terrain.
 * 
 * 14 Feb 2020 - Terrain generation is now working, but it is in dire need of improving, as it is far too slow.
 * 16 Feb 2010 - Vastly improved the terrain gen algorithm. Now many, many times faster. Threading still to be implemented. 
 * 
 * 
 * 
 * Useful links
 * ============
 * Empyrion terrain generation information
 * https://docs.google.com/document/d/1MCvuCMtFvnCV8UHAglEi-IhLJgcD60TzNBjVbAZCptw/edit#heading=h.ygrn9c9eg1fd
 * 
 * Blending textures
 *       https://answers.unity.com/questions/1351772/how-to-blend-two-textures.html
 * 
 */

using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Material quadMaterial;
    public Material grassMaterial;
    public Material rockMaterial;
    public Material sandMaterial;
    public Material dirtMaterial;

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
     * Leaving this function in for now, but it may be removed later
     */
    private Vector3 GetVertexFromQuad(GameObject quad, int vertexPos)
    {
        Vector3[] meshVerts = quad.GetComponent<MeshFilter>().mesh.vertices;
        Vector3 vertex = quad.transform.position + transform.TransformPoint(meshVerts[vertexPos]);
        Debug.Log(" ");
        Debug.Log(vertex);
        Debug.Log(" ");
        return vertex;
    }

    /*
     * Pick a material to add to the quad
     */
    Material SetMaterial(Vector3 vertex0)
    {
        print("Setting material");
        if (vertex0.y > maxHeight*0.40)
        {
            return rockMaterial;
        }        
        else if (vertex0.y > maxHeight * 0.25)
        {
            return dirtMaterial;
        }
        return grassMaterial;
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
    IEnumerator BuildChunk(int sizeX, int sizeZ)
    {
        // holds quad info within the chunk
        chunkData = new Quad[sizeX, sizeZ];
        // holds vertices coordinates within the chunk
        Vector3[,] chunkVertices = new Vector3[sizeX, sizeZ];

        // generate all vertex coordinates
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                // generate Y coordinate
                float yPos = Map(0, maxHeight, 0, 1, fBM((x + perlinXScale) * perlinXScale,
                   (z + perlinZScale) * perlinZScale,
                   perlinOctaves,
                   perlinPersistance) * perlinHeightScale);

                // Store cordinates of this vertex
                chunkVertices[x, z] = new Vector3(x,yPos,z);
    //            Debug.Log("Coords generated at " + x + " " + z + " : " + chunkVertices[x, z]);
            }
        }

        // Place chunk in world based on newly generated vertices
        // create quads
        for (int z = 1; z < sizeZ; z++)
        {
            for (int x = 1; x < sizeX; x++)
            {
                Vector3 locationInChunk = new Vector3(x, z);
                // vertex0 - chunkVertices[x-1, z]
                // vertex1 - chunkVertices[x, z]
                // vertex2 - chunkVertices[x-1, z-1]
                // vertex3 - chunkVertices[x, z-1]
                chunkData[x-1, z-1] = new Quad(locationInChunk, 
                                               chunkVertices[x-1, z], 
                                               chunkVertices[x, z],
                                               chunkVertices[x-1, z-1], 
                                               chunkVertices[x, z-1], 
                                               this.gameObject, 
                                               quadMaterial);
                chunkData[x-1, z-1].Draw();
            }
        }

        // CombineQuads();
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
