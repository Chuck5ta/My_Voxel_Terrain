/*
 * 31 JAN 2020
 * Build the terrain out of quads (a quad will be made up of 2 triangles in the game world)
 *  
 * I don't think this needs cubes or any 3D objects for the terrain. You just alter the vertex coordinates and rerender?
 * May need to add remove geometry (triangles or quads)
 * 
 * https://answers.unity.com/questions/154324/how-do-uvs-work.html
 * 
 * 
 */

using UnityEngine;


public class Quad
{
    private Material quadMaterial;

    //    private float Xaxis = 0f; // defaulted to the centre of the world
    //    private float Yaxis = 0f; // defaulted to the centre of the world
    //    private float Zaxis = 0f; // defaulted to the centre of the world

    Chunk owner; // so that we can access the chunkData array
    GameObject parent; // The chunk
    Vector2 position; // location within the chunk
                      // this is used to give the quad a unique name

    Vector3 vertex0, vertex1, vertex2, vertex3;

    /* 
     * Quad constructor
     * position is the location within the chunk
     * parent is the chunk
     * material is the material (texture)
     * vertex0, vertex1, vertex2, vertex3 is the quad's location in the world 
     * 
     */
    public Quad(Vector2 locationInChunk, Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, GameObject parent, Chunk owner, Material material)
    {
        this.owner = owner;
        this.parent = parent;
        this.position = locationInChunk; // position within the chunk
        this.vertex0 = vertex0;
        this.vertex1 = vertex1;
        this.vertex2 = vertex2;
        this.vertex3 = vertex3;
        quadMaterial = material;
    }

    public void CreateQuad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        // position = location within the chunk
        // vertices = location of each vertex of the quad within the game world

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        vertices[0] = vertex0; //top-left
        vertices[1] = vertex1; //top-right
        vertices[2] = vertex2; //bottom-left
        vertices[3] = vertex3; //bottom-right

        mesh.vertices = vertices;

        int[] triangles = new int[6] { 0, 1, 2, 3, 2, 1 };
        mesh.triangles = triangles;

        Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(0, 1); //top-left
        uvs[1] = new Vector2(1, 1); //top-right
        uvs[2] = new Vector2(0, 0); //bottom-left
        uvs[3] = new Vector2(1, 0); //bottom-right

        mesh.uv = uvs;

        Vector3[] normals = new Vector3[4] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        mesh.normals = normals;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("Quad");
        quad.name = "Quad_" + position.x + "_" + position.y;
    //    quad.transform.position = position; // set the quad's location in the chunk
        quad.transform.parent = this.parent.transform;
        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = quadMaterial;
    }

    // Kick off creating the quad mesh
    // this is called from within the chunk code
    public void Draw()
    {
        // best to build the world up in quads?
        CreateQuad(vertex0, vertex1, vertex2, vertex3);
    }
}