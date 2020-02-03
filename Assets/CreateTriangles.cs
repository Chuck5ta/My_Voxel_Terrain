using UnityEngine;

/*
 * 31 JAN 2020
 * Build the terrain out of quads, each made out of 2 triangles
 *  
 * I don't think this needs cubes or any 3D objects for the terrain. You just alter the vertex coordinates and rerender?
 * May need to add remove geometry (triangles or quads)
 * 
 * https://answers.unity.com/questions/154324/how-do-uvs-work.html
 * 
 */

public class CreateTriangles : MonoBehaviour
{
    public Material quadMaterial;

    private float Xaxis = 0f; // defaulted to the centre of the world
    private float Yaxis = 0f; // defaulted to the centre of the world
    private float Zaxis = 0f; // defaulted to the centre of the world

    /* ************

    void CreateTriangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color meshColour)
    {
        print("Creating triangle");
        Mesh mesh = new Mesh();
        //     mesh.name = "Triangle"; // MAKE THIS UNIQUE!!! 

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
        //     MeshRenderer renderer = triangle.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        //     renderer.material.color = meshColour;
    }

    void CreateTriangle2(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color meshColour)
    {
        print("Creating triangle");
        Mesh mesh = new Mesh();
        //     mesh.name = "Triangle"; // MAKE THIS UNIQUE!!! 

        // define the actual shape/primitive - 3 vertices = triangle, 4 vertices = square/quad
        Vector3[] newVertices = new Vector3[] { vertex1, vertex2, vertex3 };

        //all possible UVs
        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);
        Vector2 uv11 = new Vector2(1f, 1f);
        Vector2 uv10 = new Vector2(1f, 0f);
        Vector2[] newUV = new Vector2[] { uv01, uv11, uv10 }; // # of UVs
        int[] newTriangles = new int[] { 1, 2, 0 }; // # of vertices - triangle = 3, quad = 6 (made up of 2 triangles)

        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;

        mesh.RecalculateBounds(); // this is used for rendering

        GameObject triangle = new GameObject("triangle");
        triangle.transform.parent = this.gameObject.transform;
        MeshFilter meshFilter = (MeshFilter)triangle.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        // display the triangle
        //     MeshRenderer renderer = triangle.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        //     renderer.material.color = meshColour;
    }

    // Make a quad mesh out of 2 triangles
    void CombineTriangles()
    {
        // combine all children meshes (triangles)

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        // Total triangles = meshFilters.Length
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        // Create a new mesh (quad) on the parent object
        MeshFilter quad = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
        quad.mesh.CombineMeshes(combine);

        // Create a renderer for the parent (quad mesh)
        MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = quadMaterial;

        // Delete all children (triangle meshes)
        foreach (Transform triangle in this.transform)
        {
            Destroy(triangle.gameObject);
        }
    }

    // A quad is made out of 2 triangles
    void CreateQuadX(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4, Color meshColour)
    {
        print("Creating quad");

        // create 1st triangle
        CreateTriangle(vertex1, vertex2, vertex3, Color.blue);

        // create 2nd triangle
        Vector3 triangleTwoVertex1 = new Vector3(Xaxis, Yaxis, Zaxis + 0.5f);
        Vector3 triangleTwoVertex2 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis + 0.5f);
        Vector3 triangleTwoVertex3 = new Vector3(Xaxis + 0.5f, Yaxis, Zaxis);
        CreateTriangle2(triangleTwoVertex1, triangleTwoVertex2, triangleTwoVertex3, Color.red);

        // do we need to combine the traingles into one mesh? 
        // possibly
        // we can still alter the coordinates of all the vertices of the triangles that make up the quad
        // so, yes we should combine the triangles into a quad mesh

        // COMBINE HERE
        CombineTriangles();
    }

    ************************************************************ */


    void CreateQuad(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4, Color meshColour)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Quad";

        Vector3[] vertices = new Vector3[4];
        vertices[0] = vertex1; //top-left
        vertices[1] = vertex2; //top-right
        vertices[2] = vertex3; //bottom-left
        vertices[3] = vertex4; //bottom-right

        mesh.vertices = vertices;

        int[] triangles = new int[6] { 0, 1, 2, 3, 2, 1 };
        mesh.triangles = triangles;

        Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(0, 1); //top-left
        uvs[1] = new Vector2(1, 1); //top-right
        uvs[2] = new Vector2(0, 0); //bottom-left
        uvs[3] = new Vector2(1, 0); //bottom-right

        mesh.uv = uvs;

        Vector3[] normals = new Vector3[4] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
        mesh.normals = normals;

    //    mesh.RecalculateBounds();

        GameObject quad = new GameObject("quad");
        quad.transform.parent = this.gameObject.transform;
        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = quadMaterial;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Upright and facing forward
        //       vertices[0] = new Vector3(0, 1, 0); //top-left
        //       vertices[1] = new Vector3(1, 1, 0); //top-right
        //       vertices[2] = new Vector3(0, 0, 0); //bottom-left
        //       vertices[3] = new Vector3(1, 0, 0); //bottom-right
        // Flat and facing up
        //       vertices[0] = new Vector3(0, 0, 1); //top-left
        //       vertices[1] = new Vector3(1, 0, 1); //top-right
        //       vertices[2] = new Vector3(0, 0, 0); //bottom-left
        //       vertices[3] = new Vector3(1, 0, 0); //bottom-right

        // Initial triangle vertices coordinates (at the centre of the world)
        // Flat and facing up
        Vector3 vertex1 = new Vector3(Xaxis, Yaxis, Zaxis + 1f);     //top-left
        Vector3 vertex2 = new Vector3(Xaxis + 1f, Yaxis, Zaxis + 1f); //top-right
        Vector3 vertex3 = new Vector3(Xaxis, Yaxis, Zaxis);          //bottom-left
        Vector3 vertex4 = new Vector3(Xaxis + 1f, Yaxis, Zaxis);     //bottom-right

        // best to build the world up in quads?
        CreateQuad(vertex1, vertex2, vertex3, vertex4, Color.blue);
    }

    // Update is called once per frame
    void Update()
    {

    }
}