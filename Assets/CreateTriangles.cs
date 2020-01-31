using UnityEngine;

/*
 * 31 JAN 2020
 * Build the terrain out of quads, each made out of 2 triangles
 *  
 * I don't think this needs cubes or any 3D objects for the terrain. You just alter the vertex coordinates and rerender?
 * May need to add remove geometry (triangles or quads)
 * 
 */

public class CreateTriangles : MonoBehaviour
{
    private float Xaxis = 0f; // defaulted to the centre of the world
    private float Yaxis = 0f; // defaulted to the centre of the world
    private float Zaxis = 0f; // defaulted to the centre of the world

    
    void CreateTriangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color meshColour)
    {
        print("Creating triangle");
        Mesh mesh = new Mesh();
        mesh.name = "Triangle"; // MAKE THIS UNIQUE!!!

        // define the actual shape/primitive - 3 vertices = triangle, 4 vertices = square/quad
        Vector3[] newVertices = new Vector3[] { vertex1, vertex2, vertex3 };

        //all possible UVs
        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);
        Vector2 uv11 = new Vector2(1f, 1f);
        Vector2[] newUV = new Vector2[] { uv11, uv01, uv00 }; // # of UVs
        int[] newTriangles = new int[] { 0, 1, 2 }; // # of vertices - triangle = 3, quad = 6 (made up of 2 triangles)

        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;

        mesh.RecalculateBounds(); // this is used for rendering

        GameObject triangle = new GameObject("triangle");
        triangle.transform.parent = this.gameObject.transform;
        MeshFilter meshFilter = (MeshFilter)triangle.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        // display the triangle
        MeshRenderer renderer = triangle.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material.color = meshColour;
    }
    
    // A quad is made out of 2 triangles
    void CreateQuad(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color meshColour)
    {
        print("Creating quad");
        Mesh mesh = new Mesh();
        mesh.name = "Quad"; // MAKE THIS UNIQUE!!!

        // create 1st triangle
        CreateTriangle(vertex1, vertex2, vertex3, Color.blue);

        // create 2nd triangle
        Vector3 triangleTwoVertex1 = new Vector3(Xaxis, Yaxis, Zaxis + 0.5f);
        Vector3 triangleTwoVertex2 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis + 0.5f);
        Vector3 triangleTwoVertex3 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis);
        CreateTriangle(triangleTwoVertex1, triangleTwoVertex2, triangleTwoVertex3, Color.red);

        // do we need to combine the traingles into one mesh? 
        // possibly
        // we can still alter the coordinates of all the vertices of the triangles that make up the quad
        // so, yes we should combine the triangles into a quad mesh

        // COMBINE HERE
    }

    /* INEFFICIENT - far better to use quads - far less geometry required
    void CreatePyramid()
    {
        // X Y Z axis
        // triangle vertex 1
        Vector3 vertex0 = new Vector3(Xaxis - 0.5f, Yaxis, Zaxis + 0.5f);
        // triangle vertex 2
        Vector3 vertex1 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis + 0.5f);
        // triangle vertex 3
        Vector3 vertex2 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis - 0.5f);
        CreateTriangle(vertex0, vertex1, vertex2, Color.blue); // up - order of the coords = direction of the normals
                                                            // up facing face - facing up towards positive Y axis
        // triangle voxel 1
        vertex0 = new Vector3(Xaxis - 0.5f, Yaxis, Zaxis + 0.5f);
        // triangle vertex 2
        vertex1 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis + 0.5f);
        // triangle vertex 3
        vertex2 = new Vector3(Xaxis + 0.5f, Yaxis - 1f, Zaxis - 1f);
        CreateTriangle(vertex2, vertex1, vertex0, Color.red); // forwards/front facing face - facing towards positive Z axis

        // triangle vertex 1
        vertex0 = new Vector3(Xaxis - 0.5f, Yaxis, Zaxis + 0.5f);
        // triangle vertex 2
        vertex1 = new Vector3(Xaxis + 0.5f, Yaxis - 1f, Zaxis - 1f);
        // triangle vertex 3
        vertex2 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis - 0.5f);
        CreateTriangle(vertex1, vertex0, vertex2, Color.cyan); // back facing face - facing towards negative Z axis

        // triangle vertex 1
        vertex0 = new Vector3(Xaxis + 0.5f, Yaxis - 1f, Zaxis - 1f);
        // triangle vertex 2
        vertex1 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis + 0.5f);
        // triangle vertex 3
        vertex2 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis - 0.5f);
        CreateTriangle(vertex2, vertex1, vertex0, Color.green); // right facing face - facing towards positive X axis
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        // Initial triangle vertices coordinates
        Vector3 vertex1 = new Vector3(Xaxis, Yaxis, Zaxis);
        Vector3 vertex2 = new Vector3(Xaxis, Yaxis, Zaxis + 0.5f);
        Vector3 vertex3 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis);

        CreateQuad(vertex1, vertex2, vertex3, Color.blue);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
