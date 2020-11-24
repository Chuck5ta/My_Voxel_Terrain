/*
 * The world is made up of cubes, although technically not cubes or even cuboids as not all angles are necessarily right angles. 
 */
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Cube
{
    public GameObject cube;
    public GameObject parent;
    public PlanetChunk owner;

    public Vector3 cubeLocation;

    private Material cubeMaterial = CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad);

    public Vector3[] frontQuadVertices = new Vector3[4];
    public Vector3[] backQuadVertices = new Vector3[4];
    public Vector3[] topQuadVertices = new Vector3[4];
    public Vector3[] bottomQuadVertices = new Vector3[4];
    public Vector3[] leftQuadVertices = new Vector3[4];
    public Vector3[] rightQuadVertices = new Vector3[4];

    // Front quad
    // Back quad
    // Top quad
    // Bottom quad
    // Leftside quad
    // Rightside quad
 //   Quad frontQuad, backQuad, topQuad, bottomQuad, leftQuad, rightQuad; // TODO: Delete when no longer required

//    public enum Side { Front, Back, Top, Bottom, Leftside, Rightside }
    
    public enum CubePhysicalState { SOLID, SPACE }
    private CubePhysicalState cubePhysicalState;

    public Material defaultMaterial = CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad); // default material is dirt

    public PlanetGen planet;

    public int currentX, currentY, currentZ;


    // Cube contructor
    public Cube(GameObject parent, PlanetChunk owner,
        int currentX, int currentY, int currentZ, 
        Material material, int terrainType, 
        Vector3 cubePosition, string chunkName)
    {
        cubeLocation = cubePosition;
        this.parent = parent;
        this.owner = owner;
        cubePhysicalState = CubePhysicalState.SOLID; // default state
        cube = new GameObject(chunkName + "_" + "Cube_" + Universe.BuildPlanetChunkName(cubeLocation));
        this.currentX = currentX;
        this.currentY = currentY;
        this.currentZ = currentZ;
    }

    public void SetPhysicalState(CubePhysicalState physicalState)
    {
        cubePhysicalState = physicalState;
    }
    public CubePhysicalState GetPhysicalState()
    {
        return cubePhysicalState;
    }

    public void DrawCube()
    {
        // if neighbouring cube is SPACE, then draw the quad
        if(!HasSolidNeighbour(currentX, currentY, currentZ-1))
            GenerateFrontQuad();
        if (!HasSolidNeighbour(currentX, currentY+1, currentZ))
            GenerateTopQuad();
        if (!HasSolidNeighbour(currentX, currentY-1, currentZ))
            GenerateBottomQuad();
        if (!HasSolidNeighbour(currentX, currentY, currentZ+1))
            GenerateBackQuad();
        if (!HasSolidNeighbour(currentX-1, currentY, currentZ))
            GenerateLeftQuad();
        if (!HasSolidNeighbour(currentX+1, currentY, currentZ))
            GenerateRightQuad();
    }

    public bool HasSolidNeighbour(int x, int y, int z)
    {
     //   Cube[,,] cube = parent.GetComponent<PlanetChunk>().chunkData;
        try
        {
            if (owner.CubeIsSolid[x, y, z])
                return true;
        }
        catch(System.IndexOutOfRangeException ex) { }

        return false; // cube is air, water, or similar
    }

    public void GenerateFrontQuad()
    {
        // Front quad
        frontQuadVertices[0] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z);
        frontQuadVertices[1] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z);
        frontQuadVertices[2] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z);
        frontQuadVertices[3] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z);

        DisplayQuad(frontQuadVertices, "_Front_quad", CustomMaterials.RetrieveMaterial(CustomMaterials.sandQuad));
    }
    void GenerateTopQuad()
    {
        // Top quad
        topQuadVertices[0] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z);
        topQuadVertices[1] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z + 1);
        topQuadVertices[2] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z);
        topQuadVertices[3] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z + 1);

        DisplayQuad(topQuadVertices, "_Top_quad", CustomMaterials.RetrieveMaterial(CustomMaterials.grassQuad));
    }
    void GenerateBottomQuad()
    {
        // Bottom quad
        bottomQuadVertices[0] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z);
        bottomQuadVertices[1] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z + 1);
        bottomQuadVertices[2] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z);
        bottomQuadVertices[3] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z + 1);

        DisplayQuad(bottomQuadVertices, "_Bottom_quad", CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad));
    }
    void GenerateBackQuad()
    {
        // Back quad
        backQuadVertices[0] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z + 1);
        backQuadVertices[1] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z + 1);
        backQuadVertices[2] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z + 1);
        backQuadVertices[3] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z + 1);

        DisplayQuad(backQuadVertices, "_Back_quad", CustomMaterials.RetrieveMaterial(CustomMaterials.sandQuad));
    }
    void GenerateLeftQuad()
    {
        // Left quad
        leftQuadVertices[0] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z + 1);
        leftQuadVertices[1] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z + 1);
        leftQuadVertices[2] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z);
        leftQuadVertices[3] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z);

        DisplayQuad(leftQuadVertices, "_Left_quad", CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad));
    }
    void GenerateRightQuad()
    {
        // Right quad
        rightQuadVertices[0] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z);
        rightQuadVertices[1] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z);
        rightQuadVertices[2] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z + 1);
        rightQuadVertices[3] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z + 1);

        DisplayQuad(rightQuadVertices, "_Right_quad", CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad));
    }
    public void DisplayQuad(Vector3[] quadVertices, string quadName, Material material)
    {
        CreateQuad(cube.name + quadName,
            quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3],
            material, CustomMaterials.rockQuad); // TODO: need to name the quad!!!
    }
        
    public void CreateQuad(string quadName,
            Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,
            Material material, int terrainType)
    {
        // position = location within the chunk
        // vertices = location of each vertex of the quad within the game world

        Mesh mesh = new Mesh();

        Vector3[] vertices = { vertex0,   //top-left
                               vertex1,   //top-right
                               vertex2,   //bottom-left
                               vertex3 }; //bottom-right

        mesh.vertices = vertices;

        int[] triangles = new int[6] { 0, 1, 2, 3, 2, 1 };
        mesh.triangles = triangles;

        Vector2[] uvs = { new Vector2(0f, 1f),   //top-left
                          new Vector2(1f, 1f),   //top-right
                          new Vector2(0f, 0f),   //bottom-left
                          new Vector2(1f, 0f) }; //bottom-right

        mesh.uv = uvs;

        Vector3[] normals = new Vector3[4] 
            { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        mesh.normals = normals;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject(quadName);
        //      quad.name = "Quad_" + position.x + "_" + position.y + "_" + position.z;
        quad.name = quadName;
    //    quad.transform.position = quadLocation;
    //    quad.transform.position = cubeLocation; // set the quad's location in the chunk | Do not undrawing quadcomment!!! it will override the coordinates we want the quad to have!
        quad.transform.parent = parent.transform; // make the quad a child of the cube

        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        //    MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        //    MeshCollider boxCollider2 = quad.AddComponent<MeshCollider>();
        //    renderer.material = quadMaterial;
    }
}
