/*
 * This generates a planet from an initial cube (cube made up of verts only)
 * 
 * 
 * Useful links
 * ============
 * https://stackoverflow.com/questions/1695421/creating-spherical-meshes-with-direct-x
 * 
 * https://wiki.unity3d.com/index.php/ProceduralPrimitives
 * 
 * Deffo how to do this!
 * Icosphere
 * https://medium.com/@peter_winslow/creating-procedural-planets-in-unity-part-1-df83ecb12e91
 * Cube to sphere
 * https://catlikecoding.com/unity/tutorials/cube-sphere/
 */

using System;
using UnityEngine;

public class PlanetGen
{
    public int universeSize = 25; // 25 x 25 x 25
    // must always be an odd number, or otherwise subdivision fails
    public int planetSize = 5; // 100 x 100 x 100 world
    // bottom left corner of cube
    const int X = 10;
    const int Y = 10;
    const int Z = 10;

    public Vector3 planetCentre = new Vector3(12, 12, 12);

    // SUBDIVISION
    public int totalLayers = 3; // layers of vertices (2 layers in a cube)
    int distanceBetweenVertices = 0;

    private Material quadMaterial;

    public Vector3[,,] planetVertices;
    
    public Cube[,,] planetData; // 3D array to hold information on all of the cubes and their quads in the planet

    public GameObject planet;

    public Vector3 planetPosition;

    // The faces of the cube
    // - starting point for the generation of a sphere/planet
    public enum Face { Top, Bottom, Front, Back, LeftSide, RightSide }
    
    // generate globe 
    public PlanetGen(Vector3 planetPosition)
    {
        this.planetPosition = planetPosition;
        distanceBetweenVertices = planetSize;
        planet = new GameObject("Planet_" + planetPosition.x + "_" + planetPosition.y + "_" + planetPosition.z);
        planetData = new Cube[universeSize, universeSize, universeSize];
        planetVertices = new Vector3[universeSize, universeSize, universeSize];

        // initial planetVertices
        for (int i = 0; i < planetSize + 10; i++)
        {
            for (int k = 0; k < planetSize + 10; k++)
            {
                for (int l = 0; l < planetSize + 10; l++)
                {
                    planetVertices[i,k,l].x = -1;
                    planetVertices[i, k, l].y = -1;
                    planetVertices[i, k, l].z = -1;
                }
            }
        } 

        // Initialise planet as cube (8 vertices)
        // set the 4 initial vertices of the starting cube 
        //    these should be at the radius we want the sphere to be
        // Front
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y + planetSize-1, Z] = new Vector3(X, Y + planetSize - 1, Z);
        planetVertices[X + planetSize - 1, Y, Z] = new Vector3(X + planetSize - 1, Y, Z);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z);
        /*        DisplayInitialCube(planetVertices[X, Y, Z],
                                planetVertices[X, Y + planetSize - 1, Z],
                                planetVertices[X + planetSize - 1, Y, Z],
                                planetVertices[X + planetSize - 1, Y + planetSize - 1, Z]); */
        // Top
        planetVertices[X, Y + planetSize - 1, Z] = new Vector3(X, Y + planetSize - 1, Z);
        planetVertices[X, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X, Y + planetSize - 1, Z + planetSize - 1);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1);
        /*      DisplayInitialCube(planetVertices[X, Y + planetSize - 1, Z],
                              planetVertices[X, Y + planetSize - 1, Z + planetSize - 1],
                              planetVertices[X + planetSize - 1, Y + planetSize - 1, Z],
                              planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1]); */
        // Bottom
        planetVertices[X + planetSize - 1, Y, Z] = new Vector3(X + planetSize - 1, Y, Z);
        planetVertices[X + planetSize - 1, Y, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y, Z + planetSize - 1);
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y, Z + planetSize - 1] = new Vector3(X, Y, Z + planetSize - 1);
        /*       DisplayInitialCube(planetVertices[X + planetSize - 1, Y, Z],
                               planetVertices[X + planetSize - 1, Y, Z + planetSize - 1],
                               planetVertices[X, Y, Z],
                               planetVertices[X, Y, Z + planetSize - 1]); */
        // Back
        planetVertices[X + planetSize - 1, Y, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y, Z + planetSize - 1);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1);
        planetVertices[X, Y, Z + planetSize - 1] = new Vector3(X, Y, Z + planetSize - 1);
        planetVertices[X, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X, Y + planetSize - 1, Z + planetSize - 1);
        /*      DisplayInitialCube(planetVertices[X + planetSize - 1, Y, Z + planetSize - 1],
                              planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1],
                              planetVertices[X, Y, Z + planetSize - 1],
                              planetVertices[X, Y + planetSize - 1, Z + planetSize - 1]);*/
        // Left
        planetVertices[X, Y, Z + planetSize - 1] = new Vector3(X, Y, Z + planetSize - 1);
        planetVertices[X, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X, Y + planetSize - 1, Z + planetSize - 1);
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y + planetSize - 1, Z] = new Vector3(X, Y + planetSize - 1, Z);
              /*      DisplayInitialCube(planetVertices[X, Y, Z + planetSize - 1],
                                    planetVertices[X, Y + planetSize - 1, Z + planetSize - 1],
                                    planetVertices[X, Y, Z],
                                    planetVertices[X, Y + planetSize - 1, Z]); */
        // Right
        planetVertices[X + planetSize - 1, Y, Z] = new Vector3(X + planetSize - 1, Y, Z);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z);
        planetVertices[X + planetSize - 1, Y, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y, Z + planetSize - 1);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1);
        /*     DisplayInitialCube(planetVertices[X + planetSize - 1, Y, Z],
                             planetVertices[X + planetSize - 1, Y + planetSize - 1, Z],
                             planetVertices[X + planetSize - 1, Y, Z + planetSize - 1],
                             planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1]); */

        // Now subdivide quads


        Debug.Log("---===== SUBDIVISION STAGE =====---");
        Debug.Log("         =================");
        Subdivide();
        Debug.Log("---===== CUBE DRAWING STAGE =====---");
        Debug.Log("         ******************");
        DrawCubes();

        //    Debug.Log("Front **********************");
        // Front
        /*      subdivideQuad(planetVertices[X, Y, Z],
                              planetVertices[X, Y + planetZize, Z],
                              planetVertices[X + planetZize, Y, Z],
                              planetVertices[X + planetZize, Y + planetZize, Z],
                              planetZize,
                              Face.Front);

          //    Debug.Log("Top **********************");
              // Top
              subdivideQuad(planetVertices[X, Y + planetZize, Z],
                              planetVertices[X, Y + planetZize, Z + planetZize],
                              planetVertices[X + planetZize, Y + planetZize, Z],
                              planetVertices[X + planetZize, Y + planetZize, Z + planetZize],
                              planetZize,
                              Face.Top);

          //    Debug.Log("Bottom **********************");
              // Bottom
              subdivideQuad(planetVertices[X + planetZize, Y, Z],
                              planetVertices[X + planetZize, Y, Z + planetZize],
                              planetVertices[X, Y, Z],
                              planetVertices[X, Y, Z + planetZize],
                              planetZize,
                              Face.Bottom);

          //    Debug.Log("Back **********************");
              // Back
              subdivideQuad(planetVertices[X + planetZize, Y, Z + planetZize],
                              planetVertices[X + planetZize, Y + planetZize, Z + planetZize],
                              planetVertices[X, Y, Z + planetZize],
                              planetVertices[X, Y + planetZize, Z + planetZize],
                              planetZize,
                              Face.Back);

          //    Debug.Log("Left **********************");
              // Left
              subdivideQuad(planetVertices[X, Y, Z + planetZize],
                              planetVertices[X, Y + planetZize, Z + planetZize],
                              planetVertices[X, Y, Z],
                              planetVertices[X, Y + planetZize, Z],
                              planetZize,
                              Face.LeftSide);

          //    Debug.Log("Right **********************");
              // Right
              subdivideQuad(planetVertices[X + planetZize, Y, Z],
                              planetVertices[X + planetZize, Y + planetZize, Z],
                              planetVertices[X + planetZize, Y, Z + planetZize],
                              planetVertices[X + planetZize, Y + planetZize, Z + planetZize],
                              planetZize,
                              Face.RightSide);

              */

    }
    
   /*
    * This finds the quads the current vertex is part of
    */
    private void LocateAndDrawQuads(int currentY, int currentZ, int currentX)
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
        if (currentX + (distanceBetweenVertices - 1) < planetSize + 10 &&
            //              currentY + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentZ + (distanceBetweenVertices - 1) < planetSize + 10 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ].x != -1 &&
            planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1].x != -1 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1].x != -1)
        {

            // BOTTOM SIDE QUAD
            if (currentY < universeSize/2) // in the bottom half of the planet
            {
                DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
                               planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1]);
            }
            else // TOP SIDE QUAD
            {
                DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
                                planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
                                planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
                                planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1]);
            }

    //        Debug.Log("Horizontal plane quad: " + currentX + "," + currentY + "," + currentZ);
        }

        // Vertical plane (PosX PosY) quad - **** this should locate the front and back quads of a cube ****
        // =================================================================================================
        // (X,Y,Z) (X+1, Y, Z) (X, Y+1, Z) (X+1, Y+1, Z)
        if (currentX + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentY + (distanceBetweenVertices - 1) < planetSize + 10 &&
            //currentZ + (distanceBetweenVertices - 1) < planetSize + 10 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ].x != -1 &&
            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ].x != -1 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY + distanceBetweenVertices - 1, currentZ].x != -1)
        {
            // FRONT SIDE QUAD
            if (currentZ < universeSize / 2) // in the front half of the planet
            {
                DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY + distanceBetweenVertices - 1, currentZ]);
            }
            else // BACK SIDE QUAD
            {
                DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY + distanceBetweenVertices - 1, currentZ]);
            }
   //         Debug.Log("Vetical plane quad: " + currentX + "," + currentY + "," + currentZ);
        }

        // Side planes (PosY PosZ) quad - **** this should locate the left and right side quads of a cube ****
        // ===================================================================================================
        // (X,Y,Z) (X, Y+1, Z) (X, Y, Z+1) (X, Y+1, Z+1)
        if (//currentX + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentY + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentZ + (distanceBetweenVertices - 1) < planetSize + 10 &&
            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ].x != -1 &&
            planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1].x != -1 &&
            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ + distanceBetweenVertices - 1].x != -1)
        {
            // LEFT SIDE QUAD
            if (currentX < universeSize / 2) // in the left half of the planet
            {
                DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ + distanceBetweenVertices - 1]);
            }
            else // RIGHT SIDE QUAD
            {
                DisplayInitialCube(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
                               planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ + distanceBetweenVertices - 1]);
            }
    //        Debug.Log("Vetical SIDE plane quad: " + currentX + "," + currentY + "," + currentZ);
        }
    }


    /*
     * This finds the quads the current vertex is part of
     */
    private void CreateCube(int currentX, int currentY, int currentZ)
    {
        //     Cube newCube = new Cube(this, planetVertices, distanceBetweenVertices, currentX, currentY, currentZ,
        //                             CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
        //                             CustomMaterials.rockQuad);
   //     planetData[currentX, currentY, currentZ] = new Cube(this, planetVertices, distanceBetweenVertices, currentX, currentY, currentZ,
   //                             CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
   //                             CustomMaterials.rockQuad);
    }

    /*
     * This controls the drawing of a row (along the X axis), based on the vertices held in planetVertices
     */
    private void DrawRowOfCubes(int currentY, int currentZ)
    {
        for (int currentX = X; currentX < planetSize + 10; currentX += distanceBetweenVertices)
        { // locate quads along the X avis
            Debug.Log("X is : " + currentX);
            // IS THERE A VERTEX HERE?
            if (planetVertices[currentX, currentY, currentZ].x != -1)
            {
                Debug.Log("Vertex found at: " + currentX + "," + currentY + "," + currentZ + "," + " " + planetVertices[currentX, currentY, currentZ]);
                Debug.Log("Create Cube @ " + currentX + "," + currentY + "," + currentZ);
                CreateCube(currentX, currentY, currentZ); // currentY, currentZ, currentX will be the location in the 3D array planetData

                //       LocateAndDrawQuads(distanceBetweenVertices, currentY, currentZ, currentX);
                // can also have a locate and subdivide quads
            }
        }
    }

    /*
     * This controls the drawing of one layer of a planet's quads, based on the vertices held in planetVertices
     */
    private void DrawLayerOfCubes(int currentY)
    {
        for (int currentZ = Z; currentZ < planetSize + 10; currentZ += distanceBetweenVertices)
        {
            Debug.Log("Z is : " + currentZ);
            DrawRowOfCubes(currentY, currentZ);
        }
    }

    /*
     * This controls the drawing of a planet's vertices, based on the vertices held in planetVertices
     */
    public void DrawCubes()
    {
        // current X, current Y, current Z
        for (int currentY = Y; currentY < planetSize + 10; currentY += distanceBetweenVertices) // process only existing vertices
        {
            Debug.Log("Y is : " + currentY);
            DrawLayerOfCubes(currentY);
        }
    }


    // *******************************************
    //   SUCCESSFULLY LOCATED THE VERTICES!
    // *******************************************

    /*
     * This finds the quads the current vertex is part of
     */
    private void LocateAndSubdivideQuads(int currentY, int currentZ, int currentX)
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
        if (currentX + (distanceBetweenVertices - 1) < planetSize + 10 &&
            //              currentY + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentZ + (distanceBetweenVertices - 1) < planetSize + 10 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ].x != -1 &&
            planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1].x != -1 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1].x != -1)
        {

            // BOTTOM SIDE QUAD
            if (currentY < universeSize / 2) // in the bottom half of the planet
            {
    //            Debug.Log("********* BOTTOM QUAD:" + " " + currentX + "," + currentY + "," + currentZ);
                subdivideQuad(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
                               planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1],
                               planetSize,
                               Face.Bottom);
     //           Debug.Log("BOTTOM SIDE QUAD subdivided: " + currentX + "," + currentY + "," + currentZ); 
     //           Debug.Log("VERTICES 0 : " + planetVertices[currentX, currentY, currentZ]);
     //           Debug.Log("VERTICES 1 : " + planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ]);
     //           Debug.Log("VERTICES 2 : " + planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1]);
     //           Debug.Log("VERTICES 3 : " + planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1]);
            }
            else // TOP SIDE QUAD
            {
    //            Debug.Log("********* TOP QUAD:" + " " + currentX + "," + currentY + "," + currentZ);
                subdivideQuad(planetVertices[currentX, currentY, currentZ],
                                planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
                                planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
                                planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ + distanceBetweenVertices - 1],
                               planetSize,
                               Face.Top);
    //            Debug.Log("TOP SIDE QUAD subdivided: " + currentX + "," + currentY + "," + currentZ);
            }
        }

        // Vertical plane (PosX PosY) quad - **** this should locate the front and back quads of a cube ****
        // =================================================================================================
        // (X,Y,Z) (X+1, Y, Z) (X, Y+1, Z) (X+1, Y+1, Z)
        if (currentX + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentY + (distanceBetweenVertices - 1) < planetSize + 10 &&
            //currentZ + (distanceBetweenVertices - 1) < planetSize + 10 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ].x != -1 &&
            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ].x != -1 &&
            planetVertices[currentX + distanceBetweenVertices - 1, currentY + distanceBetweenVertices - 1, currentZ].x != -1)
        {
            // FRONT SIDE QUAD
            if (currentZ < universeSize / 2) // in the front half of the planet
            {
        //        Debug.Log("********* FRONT QUAD:" + " " + currentX + "," + currentY + "," + currentZ);
                subdivideQuad(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY + distanceBetweenVertices - 1, currentZ],
                               planetSize,
                               Face.Front);
        //        Debug.Log("FRONT SIDE QUAD subdivided: " + currentX + "," + currentY + "," + currentZ);
            }
            else // BACK SIDE QUAD
            {
    //            Debug.Log("********* BACK QUAD:" + " " + currentX + "," + currentY + "," + currentZ);
                subdivideQuad(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY, currentZ],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
                               planetVertices[currentX + distanceBetweenVertices - 1, currentY + distanceBetweenVertices - 1, currentZ],
                               planetSize,
                               Face.Back);
        //        Debug.Log("BACK SIDE QUAD subdivided: " + currentX + "," + currentY + "," + currentZ);
            }
        }

        // Side planes (PosY PosZ) quad - **** this should locate the left and right side quads of a cube ****
        // ===================================================================================================
        // (X,Y,Z) (X, Y+1, Z) (X, Y, Z+1) (X, Y+1, Z+1)
        if (//currentX + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentY + (distanceBetweenVertices - 1) < planetSize + 10 &&
            currentZ + (distanceBetweenVertices - 1) < planetSize + 10 &&
            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ].x != -1 &&
            planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1].x != -1 &&
            planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ + distanceBetweenVertices - 1].x != -1)
        {
            // LEFT SIDE QUAD
            if (currentX < universeSize / 2) // in the left half of the planet
            {
        //        Debug.Log("********* LEFT SIDE QUAD:" + " " + currentX + "," + currentY + "," + currentZ);
                subdivideQuad(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ + distanceBetweenVertices - 1],
                               planetSize,
                               Face.LeftSide);
        //        Debug.Log("LEFT SIDE QUAD subdivided: " + currentX + "," + currentY + "," + currentZ);
            }
            else // RIGHT SIDE QUAD
            {
        //        Debug.Log("********* RIGHT SIDE QUAD:" + " " + currentX + "," + currentY + "," + currentZ);
                subdivideQuad(planetVertices[currentX, currentY, currentZ],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ],
                               planetVertices[currentX, currentY, currentZ + distanceBetweenVertices - 1],
                               planetVertices[currentX, currentY + distanceBetweenVertices - 1, currentZ + distanceBetweenVertices - 1],
                               planetSize,
                               Face.RightSide);
         //       Debug.Log("RIGHT SIDE QUAD subdivided: " + currentX + "," + currentY + "," + currentZ);
            }
        }
    }

    public void Subdivide()
    {
        // this changes on each subdivsion of the surfaces
        int currentTotalLayers = 2; // 2 layers of vertices in a cube / starting number of layer - increase by ?????
        // this changes on each subdivsion of the surfaces
        distanceBetweenVertices = planetSize;
        // increase by        1 2 4 8  1
        // number of layers 2 3 5 9 15
        // currentTotalLayers = currentTotalLayers + currentTotalLayers - 1
        // locate quads
        while (currentTotalLayers <= totalLayers)
        { 
            // subdivide the current faces
            // increase currentLayerTotal to the new current total number of layers
            // current X, current Y, current Z
            for (int currentY = Y; currentY < planetSize + 10; currentY += (distanceBetweenVertices - 1)) // process only existing vertices
            {
                for (int currentZ = Z; currentZ < planetSize + 10; currentZ += (distanceBetweenVertices - 1))
                {
                    for (int currentX = X; currentX < planetSize + 10; currentX += (distanceBetweenVertices - 1))
                    { // locate quads along the X avis
                      // IS THERE A VERTEX HERE?
                        if (planetVertices[currentX, currentY, currentZ].x != -1)
                        {
    //                        Debug.Log("SUBIVIDE QUADS - Vertex found at: " + currentX + "," + currentY + "," + currentZ + "," + " " + planetVertices[currentX, currentY, currentZ]);
                            LocateAndSubdivideQuads(currentY, currentZ, currentX);
                            // can also have a locate and subdivide quads
                        }
                    }
                }
            }
            // increase currentLayerTotal by the number of new layers of vertices - ????? what is the equation for this ?????
            currentTotalLayers = currentTotalLayers + currentTotalLayers - 1; // increased by 1 after the first subdivision (subdivided cube)
            // decrease the distance between the vertices
            distanceBetweenVertices = Convert.ToInt32(distanceBetweenVertices / 2);
        }
    }


    // generate globe  - location in the game world (first planet at 0,0,0)
    public PlanetGen(Vector3 planetLocation, float radius)
    {
        // Initialise planet as cube (8 vertices)
    }


    /*
     * This places the vertex at a specified distance beyond its current location, along a calculated vector
     */
    private Vector3 PushVertexOut(Vector3 vectorEquationResult, int distance, Vector3 pushOutVertex)
    {
        // X
        if (vectorEquationResult.x > planetCentre.x)
            pushOutVertex.x = vectorEquationResult.x + distance;
        else if (vectorEquationResult.x < planetCentre.x)
            pushOutVertex.x = vectorEquationResult.x - distance;
        // Y
        if (vectorEquationResult.y > planetCentre.y)
            pushOutVertex.y = vectorEquationResult.y + distance;
        else if (vectorEquationResult.y < planetCentre.y)
            pushOutVertex.y = vectorEquationResult.y - distance;
        // Z
        if (vectorEquationResult.z > planetCentre.z)
            pushOutVertex.z = vectorEquationResult.z + distance;
        else if (vectorEquationResult.z < planetCentre.z)
            pushOutVertex.z = vectorEquationResult.z - distance;

 //       Debug.Log("QUAD CENTRE - Pushed out vertext: " + pushOutVertex);

        return pushOutVertex;
    }

    /*
     * This is the vector equation used to acquire the direction (in 3D space) the vertex is to be pushed out along
     * 
     * TODO: Put this in its own class?
     */
    private Vector3 CalculateVector(Vector3 newVector)
    {
        // Calculate the direction vector
        Vector3 directionVector;
        directionVector.x = newVector.x - planetCentre.x;
        directionVector.y = newVector.y - planetCentre.y;
        directionVector.z = newVector.z - planetCentre.z;

        //        Debug.Log("Quad - Direction vector: " + directionVector + " " + bottomLeft + " " + topLeft);
        // Position Vector = cubeCentre.x cubeCentre.z cubeCentre.z
        Vector3 vectorEquationResult;
        vectorEquationResult.x = planetCentre.x + directionVector.x;
        vectorEquationResult.y = planetCentre.y + directionVector.y;
        vectorEquationResult.z = planetCentre.z + directionVector.z;

        //        Debug.Log("Quad - Inter result: " + interResult + " " + bottomLeft + " " + topLeft);
        return vectorEquationResult;
    }

    /*
     * Used when figuring out the centre of the quad and the midpoint of the edges around the quad 
     * This returns the midpoint of one edge
     * - the vertical and horizontal edges are used to locate the quad's centre
     */
    private Vector3 GetEdgeMidpoint(Vector3 vertex1, Vector3 vertex2, out Vector3 vertex)
    {
        // X coord
        if (vertex2.x > vertex1.x)
            // (int)((topLeft.x - v0.x) / 2)
            vertex.x = (int)((vertex2.x - vertex1.x) / 2 + vertex1.x);
        else if (vertex2.x < vertex1.x)
            vertex.x = (int)((vertex1.x - vertex2.x) / 2 + vertex2.x);
        else vertex.x = vertex1.x;
        // Y coord
        if (vertex2.y > vertex1.y)
            vertex.y = (int)((vertex2.y - vertex1.y) / 2 + vertex1.y);
        else if (vertex2.y < vertex1.y)
            vertex.y = (int)((vertex1.y - vertex2.y) / 2 + vertex2.y);
        else vertex.y = vertex1.y;
        // Z coord
        if (vertex2.z > vertex1.z)
            vertex.z = (int)((vertex2.z - vertex1.z) / 2 + vertex1.z);
        else if (vertex2.z < vertex1.z)
            vertex.z = (int)((vertex1.z - vertex2.z) / 2 + vertex2.z);
        else vertex.z = vertex1.z;
        return vertex;
    }


    /*
     * This figures out where the centre of a quad is, then creates a vertex there and
     * then uses a vector equation to push out the vertex 
     * (all vertices must be the sphere's radius distance from the cube's centre)
     * 
     * bottomLeft, topLeft, bottomRight are the vertices of the current quad
     * side is the side of the cube currently being worked on (top, botton, front, back, etc.)
     * newVector holds the coords of the newly created vector (centre of the face/quad)
     */
    public void GetTheCentreOfTheQuad(Vector3 bottomLeft, Vector3 topLeft, Vector3 bottomRight, Face side, out Vector3 quadCentre)
    {
        // get the mid point of 2 of the edges, so that we can then figure out the centre of the quad
        // VERICAL edge
        Vector3 verticalMidpoint;
        GetEdgeMidpoint(bottomLeft, topLeft, out verticalMidpoint);

  //      Debug.Log("QUAD CENTRE - Vertical vertex: " + verticalMidpoint + " *** " + bottomLeft + " *** " + topLeft);

        // HORIZONTAL edge
        Vector3 horizontalMidpoint;
        GetEdgeMidpoint(bottomLeft, bottomRight, out horizontalMidpoint);

  //      Debug.Log("QUAD CENTRE - Horizontal vertex: " + horizontalMidpoint + " *** " + bottomLeft + " *** " + bottomRight);

        // Left and Right faces
        if (side == Face.LeftSide || side == Face.RightSide)
        {
            quadCentre.x = (verticalMidpoint.x < horizontalMidpoint.x) ? quadCentre.x = verticalMidpoint.x : quadCentre.x = horizontalMidpoint.x;
        }
        else // Bottom, Top, Front, and Back faces
        {
            quadCentre.x = (verticalMidpoint.x > horizontalMidpoint.x) ? quadCentre.x = verticalMidpoint.x : quadCentre.x = horizontalMidpoint.x;
        }
        // Front and Top and Bottom and Back and Left and Right faces (ALL FACES)
        quadCentre.y = (verticalMidpoint.y > horizontalMidpoint.y) ? quadCentre.y = verticalMidpoint.y : quadCentre.y = horizontalMidpoint.y;

        // Left and Back faces
        if (side == Face.Back)
        {
            quadCentre.z = (verticalMidpoint.z < horizontalMidpoint.z) ? quadCentre.z = verticalMidpoint.z : quadCentre.z = horizontalMidpoint.z;
        }
        else // Front, Top, Bottom and Right faces
        {
            quadCentre.z = (verticalMidpoint.z > horizontalMidpoint.z) ? quadCentre.z = verticalMidpoint.z : quadCentre.z = horizontalMidpoint.z;
        }

 //       Debug.Log("QUAD CENTRE pre push out: " + quadCentre);

        // VECTOR or PARAMETRIC EQUATION
        // https://www.youtube.com/watch?v=PyPp4QvQY3Q
        // https://www.youtube.com/watch?v=JlRagTNGBF0
        // start point = 250, 250, 250
        // 2nd point current X 250, Y 250, Z 0 of the vertex
        // 
        // Direction vector = 250 - 250, 250 - 250, 0 - 250
        //                        0,         0,       -250
        //
        // new X = 250 + (currentX * t)
        // new Y = 250 + (currentY * t)
        // new Y = 250 + (currentZ * t)

        // (250, 250, 250) + (0 t, 0 t, -250 t)
        // x = 250
        // y = 250
        // z = 250 + -250t = 0
        // need to add 5 - push out vertex
        Vector3 vectorEquationResult;
        vectorEquationResult = CalculateVector(quadCentre);
        int distance = 5; // TODO: this needs to be based on the required redius of a sphere (planet)
        quadCentre = PushVertexOut(vectorEquationResult, distance, quadCentre);
    }

    /*
     * Gets the centre of an edge
     * 
     * v0, v1 are the vertices of the edge
     * newVector holds the coordinates of the newly created vector (centre of the edge)
     */
    public void GetTheCentreOfTheEdge(Vector3 vertex1, Vector3 vertex2, out Vector3 edgeCentre)
    {
        GetEdgeMidpoint(vertex1, vertex2, out edgeCentre);

        //      Debug.Log("Quad - Subdived edge vertex: " + newVector + " *** " + v0 + " *** " + v1);

        // PUSH THE VERTEX OUT

        // VECTOR or PARAMETRIC EQUATION
        // https://www.youtube.com/watch?v=PyPp4QvQY3Q
        // https://www.youtube.com/watch?v=JlRagTNGBF0
        // start point = 250, 250, 250
        // 2nd point current X 250, Y 250, Z 0 of the vertex
        // 
        // Direction vector = 250 - 250, 250 - 250, 0 - 250
        //                        0,         0,       -250
        //
        // new X = 250 + (currentX * t)
        // new Y = 250 + (currentY * t)
        // new Y = 250 + (currentZ * t)

        // (250, 250, 250) + (0 t, 0 t, -250 t)
        // x = 250
        // y = 250
        // z = 250 + -250t = 0
        // need to add 5 somehow!
        Vector3 vectorEquationResult;
        vectorEquationResult = CalculateVector(edgeCentre);
        int distance = 5; // TODO: this needs to be based on the required redius of a sphere (planet)
        edgeCentre = PushVertexOut(vectorEquationResult, distance, edgeCentre);
    }

    /* 
     * v0 bottom left
     * v1 top left
     * v2 bottom right
     * v3 top right
     */
    public void subdivideQuad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, int length, Face side)
    {
    //    Debug.Log("*** SIDE : " + side);

        Vector3 quadCentre = new Vector3(0f,0f,0f); // initialise to 0
        // find the centre of the quad
        // new vertex v0.x + length/2, v0.y + length/2, v0.z + length/2

        GetTheCentreOfTheQuad(v0, v1, v2, side, out quadCentre);

        // output new vertex
 //       Debug.Log("QUAD CENTRE - Pushed out vertext: " + quadCentre + " " + v0 + " " + v1);

        // store the new vertex
        planetVertices[(int)quadCentre.x, (int)quadCentre.y, (int)quadCentre.z] = new Vector3(quadCentre.x, quadCentre.y, quadCentre.z);
  //      Debug.Log("Quad - Newly stored vertex: " + quadCentre + " " + v0 + " " + v1);

  //      Debug.Log("QUAD CENTRE: " + quadCentre);
  //      Debug.Log("QUAD CENTRE: " + planetVertices[(int)quadCentre.x, (int)quadCentre.y, (int)quadCentre.z]);

        // PUSH NEW VERTEX OUT (radius)
        // may need to move quad to new location in the array

        // SPLIT QUAD's EDGES
        Vector3 edgeCentre;
        //   bottom edge
 //       Debug.Log("*** BOTTOM EDGE ***");
        GetTheCentreOfTheEdge(v0, v2, out edgeCentre);
 //       Debug.Log("Pushed out vertex: " + edgeCentre + " " + v0 + " " + v2);
        //   left side edge
 //       Debug.Log("*** LEFT SIDE EDGE ***");
        GetTheCentreOfTheEdge(v0, v1, out edgeCentre);
 //       Debug.Log("Pushed out vertex: " + edgeCentre + " " + v0 + " " + v1);
        //   left side edge
  //      Debug.Log("*** RIGHT SIDE  EDGE ***");
        GetTheCentreOfTheEdge(v2, v3, out edgeCentre);
 //       Debug.Log("Pushed out vertex: " + edgeCentre + " " + v2 + " " + v3);
        //   top edge
 //       Debug.Log("*** TOP EDGE ***");
        GetTheCentreOfTheEdge(v1, v3, out edgeCentre);
 //       Debug.Log("Pushed out vertex: " + edgeCentre + " " + v1 + " " + v3);
    }



    public void DisplayInitialCube(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {        
      //  Quad newQuad = new Quad(planetPosition, 
      //                          v0, v1, v2, v3,
      //                          CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
      //                          CustomMaterials.rockQuad);
      //  newQuad.Draw();
    }

    // apply terrain generation
    // Generate vertices
    public void GenerateVertices()
    {

    }



    // END OF QUAD CREATION IN THE GAME Universe
    // ***************************************

    /*
     * Display the quads - a row at a time, to create the terrain in a chunk
     * Unable to use the C# thread for this, therefore used Unity's Coroutine
     * May revisit this, as I would like to use C# thread throughout
     */
    void GenerateRowOfQuads(int y, int z, int sizeX)
    {
 /*       int terrainType = 0;
        for (int x = 88; x <= sizeX; x++)
        {
            //     quadMaterial = Texturing.SetMaterial(planetVertices[x - 1, y, z], maxTerrainHeight, out terrainType); // not ideal!!!
            quadMaterial = CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad);
            Vector3 locationInChunk = new Vector3(x, z);
            // vertex0 - chunkVertices[x-1, z];
            // vertex1 - chunkVertices[x, z]
            // vertex2 - chunkVertices[x-1, z-1]
            // vertex3 - chunkVertices[x, z-1]
            if (planetVertices[x - 1, y, z] == null || planetVertices[x, y, z] == null ||
                planetVertices[x - 1, y, z - 1] == null || planetVertices[x, y, z - 1] == null)
            {
                Debug.Log("NULL location found");
                continue;
            }
            planetData[x - 1, y - 1, z - 1] = new Quad(locationInChunk,
                                           planetVertices[x - 1, y, z],
                                           planetVertices[x, y, z],
                                           planetVertices[x - 1, y, z - 1],
                                           planetVertices[x, y, z - 1],
                                           planet.gameObject,
                                           this,
                                           quadMaterial,
                                           terrainType);
            planetData[x - 1, y - 1, z - 1].Draw();
        }           */
    }

    /*
     * Draw the quads in the chunk
     * 
     * Unity is not thread safe yet - 
     * look at https://docs.unity3d.com/Manual/JobSystem.html?_ga=2.149032254.968692378.1582283703-307768343.1578037165
     */
    public void DrawPlanet()
    {
        for (int y = 100; y <= 104; y++) // TODO: magic number!!!
        {
                Debug.Log("**************** Creating level : " + y);
            for (int z = 1; z <= 200; z++)
            {
                Debug.Log("**************** Creating Z row: " + z);
                // place the generation of a row of quads in its own thread
  //              GenerateRowOfQuads(y, z, 200);
            }
        }
    }

    // *****************************************
    // START OF QUAD CREATION IN THE GAME Universe


    /*
     * this draws the planet, using the vertices generated on the creation of the planet object
     */
    public void BuildPlanet()
    {
        Debug.Log("**************** DRAWING PLANET **************");
        // make use of the planetVertices[...] to draw the quads
        // 599 x 599 x 599 array of vertices
        // planet starts at 100x100x100

  //      planetData = new Quad[599, 599, 599];
        // need to figure out how to build the quads
  //      DrawPlanet();
    }

}
