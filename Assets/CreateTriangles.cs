using UnityEngine;

public class CreateTriangles : MonoBehaviour
{
    public Material triangleMaterial;

    void CreateTriangle(Vector3 voxel0, Vector3 voxel1, Vector3 voxel2)
    {
        print("Creating triangle");
        Mesh mesh = new Mesh();
        mesh.name = "Triangle";

        Vector3[] vertices = new Vector3[3]; // total number of vertices of object created (cube = 8, quad = 4, triangle = 3)
        Vector3[] normals = new Vector3[3];
        Vector2[] uvs = new Vector2[3];
        int[] triangles = new int[3]; //the number of vertices making up a triangle

        //all possible UVs
        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv10 = new Vector2(1f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);

        // define the actual shape/primitive - 3 vertices = triangle, 4 vertices = square/quad
        vertices = new Vector3[] { voxel0, voxel1, voxel2 };
        normals = new Vector3[] {Vector3.forward, // direction in which the face can be seen (other side will be invisible)
                                 Vector3.forward,
                                 Vector3.forward};

        uvs = new Vector2[] { uv00, uv10, uv01 }; // # of UVs
        triangles = new int[] { 0, 1, 2 }; // # of vertices

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject triangle = new GameObject("triangle");
        triangle.transform.parent = this.gameObject.transform;
        MeshFilter meshFilter = (MeshFilter)triangle.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        // display the triangle
        MeshRenderer renderer = triangle.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        // set the triangle's material
        renderer.material = triangleMaterial;

    }

    // Start is called before the first frame update
    void Start()
    {
        // X Y Z axis
        // triangle voxel 1
        Vector3 voxel0 = new Vector3(-0.5f, 0f, 0.5f);
        // triangle voxel 2
        Vector3 voxel1 = new Vector3(0.5f, 0f, 0.5f);
        // triangle voxel 3
        Vector3 voxel2 = new Vector3(0.5f, 0f, -0.5f);
        CreateTriangle(voxel0, voxel1, voxel2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
