/*
 * The world is made up of cubes, although technically not cubes or even cuboids as not all angles are necessarily right angles. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube
{
    public GameObject cube;
    public PlanetChunk parentChunk;

    public Vector3 cubeLocation;

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
    Quad frontQuad, backQuad, topQuad, bottomQuad, leftQuad, rightQuad;

    public enum Side { Front, Back, Top, Bottom, Leftside, Rightside }
    
    public enum CubePhysicalState { SOLID, SPACE }
    private CubePhysicalState cubePhysicalState;


    public Material defaultMaterial = CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad); // default material is dirt

    public PlanetGen planet;

    public int currentX, currentY, currentZ;

    //    public Quad[,,] quadData;


    // Cube contructor
    public Cube(Vector3[,,] planetVertices, int currentX, int currentY, int currentZ, Material material, 
        int terrainType, Vector3 cubePosition, string chunkName, PlanetChunk parent)
    {
        cubeLocation = cubePosition;
        this.parentChunk = parent;
        cubePhysicalState = CubePhysicalState.SOLID; // default state
        cube = new GameObject(chunkName + "_" + "Cube_" + Universe.BuildPlanetChunkName(cubeLocation));
        this.currentX = currentX;
        this.currentY = currentY;
        this.currentZ = currentZ;
        cube.transform.position = cubeLocation;
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
            if (parentChunk.chunkData[x, y, z].GetPhysicalState() == CubePhysicalState.SOLID)
                return true;
        }
        catch(System.IndexOutOfRangeException ex) { }

        return false; // cube is air, water, or similar
    }


    /*
    public Cube(PlanetGen planet, Vector3[,,] planetVertices, int distanceBetweenVertices, int currentX, int currentY, int currentZ, Material material, int terrainType)
    {
        cube = new GameObject("Cube");
        this.planet = planet;
        this.currentX = currentX;
        this.currentY = currentY;
        this.currentZ = currentZ;
        cube.name = "Cube_" + currentX + "_" + currentY + "_" + currentZ; // actual loacation within the 3D game world as well as in the array

        LocateBottomQuad(planetVertices, distanceBetweenVertices);
    }
    */

    public void GenerateFrontQuad()
    {
        // Front quad
        frontQuadVertices[0] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z);
        frontQuadVertices[1] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z);
        frontQuadVertices[2] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z);
        frontQuadVertices[3] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z);

        DisplayQuad(frontQuadVertices, "_Front_quad", CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad));
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

        DisplayQuad(backQuadVertices, "_Back_quad", CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad));
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
        Vector3 quadPosition = new Vector3(cube.transform.position.x,
                                            cube.transform.position.y,
                                            cube.transform.position.z);
        Quad newQuad = new Quad(quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3],
                                material,
                                CustomMaterials.rockQuad, quadPosition);
        newQuad.Draw(cube.name + quadName); // TODO: need to name the quad!!!
    }


}
