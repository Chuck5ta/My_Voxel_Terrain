using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Material quadMaterial;
    public Quad[,] chunkData; // 2D array to hold information on all of the quads in the chunk
                              // I may have to build the world in blocks, if this quad attempt does not work out,
                              // then chunkData will become a 3D array

    public int chunkLengthX = 4; // these don't seem to be used based on the actual value (not 10 x 10 x 10)
    public int chunkLengthZ = 4;
    public int maxHeight = 10; // where 1 square = 1 metre wide, this is a world with a max height of 100 metres

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

    /* 
     * chunk size (in the x, y, and z directions)
     * Set y to 0
     * We only want to create a flat plain initially
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
                // use Perlin here to decide on the Y axis position of the quad
                // could we use this to be the position of the vertex, not the quad?
                float yPos = Map(0, maxHeight, 0, 1, fBM((x + perlinXScale) * perlinXScale,
                                 (z + perlinZScale) * perlinZScale,
                                 perlinOctaves,
                                 perlinPersistance) * perlinHeightScale);

                Vector3 pos = new Vector3(x, yPos, z);

                // need to also pass the coordinates of the 4 vertices
                // it needs to know what quads are next to it, then link up to them
                chunkData[x, z] = new Quad(pos, this.gameObject, quadMaterial);
                chunkData[x, z].Draw();
            }
        }

        CombineQuads();
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

        // Create a renderer for the parent (quad mesh)
 //       MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
 //       renderer.material = quadMaterial;

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
