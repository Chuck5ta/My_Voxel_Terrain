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

public class Quad
{
    public Material quadMaterial;

    private float Xaxis = 0f; // defaulted to the centre of the world
    private float Yaxis = 0f; // defaulted to the centre of the world
    private float Zaxis = 0f; // defaulted to the centre of the world

    GameObject parent; // The chunk
    Vector3 position; // location within the chunk

    /* 
     * Quad constructor
     * position is the location within the chunk
     * parent is the chunk
     * material is the material (texture)
     * 
     */
    public Quad(Vector3 position, GameObject parent, Material material)
    {
        this.parent = parent;
        this.position = position;
        this.quadMaterial = material;
    }

    public void CreateQuad(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4, Color meshColour)
    {
        Mesh mesh = new Mesh();
        mesh.name = "Quad_" + position.x + "_" + position.y + "_" + position.z;

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

        GameObject quad = new GameObject("Quad");
        quad.transform.position = position;
        quad.transform.parent = parent.transform;
        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = quadMaterial;
    }

    // Kick off creating the quad mesh
    // this is called from within the chunk code
    public void Draw()
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