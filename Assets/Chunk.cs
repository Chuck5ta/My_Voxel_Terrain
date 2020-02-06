using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Material quadMaterial;
    public Quad[,,] chunkData; // 3D array to hold information on all of the quads

    [SerializeField] int lengthX = 1;
    [SerializeField] int lengthY = 1;
    [SerializeField] int lengthZ = 1;

    /* 
     * chunk size (in the x, y, and z directions)
     * Set y to 0
     * We only want to create a flat plain initially
     * 
     */
    IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ)
    {
        chunkData = new Quad[sizeX, sizeY, sizeZ];

        // create quads
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
                for (int z = 0; z < sizeZ; z++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    chunkData[x,y,z] = new Quad(pos, this.gameObject, quadMaterial);
                }

        // draw quads
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
                for (int z = 0; z < sizeZ; z++)
                {
                    chunkData[x,y,z].Draw();
                    yield return null;
                }

        CombineQuads();
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

        // Delete all children (quad meshes)
        foreach (Transform childMesh in this.transform)
        {
            Destroy(childMesh.gameObject);
        }

        // Create a new mesh (quad) on the parent object
        MeshFilter mf = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh.CombineMeshes(combine);

        // Create a renderer for the parent (quad mesh)
        MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = quadMaterial;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 10 quads by 10 quads terrain
        StartCoroutine(BuildChunk(lengthX, lengthY, lengthZ));
    }

    // Update is called once per frame
    void Update()
    {        
    }
}
