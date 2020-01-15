using UnityEngine;

public class CreateTriangles : MonoBehaviour
{
    void CreateTriangle(Vector3 voxel0, Vector3 voxel1, Vector3 voxel2, Color meshColour)
    {
        print("Creating triangle");
        Mesh mesh = new Mesh();
        mesh.name = "Triangle"; // MAKE THIS UNIQUE!!!

        // define the actual shape/primitive - 3 vertices = triangle, 4 vertices = square/quad
        Vector3[] newVertices = new Vector3[] { voxel0, voxel1, voxel2 };

        //all possible UVs
        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);
        Vector2 uv11 = new Vector2(1f, 1f);
        Vector2[] newUV = new Vector2[] { uv11, uv01, uv00 }; // # of UVs
        int[] newTriangles = new int[] { 0, 1, 2 }; // # of vertices - triangle = 3, quad = 6 (made up of 2 triangles)

        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;

        mesh.RecalculateBounds();

        GameObject triangle = new GameObject("triangle");
        triangle.transform.parent = this.gameObject.transform;
        MeshFilter meshFilter = (MeshFilter)triangle.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        // display the triangle
        MeshRenderer renderer = triangle.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material.color = meshColour;
    }

    void CreatePyramid()
    {
        // X Y Z axis
        // triangle voxel 1
        Vector3 voxel0 = new Vector3(-0.5f, 0f, 0.5f);
        // triangle voxel 2
        Vector3 voxel1 = new Vector3(0.5f, 0f, 0.5f);
        // triangle voxel 3
        Vector3 voxel2 = new Vector3(0.5f, 0f, -0.5f);
        CreateTriangle(voxel0, voxel1, voxel2, Color.blue); // up - order of the coords = direction of the normals
                                                            // up facing face - facing up towards positive Y axis
        // triangle voxel 1
        voxel0 = new Vector3(-0.5f, 0f, 0.5f);
        // triangle voxel 2
        voxel1 = new Vector3(0.5f, 0f, 0.5f);
        // triangle voxel 3
        voxel2 = new Vector3(0.5f, -1f, -1f);
        CreateTriangle(voxel2, voxel1, voxel0, Color.red); // forwards/front facing face - facing towards positive Z axis

        // triangle voxel 1
        voxel0 = new Vector3(-0.5f, 0f, 0.5f);
        // triangle voxel 2
        voxel1 = new Vector3(0.5f, -1f, -1f);
        // triangle voxel 3
        voxel2 = new Vector3(0.5f, 0f, -0.5f);
        CreateTriangle(voxel1, voxel0, voxel2, Color.cyan); // back facing face - facing towards negative Z axis

        // triangle voxel 1
        voxel0 = new Vector3(0.5f, -1f, -1f);
        // triangle voxel 2
        voxel1 = new Vector3(0.5f, 0f, 0.5f);
        // triangle voxel 3
        voxel2 = new Vector3(0.5f, 0f, -0.5f);
        CreateTriangle(voxel2, voxel1, voxel0, Color.green); // right facing face - facing towards positive X axis
    }

    // Start is called before the first frame update
    void Start()
    {     
        CreatePyramid();
        //  CreateTriangle();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
