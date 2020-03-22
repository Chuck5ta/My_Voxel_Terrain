/*
 * The world is made up of cubes, although technically not cubes or even cuboids as not all angles are necessarily right angles. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube
{
    public GameObject cube;

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
    

    public Material defaultMaterial = CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad); // default material is dirt

    public PlanetGen planet;

    public int currentX, currentY, currentZ;

    //    public Quad[,,] quadData;


    // Cube contructor
    public Cube(Vector3[,,] planetVertices, int currentX, int currentY, int currentZ, Material material, int terrainType, Vector3 cubePosition, string chunkName)
    {
        cubeLocation = cubePosition;
        cube = new GameObject(chunkName + "_" + "Cube_" + Universe.BuildPlanetChunkName(cubeLocation));
        this.currentX = currentX;
        this.currentY = currentY;
        this.currentZ = currentZ;
  //      cube.name = "Cube_" + currentX + "_" + currentY + "_" + currentZ; // actual loacation within the 3D game world as well as in the array
        cube.transform.position = cubeLocation;

        GenerateFrontQuad();
        GenerateTopQuad();
        GenerateBottomQuad();
        GenerateBackQuad();
        GenerateLeftQuad();
        GenerateRightQuad();
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
    
    void GenerateFrontQuad()
    {
        // Front quad
        frontQuadVertices[0] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z);
        frontQuadVertices[1] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z);
        frontQuadVertices[2] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z);
        frontQuadVertices[3] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z);

        DisplayQuad(frontQuadVertices, "_Front_quad");
    }
    void GenerateTopQuad()
    {
        // Top quad
        topQuadVertices[0] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z);
        topQuadVertices[1] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z + 1);
        topQuadVertices[2] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z);
        topQuadVertices[3] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z + 1);

        DisplayQuad(topQuadVertices, "_Top_quad");
    }
    void GenerateBottomQuad()
    {
        // Bottom quad
        bottomQuadVertices[0] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z);
        bottomQuadVertices[1] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z + 1);
        bottomQuadVertices[2] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z);
        bottomQuadVertices[3] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z + 1);

        DisplayQuad(bottomQuadVertices, "_Bottom_quad");
    }
    void GenerateBackQuad()
    {
        // Back quad
        backQuadVertices[0] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z + 1);
        backQuadVertices[1] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z + 1);
        backQuadVertices[2] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z + 1);
        backQuadVertices[3] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z + 1);

        DisplayQuad(backQuadVertices, "_Back_quad");
    }
    void GenerateLeftQuad()
    {
        // Left quad
        leftQuadVertices[0] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z + 1);
        leftQuadVertices[1] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z + 1);
        leftQuadVertices[2] = new Vector3(cubeLocation.x, cubeLocation.y, cubeLocation.z);
        leftQuadVertices[3] = new Vector3(cubeLocation.x, cubeLocation.y + 1, cubeLocation.z);

        DisplayQuad(leftQuadVertices, "_Left_quad");
    }
    void GenerateRightQuad()
    {
        // Right quad
        rightQuadVertices[0] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z);
        rightQuadVertices[1] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z);
        rightQuadVertices[2] = new Vector3(cubeLocation.x + 1, cubeLocation.y, cubeLocation.z + 1);
        rightQuadVertices[3] = new Vector3(cubeLocation.x + 1, cubeLocation.y + 1, cubeLocation.z + 1);

        DisplayQuad(rightQuadVertices, "_Right_quad");
    }
    public void DisplayQuad(Vector3[] quadVertices, string quadName)
    {
        Vector3 quadPosition = new Vector3(cube.transform.position.x,
                                            cube.transform.position.y,
                                            cube.transform.position.z);
        Quad newQuad = new Quad(quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3],
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad, quadPosition);
        newQuad.Draw(cube.name + quadName); // TODO: need to name the quad!!!
    }



    public void DisplayRightQuad(Vector3[] quadVertices)
    {
        Vector3 quadPosition = new Vector3(cube.transform.position.x,
                                            cube.transform.position.y,
                                            cube.transform.position.z);
        Quad newQuad = new Quad(quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3],
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad, quadPosition);
        newQuad.Draw(cube.name + "_Right_quad"); // TODO: need to name the quad!!!
    }



    private void GenerateCube(int X, int Y, int Z, Vector3 cubePosition)
    {
        /*

        // Front quad
        chunkVertices[X, Y, Z] = new Vector3(chunkXIndex + X, chunkYIndex + Y, chunkZIndex + Z);
        chunkVertices[X, Y + 1, Z] = new Vector3(chunkXIndex + X, chunkYIndex + 1 + Y, chunkZIndex + Z);
        chunkVertices[X + 1, Y, Z] = new Vector3(chunkXIndex + 1 + X, chunkYIndex + Y, chunkZIndex + Z);
        chunkVertices[X + 1, Y + 1, Z] = new Vector3(chunkXIndex + 1 + X, chunkYIndex + 1 + Y, chunkZIndex + Z);

        // Top quad
        chunkVertices[X, Y + 1, Z] = new Vector3(chunkXIndex, Y + 1, Z);
        chunkVertices[X, Y + 1, Z + 1] = new Vector3(chunkXIndex, Y + 1, Z + 1);
        chunkVertices[X + 1, Y + 1, Z] = new Vector3(chunkXIndex + 1, Y + 1, Z);
        chunkVertices[X + 1, Y + 1, Z + 1] = new Vector3(chunkXIndex + 1, Y + 1, Z + 1);

        // Bottom quad
        chunkVertices[X + 1, Y, Z] = new Vector3(X + 1, Y, Z);
        chunkVertices[X + 1, Y, Z + 1] = new Vector3(X + 1, Y, Z + 1);
        chunkVertices[X, Y, Z] = new Vector3(X, Y, Z);
        chunkVertices[X, Y, Z + 1] = new Vector3(X, Y, Z + 1);

        // Back quad
        chunkVertices[X + 1, Y, Z + 1] = new Vector3(X + 1, Y, Z + 1);
        chunkVertices[X + 1, Y + 1, Z + 1] = new Vector3(X + 1, Y + 1, Z + 1);
        chunkVertices[X, Y, Z + 1] = new Vector3(X, Y, Z + 1);
        chunkVertices[X, Y + 1, Z + 1] = new Vector3(X, Y + 1, Z + 1);

        // Left quad
        chunkVertices[X, Y, Z + 1] = new Vector3(X, Y, Z + 1);
        chunkVertices[X, Y + 1, Z + 1] = new Vector3(X, Y + 1, Z + 1);
        chunkVertices[X, Y, Z] = new Vector3(X, Y, Z);
        chunkVertices[X, Y + 1, Z] = new Vector3(X, Y + 1, Z);

        // Right quad
        chunkVertices[X + 1, Y, Z] = new Vector3(X + 1, Y, Z);
        chunkVertices[X + 1, Y + 1, Z] = new Vector3(X + 1, Y + 1, Z);
        chunkVertices[X + 1, Y, Z + 1] = new Vector3(X + 1, Y, Z + 1);
        chunkVertices[X + 1, Y + 1, Z + 1] = new Vector3(X + 1, Y + 1, Z + 1);

        // STORE THE ABOVE AS THE FIRST CUBE
        // store front quad
        chunkData[X, Y, Z].storeFrontQuadData(chunkVertices[X, Y, Z],
                                            chunkVertices[X, Y + 1, Z],
                                            chunkVertices[X + 1, Y, Z],
                                            chunkVertices[X + 1, Y + 1, Z]);
        // store top quad
        chunkData[X, Y, Z].storeTopQuadData(chunkVertices[X, Y + 1, Z],
                                            chunkVertices[X, Y + 1, Z + 1],
                                            chunkVertices[X + 1, Y + 1, Z],
                                            chunkVertices[X + 1, Y + 1, Z + 1]);
        // store bottom quad
        chunkData[X, Y, Z].storeBottomQuadData(chunkVertices[X + 1, Y, Z],
                                            chunkVertices[X + 1, Y, Z + 1],
                                            chunkVertices[X, Y, Z],
                                            chunkVertices[X, Y, Z + 1]);
        // store back quad
        chunkData[X, Y, Z].storeBackQuadData(chunkVertices[X + 1, Y, Z + 1],
                                            chunkVertices[X + 1, Y + 1, Z + 1],
                                            chunkVertices[X, Y, Z + 1],
                                            chunkVertices[X, Y + 1, Z + 1]);
        // store left quad
        chunkData[X, Y, Z].storeLeftQuadData(chunkVertices[X, Y, Z + 1],
                                            chunkVertices[X, Y + 1, Z + 1],
                                            chunkVertices[X, Y, Z],
                                            chunkVertices[X, Y + 1, Z]);
        // store right quad
        chunkData[X, Y, Z].storeRightQuadData(chunkVertices[X + 1, Y, Z],
                                            chunkVertices[X + 1, Y + 1, Z],
                                            chunkVertices[X + 1, Y, Z + 1],
                                            chunkVertices[X + 1, Y + 1, Z + 1]);

        */
    }


    // store front quad data
    public void storeFrontQuadData(Vector3 quadVertex0, Vector3 quadVertex1, Vector3 quadVertex2, Vector3 quadVertex3)
    {
        frontQuadVertices[0] = new Vector3(quadVertex0.x, quadVertex0.y, quadVertex0.z);
        frontQuadVertices[1] = new Vector3(quadVertex1.x, quadVertex1.y, quadVertex1.z);
        frontQuadVertices[2] = new Vector3(quadVertex2.x, quadVertex2.y, quadVertex2.z);
        frontQuadVertices[3] = new Vector3(quadVertex3.x, quadVertex3.y, quadVertex3.z);

        // TEST
        DisplayFrontQuad(frontQuadVertices); // TODO: need to name the quad!!!
    }
    // store top quad data
    public void storeTopQuadData(Vector3 quadVertex0, Vector3 quadVertex1, Vector3 quadVertex2, Vector3 quadVertex3)
    {
        topQuadVertices[0] = new Vector3(quadVertex0.x, quadVertex0.y, quadVertex0.z);
        topQuadVertices[1] = new Vector3(quadVertex1.x, quadVertex1.y, quadVertex1.z);
        topQuadVertices[2] = new Vector3(quadVertex2.x, quadVertex2.y, quadVertex2.z);
        topQuadVertices[3] = new Vector3(quadVertex3.x, quadVertex3.y, quadVertex3.z);

        // TEST
        DisplayTopQuad(topQuadVertices); // TODO: need to name the quad!!!
    }
    // store bottoms quad data
    public void storeBottomQuadData(Vector3 quadVertex0, Vector3 quadVertex1, Vector3 quadVertex2, Vector3 quadVertex3)
    {
        bottomQuadVertices[0] = new Vector3(quadVertex0.x, quadVertex0.y, quadVertex0.z);
        bottomQuadVertices[1] = new Vector3(quadVertex1.x, quadVertex1.y, quadVertex1.z);
        bottomQuadVertices[2] = new Vector3(quadVertex2.x, quadVertex2.y, quadVertex2.z);
        bottomQuadVertices[3] = new Vector3(quadVertex3.x, quadVertex3.y, quadVertex3.z);

        // TEST
        DisplayBottomQuad(bottomQuadVertices); // TODO: need to name the quad!!!
    }
    // store back quad data
    public void storeBackQuadData(Vector3 quadVertex0, Vector3 quadVertex1, Vector3 quadVertex2, Vector3 quadVertex3)
    {
        backQuadVertices[0] = new Vector3(quadVertex0.x, quadVertex0.y, quadVertex0.z);
        backQuadVertices[1] = new Vector3(quadVertex1.x, quadVertex1.y, quadVertex1.z);
        backQuadVertices[2] = new Vector3(quadVertex2.x, quadVertex2.y, quadVertex2.z);
        backQuadVertices[3] = new Vector3(quadVertex3.x, quadVertex3.y, quadVertex3.z);

        // TEST
        DisplayBackQuad(backQuadVertices); // TODO: need to name the quad!!!
    }
    // store left quad data
    public void storeLeftQuadData(Vector3 quadVertex0, Vector3 quadVertex1, Vector3 quadVertex2, Vector3 quadVertex3)
    {
        leftQuadVertices[0] = new Vector3(quadVertex0.x, quadVertex0.y, quadVertex0.z);
        leftQuadVertices[1] = new Vector3(quadVertex1.x, quadVertex1.y, quadVertex1.z);
        leftQuadVertices[2] = new Vector3(quadVertex2.x, quadVertex2.y, quadVertex2.z);
        leftQuadVertices[3] = new Vector3(quadVertex3.x, quadVertex3.y, quadVertex3.z);

        // TEST
        DisplayLeftQuad(leftQuadVertices); // TODO: need to name the quad!!!
    }
    // store right quad data
    public void storeRightQuadData(Vector3 quadVertex0, Vector3 quadVertex1, Vector3 quadVertex2, Vector3 quadVertex3)
    {
        rightQuadVertices[0] = new Vector3(quadVertex0.x, quadVertex0.y, quadVertex0.z);
        rightQuadVertices[1] = new Vector3(quadVertex1.x, quadVertex1.y, quadVertex1.z);
        rightQuadVertices[2] = new Vector3(quadVertex2.x, quadVertex2.y, quadVertex2.z);
        rightQuadVertices[3] = new Vector3(quadVertex3.x, quadVertex3.y, quadVertex3.z);

        // TEST
        DisplayRightQuad(rightQuadVertices); // TODO: need to name the quad!!!
    }


    public void DisplayFrontQuad(Vector3[] quadVertices)
    {
        Vector3 quadPosition = new Vector3(cube.transform.position.x,
                                            cube.transform.position.y,
                                            cube.transform.position.z);
        Quad newQuad = new Quad(quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3],
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad, cube.transform.position);
        newQuad.Draw(cube.name + "_Front_quad"); // TODO: need to name the quad!!!
    }
    public void DisplayTopQuad(Vector3[] quadVertices)
    {
        Vector3 quadPosition = new Vector3(cube.transform.position.x,
                                            cube.transform.position.y,
                                            cube.transform.position.z);
        Quad newQuad = new Quad(quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3],
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad, quadPosition);
        newQuad.Draw(cube.name + "_Top_quad"); // TODO: need to name the quad!!!
    }
    public void DisplayBottomQuad(Vector3[] quadVertices)
    {
        Vector3 quadPosition = new Vector3(cube.transform.position.x,
                                            cube.transform.position.y,
                                            cube.transform.position.z);
        Quad newQuad = new Quad(quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3],
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad, quadPosition);
        newQuad.Draw(cube.name + "_Bottom_quad"); // TODO: need to name the quad!!!
    }
    public void DisplayBackQuad(Vector3[] quadVertices)
    {
        Vector3 quadPosition = new Vector3(cube.transform.position.x,
                                            cube.transform.position.y,
                                            cube.transform.position.z);
        Quad newQuad = new Quad(quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3],
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad, quadPosition);
        newQuad.Draw(cube.name + "_Back_quad"); // TODO: need to name the quad!!!
    }
    public void DisplayLeftQuad(Vector3[] quadVertices)
    {
        Vector3 quadPosition = new Vector3(cube.transform.position.x,
                                            cube.transform.position.y,
                                            cube.transform.position.z);
        Quad newQuad = new Quad(quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3],
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad, quadPosition);
        newQuad.Draw(cube.name + "_Left_quad"); // TODO: need to name the quad!!!
    }



    // check for which ones are active/visible
    // set those up (visible, material, etc.)

    // set the others to not visible


    public void LocateBottomQuad(Vector3[,,] planetVertices, int distanceBetweenVertices)
    {
        // quad starting at the coords passed into the cube constructor
        if (planetVertices[currentX + distanceBetweenVertices, currentY, currentZ].x != -1 &&
            planetVertices[currentX, currentY, currentZ + distanceBetweenVertices].x != -1 &&
            planetVertices[currentX + distanceBetweenVertices, currentY, currentZ + distanceBetweenVertices].x != -1)
        {
            Debug.Log("Bottom quad located @ " + currentX + "," + currentY + "," + currentZ);
        }
    }


    /*
     * This finds the quads the current vertex is part of
     */
    public void LocateAndDrawQuads(Vector3[,,] planetVertices, int distanceBetweenVertices, int currentX, int currentY, int currentZ)
    {
        // FIND THE QUADS
        // X + 1 Y + 1 Z
        // X - 1 Y - 1 Z
        // X + 1 Y - 1 Z
        // X - 1 Y + 1 Z
        // X Y Y+1 Z + 1
        // X Y Y + 1 Z - 1
        // X Y Y - 1 Z + 1
        // X Y Y - 1 Z - 1
        // Directly above
        // X Y+1 Z

        // NEED TO ID WHERE THE QUAD IS - TOP, BOTTOM, FRONT, BACK, LEFT SIDE, RIGHT SIDE

        // Horizontal plane (PosX PosZ)  quad - **** this should locate the bottom and top quads of a cube ****
        // ====================================================================================================
        // (X,Y,Z) (X+1, Y, Z) (X, Y, Z+1) (X+1, Y, Z+1)
        if (currentX + (distanceBetweenVertices - 1) < planet.planetSize + 10 &&
            //              currentY + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentZ + (distanceBetweenVertices - 1) < planet.planetSize + 10 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ].x != -1 &&
            planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1].x != -1 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1].x != -1)
        {
            Debug.Log("Bottom quad located @ " + currentX + "," + currentY + "," + currentZ);
            // BOTTOM SIDE QUAD
            if (currentY < planet.universeSize / 2) // in the bottom half of the planet
            {
                bottomQuad = new Quad(Side.Bottom, 
                                        planetVertices[currentX, currentY, currentZ],
                                        planetVertices[currentX + distanceBetweenVertices, currentY, currentZ],
                                        planetVertices[currentX, currentY, currentZ + distanceBetweenVertices],
                                        planetVertices[currentX + distanceBetweenVertices, currentY, currentZ + distanceBetweenVertices],
                                        cube, // WTF is this???
                                        this, // WTF is this???
                                        CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                        CustomMaterials.rockQuad);
                Debug.Log("Draw quad @ " + currentX + "," + currentY + "," + currentZ);
                Debug.Log("distanceBetweenVertices : " + distanceBetweenVertices);

                Debug.Log("VERTICES 0 : " + planetVertices[currentX, currentY, currentZ]);
                Debug.Log("VERTICES 1 : " + planetVertices[currentX + distanceBetweenVertices-1, currentY, currentZ]);
                Debug.Log("VERTICES 2 : " + planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1]);
                Debug.Log("VERTICES 3 : " + planetVertices[currentX + distanceBetweenVertices-1, currentY, currentZ + distanceBetweenVertices - 1]);
                bottomQuad.Draw("TODO");

    //            DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
    //                           planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
    //                           planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
    //                           planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1]);
            }
            else // TOP SIDE QUAD
            {
 //               DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
 //                               planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
 //                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
 //                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1]);
            }

            //        Debug.Log("Horizontal plane quad: " + currentX + "," + currentY + "," + currentZ);
        }

        // Vertical plane (PosX PosY) quad - **** this should locate the front and back quads of a cube ****
        // =================================================================================================
        // (X,Y,Z) (X+1, Y, Z) (X, Y+1, Z) (X+1, Y+1, Z)
        if (currentX + (distanceBetweenVertices - 1) < planet.universeSize &&
            currentY + (distanceBetweenVertices - 1) < planet.universeSize &&
            //currentZ + (distanceBetweenVertices - 1) < planetSize + 10 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ].x != -1 &&
            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ].x != -1 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY + distanceBetweenVertices - 1, currentZ].x != -1)
        {
            // FRONT SIDE QUAD
            if (currentZ < planet.universeSize / 2) // in the front half of the planet
            {
   //             DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
   //                            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
   //                            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
   //                            planetVertices[currentX + distanceBetweenVertices - 1, currentY + distanceBetweenVertices - 1, currentZ]);
            }
            else // BACK SIDE QUAD
            {
  //              DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
   //                            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
   //                            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
   //                            planetVertices[currentX + distanceBetweenVertices - 1, currentY + distanceBetweenVertices - 1, currentZ]);
            }
            //         Debug.Log("Vetical plane quad: " + currentX + "," + currentY + "," + currentZ);
        }

        // Side planes (PosY PosZ) quad - **** this should locate the left and right side quads of a cube ****
        // ===================================================================================================
        // (X,Y,Z) (X, Y+1, Z) (X, Y, Z+1) (X, Y+1, Z+1)
        if (//currentX + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentY + (distanceBetweenVertices - 1) < planet.universeSize &&
            currentZ + (distanceBetweenVertices - 1) < planet.universeSize &&
            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ].x != -1 &&
            planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1].x != -1 &&
            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ + distanceBetweenVertices - 1].x != -1)
        {
            // LEFT SIDE QUAD
            if (currentX < planet.universeSize / 2) // in the left half of the planet
            {
    //            DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
    //                           planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
    //                           planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
    //                           planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ + distanceBetweenVertices - 1]);
            }
            else // RIGHT SIDE QUAD
            {
    //            DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
    //                           planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
    //                           planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
    //                           planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ + distanceBetweenVertices - 1]);
            }
            //        Debug.Log("Vetical SIDE plane quad: " + currentX + "," + currentY + "," + currentZ);
        }
    }



}
