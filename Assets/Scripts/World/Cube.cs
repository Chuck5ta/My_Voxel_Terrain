/*
 * The world is made up of cubes, although technically not cubes or even cuboids as not all angles are necessarily right angles. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube
{
    public GameObject cube;
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

//    public Quad[,,] quadData;


    // Cube contructor
    public Cube(PlanetGen planet, Vector3[,,] planetVertices, int distanceBetweenVertices, int currentX, int currentY, int currentZ, Material material, int terrainType)
    {
        cube = new GameObject("Cube");
        this.planet = planet;
        cube.name = "Cube_" + currentX + "_" + currentY + "_" + currentZ; // actual loacation within the 3D game world as well as in the array

        LocateAndDrawQuads(planetVertices, distanceBetweenVertices, currentX, currentY, currentZ);
    }

    // check for which ones are active/visible
    // set those up (visible, material, etc.)

    // set the others to not visible



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
                Debug.Log("VERTICES 1 : " + planetVertices[currentX + distanceBetweenVertices+2, currentY, currentZ]);
                Debug.Log("VERTICES 2 : " + planetVertices[currentX, currentY, currentZ + distanceBetweenVertices + 2]);
                Debug.Log("VERTICES 3 : " + planetVertices[currentX + distanceBetweenVertices + 2, currentY, currentZ + distanceBetweenVertices + 2]);
                bottomQuad.Draw();

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
