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
    public int planetZize = 499; // 100 x 100 x 100 world

    public Vector3[,,] planetVertices;

    const int X = 100;
    const int Y = 100;
    const int Z = 100;

    public enum Face { Top, Bottom, Front, Back, LeftSide, RightSide }
    
    // generate globe 
    public PlanetGen()
    {
        planetVertices = new Vector3[planetZize + 201, planetZize + 201, planetZize + 201];
        // Initialise planet as cube (8 vertices)
        // set the 4 initial vertices of the starting cube 
        //    these should be at the radius we want the sphere to be
        // Front
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y + planetZize, Z] = new Vector3(X, Y + planetZize, Z);
        planetVertices[X + planetZize, Y, Z] = new Vector3(X + planetZize, Y, Z);
        planetVertices[X + planetZize, Y + planetZize, Z] = new Vector3(X + planetZize, Y + planetZize, Z);
        DisplayInitialCube(planetVertices[X, Y, Z],
                        planetVertices[X, Y + planetZize, Z],
                        planetVertices[X + planetZize, Y, Z],
                        planetVertices[X + planetZize, Y + planetZize, Z]);
        // Top
        planetVertices[X, Y + planetZize, Z] = new Vector3(X, Y + planetZize, Z);
        planetVertices[X, Y + planetZize, Z + planetZize] = new Vector3(X, Y + planetZize, Z + planetZize);
        planetVertices[X + planetZize, Y + planetZize, Z] = new Vector3(X + planetZize, Y + planetZize, Z);
        planetVertices[X + planetZize, Y + planetZize, Z + planetZize] = new Vector3(X + planetZize, Y + planetZize, Z + planetZize);
        DisplayInitialCube(planetVertices[X, Y + planetZize, Z],
                        planetVertices[X, Y + planetZize, Z + planetZize],
                        planetVertices[X + planetZize, Y + planetZize, Z],
                        planetVertices[X + planetZize, Y + planetZize, Z + planetZize]);
        // Bottom
        planetVertices[X + planetZize, Y, Z] = new Vector3(X + planetZize, Y, Z);
        planetVertices[X + planetZize, Y, Z + planetZize] = new Vector3(X + planetZize, Y, Z + planetZize);
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y, Z + planetZize] = new Vector3(X, Y, Z + planetZize);
        DisplayInitialCube(planetVertices[X + planetZize, Y, Z],
                        planetVertices[X + planetZize, Y, Z + planetZize],
                        planetVertices[X, Y, Z],
                        planetVertices[X, Y, Z + planetZize]);
        // Back
        planetVertices[X + planetZize, Y, Z + planetZize] = new Vector3(X + planetZize, Y, Z + planetZize);
        planetVertices[X + planetZize, Y + planetZize, Z + planetZize] = new Vector3(X + planetZize, Y + planetZize, Z + planetZize);
        planetVertices[X, Y, Z + planetZize] = new Vector3(X, Y, Z + planetZize);
        planetVertices[X, Y + planetZize, Z + planetZize] = new Vector3(X, Y + planetZize, Z + planetZize);
        DisplayInitialCube(planetVertices[X + planetZize, Y, Z + planetZize],
                        planetVertices[X + planetZize, Y + planetZize, Z + planetZize],
                        planetVertices[X, Y, Z + planetZize],
                        planetVertices[X, Y + planetZize, Z + planetZize]);
        // Left
        planetVertices[X, Y, Z + planetZize] = new Vector3(X, Y, Z + planetZize);
        planetVertices[X, Y + planetZize, Z + planetZize] = new Vector3(X, Y + planetZize, Z + planetZize);
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y + planetZize, Z] = new Vector3(X, Y + planetZize, Z);
        DisplayInitialCube(planetVertices[X, Y, Z + planetZize],
                        planetVertices[X, Y + planetZize, Z + planetZize],
                        planetVertices[X, Y, Z],
                        planetVertices[X, Y + planetZize, Z]);
        // Right
        planetVertices[X + planetZize, Y, Z] = new Vector3(X + planetZize, Y, Z);
        planetVertices[X + planetZize, Y + planetZize, Z] = new Vector3(X + planetZize, Y + planetZize, Z);
        planetVertices[X + planetZize, Y, Z + planetZize] = new Vector3(X + planetZize, Y, Z + planetZize);
        planetVertices[X + planetZize, Y + planetZize, Z + planetZize] = new Vector3(X + planetZize, Y + planetZize, Z + planetZize);
        DisplayInitialCube(planetVertices[X + planetZize, Y, Z],
                        planetVertices[X + planetZize, Y + planetZize, Z],
                        planetVertices[X + planetZize, Y, Z + planetZize],
                        planetVertices[X + planetZize, Y + planetZize, Z + planetZize]);

        // Now subdivide quads

        Debug.Log("Front **********************");
        // Front
        subdivideQuad(planetVertices[X, Y, Z],
                        planetVertices[X, Y + planetZize, Z],
                        planetVertices[X + planetZize, Y, Z],
                        planetVertices[X + planetZize, Y + planetZize, Z],
                        planetZize,
                        Face.Front);

        Debug.Log("Top **********************");
        // Top
        subdivideQuad(planetVertices[X, Y + planetZize, Z],
                        planetVertices[X, Y + planetZize, Z + planetZize],
                        planetVertices[X + planetZize, Y + planetZize, Z],
                        planetVertices[X + planetZize, Y + planetZize, Z + planetZize],
                        planetZize,
                        Face.Top);

        Debug.Log("Bottom **********************");
        // Bottom
        subdivideQuad(planetVertices[X + planetZize, Y, Z],
                        planetVertices[X + planetZize, Y, Z + planetZize],
                        planetVertices[X, Y, Z],
                        planetVertices[X, Y, Z + planetZize],
                        planetZize,
                        Face.Bottom);

        Debug.Log("Back **********************");
        // Back
        subdivideQuad(planetVertices[X + planetZize, Y, Z + planetZize],
                        planetVertices[X + planetZize, Y + planetZize, Z + planetZize],
                        planetVertices[X, Y, Z + planetZize],
                        planetVertices[X, Y + planetZize, Z + planetZize],
                        planetZize,
                        Face.Back);

        Debug.Log("Left **********************");
        // Left
        subdivideQuad(planetVertices[X, Y, Z + planetZize],
                        planetVertices[X, Y + planetZize, Z + planetZize],
                        planetVertices[X, Y, Z],
                        planetVertices[X, Y + planetZize, Z],
                        planetZize,
                        Face.LeftSide);

        Debug.Log("Right **********************");
        // Right
        subdivideQuad(planetVertices[X + planetZize, Y, Z],
                        planetVertices[X + planetZize, Y + planetZize, Z],
                        planetVertices[X + planetZize, Y, Z + planetZize],
                        planetVertices[X + planetZize, Y + planetZize, Z + planetZize],
                        planetZize,
                        Face.RightSide);

    }

    // generate globe  - location in the game world (first planet at 0,0,0)
    public PlanetGen(Vector3 univerLocation, float radius)
    {
        // Initialise planet as cube (8 vertices)
    }


    /*
     * bottomLeft, topLeft, bottomRight are the vertices of the current quad
     * side is the side of the cube currently being worked on (top, botton, front, back, etc.)
     * newVector holds the coords of the newly created vector (centre of the face/quad)
     */
    public void GetTheCentreOfTheQuad(Vector3 bottomLeft, Vector3 topLeft, Vector3 bottomRight, Face side, out Vector3 newVector)
    {
        // VERICAL
        Vector3 vertical;
        // X coord
        if (topLeft.x > bottomLeft.x)
            // (int)((topLeft.x - v0.x) / 2)
            vertical.x = (int)((topLeft.x - bottomLeft.x) / 2 + bottomLeft.x);
        else if (topLeft.x < bottomLeft.x)
            vertical.x = (int)((bottomLeft.x - topLeft.x) / 2 + topLeft.x);
        else vertical.x = bottomLeft.x;
        // Y coord
        if (topLeft.y > bottomLeft.y)
            vertical.y = (int)((topLeft.y - bottomLeft.y) / 2 + bottomLeft.y);
        else if (topLeft.y < bottomLeft.y)
            vertical.y = (int)((bottomLeft.y - topLeft.y) / 2 + topLeft.y);
        else vertical.y = bottomLeft.y;
        // Z coord
        if (topLeft.z > bottomLeft.z)
            vertical.z = (int)((topLeft.z - bottomLeft.z) / 2 + bottomLeft.z);
        else if (topLeft.z < bottomLeft.z)
            vertical.z = (int)((bottomLeft.z - topLeft.z) / 2 + topLeft.z);
        else vertical.z = bottomLeft.z;

 //       Debug.Log("Quad - Vertical vertex: " + vertical + " *** " + bottomLeft + " *** " + topLeft);

        // HORIZONTAL
        Vector3 horiz;
        // X coord
        if (bottomRight.x > bottomLeft.x)
            horiz.x = (int)((bottomRight.x - bottomLeft.x) / 2 + bottomLeft.x);
        else if (bottomRight.x < bottomLeft.x)
            horiz.x = (int)((bottomLeft.x - bottomRight.x) / 2 + bottomRight.x);
        else horiz.x = bottomLeft.x;
        // Y coord
        if (bottomRight.y > bottomLeft.y)
            horiz.y = (int)((bottomRight.y - bottomLeft.y) / 2 + bottomLeft.y);
        else if (bottomRight.y < bottomLeft.y)
            horiz.y = (int)((bottomLeft.y - bottomRight.y) / 2 + bottomRight.y);
        else horiz.y = bottomLeft.y;
        // Z coord
        if (bottomRight.z > bottomLeft.z)
            horiz.z = (int)((bottomRight.z - bottomLeft.z) / 2 + bottomLeft.z);
        else if (bottomRight.z < bottomLeft.z)
            horiz.z = (int)((bottomLeft.z - bottomRight.z) / 2 + bottomRight.z);
        else horiz.z = bottomLeft.z;

 //       Debug.Log("Quad - Horizontal vertex: " + horiz + " *** " + bottomLeft + " *** " + bottomRight);

        // Bottom face
        if (side == Face.Bottom || side == Face.Back)
        {
            newVector.x = (vertical.x < horiz.x) ? newVector.x = vertical.x : newVector.x = horiz.x;
        }
        else // Front and Top and Left and Right faces
        {            
            newVector.x = (vertical.x > horiz.x) ? newVector.x = vertical.x : newVector.x = horiz.x;
        }
        // Front and Top and Bottom and Back and Left and Right faces 
        newVector.y = (vertical.y > horiz.y) ? newVector.y = vertical.y : newVector.y = horiz.y;

        // Left and Back faces
        if (side == Face.LeftSide || side == Face.Back)
        {
            newVector.z = (vertical.z < horiz.z) ? newVector.z = vertical.z : newVector.z = horiz.z;
        }
        else // Front and Top and Bottom and Right faces
        {            
            newVector.z = (vertical.z > horiz.z) ? newVector.z = vertical.z : newVector.z = horiz.z;
        }

//        Debug.Log("Quad - Newly created vertex: " + newVector);
        
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

        Vector3 cubeCentre = new Vector3(349f, 349f, 349f);
        // Calculate the direction vector
        Vector3 directionVector;
        directionVector.x = newVector.x - cubeCentre.x;
        directionVector.y = newVector.y - cubeCentre.y;
        directionVector.z = newVector.z - cubeCentre.z;

//        Debug.Log("Quad - Direction vector: " + directionVector + " " + bottomLeft + " " + topLeft);
        Vector3 interResult;
        interResult.x = cubeCentre.x + directionVector.x;
        interResult.y = cubeCentre.y + directionVector.y;
        interResult.z = cubeCentre.z + directionVector.z;

//        Debug.Log("Quad - Inter result: " + interResult + " " + bottomLeft + " " + topLeft);
        // X
        if (interResult.x > cubeCentre.x)
            newVector.x = interResult.x + 5;
        else if (interResult.x < cubeCentre.x)
            newVector.x = interResult.x - 5;
        // Y
        if (interResult.y > cubeCentre.y)
            newVector.y = interResult.y + 5;
        else if (interResult.y < cubeCentre.y)
            newVector.y = interResult.y - 5;
        // Z
        if (interResult.z > cubeCentre.z)
            newVector.z = interResult.z + 5;
        else if (interResult.z < cubeCentre.z)
            newVector.z = interResult.z - 5;
    }

    /*
     * Gets the centre of an edge
     * 
     * v0, v1 are the vertices of the edge
     * newVector holds the coordinates of the newly created vector (centre of the edge)
     */
    public void GetTheCentreOfTheEdge(Vector3 v0, Vector3 v1, out Vector3 newVector)
    {
        // Works for the edges of the FRONT FACE, TOP FACE, BOTTOM FACE, BACK FACE, LEFT FACE, RIGHT FACE
        // X coord
        if (v1.x > v0.x)
            // (int)((topLeft.x - v0.x) / 2)
            newVector.x = (int)((v1.x - v0.x) / 2 + v0.x);
        else if (v1.x < v0.x)
            newVector.x = (int)((v0.x - v1.x) / 2 + v1.x);
        else newVector.x = v0.x;
        // Y coord
        if (v1.y > v0.y)
            newVector.y = (int)((v1.y - v0.y) / 2 + v0.y);
        else if (v1.y < v0.y)
            newVector.y = (int)((v0.y - v1.y) / 2 + v1.y);
        else newVector.y = v0.y;
        // Z coord
        if (v1.z > v0.z)
            newVector.z = (int)((v1.z - v0.z) / 2 + v0.z);
        else if (v1.z < v0.z)
            newVector.z = (int)((v0.z - v1.z) / 2 + v1.z);        
        else newVector.z = v0.z;

        Debug.Log("Quad - Subdived edge vertex: " + newVector + " *** " + v0 + " *** " + v1);

        // PUSH THE VERTEX OUT

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

        // WORKS for the edges of the FRONT face, TOP face, BOTTOM face, BACK face, LEFT SIDE face, RIGHT SIDE face,
        //            

        Vector3 cubeCentre = new Vector3(349f, 349f, 349f);
        // Calculate the direction vector
        Vector3 directionVector;
        directionVector.x = newVector.x - cubeCentre.x;
        directionVector.y = newVector.y - cubeCentre.y;
        directionVector.z = newVector.z - cubeCentre.z;

        //        Debug.Log("Quad - Direction vector: " + directionVector + " " + bottomLeft + " " + topLeft);
        Vector3 interResult;
        interResult.x = cubeCentre.x + directionVector.x;
        interResult.y = cubeCentre.y + directionVector.y;
        interResult.z = cubeCentre.z + directionVector.z;

        //        Debug.Log("Quad - Inter result: " + interResult + " " + bottomLeft + " " + topLeft);
        // X
        if (interResult.x > cubeCentre.x)
            newVector.x = interResult.x + 5;
        else if (interResult.x < cubeCentre.x)
            newVector.x = interResult.x - 5;
        // Y
        if (interResult.y > cubeCentre.y)
            newVector.y = interResult.y + 5;
        else if (interResult.y < cubeCentre.y)
            newVector.y = interResult.y - 5;
        // Z
        if (interResult.z > cubeCentre.z)
            newVector.z = interResult.z + 5;
        else if (interResult.z < cubeCentre.z)
            newVector.z = interResult.z - 5;
    }

    /* 
     * v0 bottom left
     * v1 top left
     * v2 bottom right
     * v3 top right
     */
    public void subdivideQuad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, int length, Face side)
    {
        Debug.Log("*** SIDE : " + side);

        Vector3 newVector = new Vector3(0f,0f,0f);
        // find the centre of the quad
        // new vertex v0.x + length/2, v0.y + length/2, v0.z + length/2

        GetTheCentreOfTheQuad(v0, v1, v2, side, out newVector);

        // output new vertex
   //     Debug.Log("Quad - Push out vertext: " + newVector + " " + v0 + " " + v1);

        // store the new vertex
        planetVertices[(int)newVector.x, (int)newVector.y, (int)newVector.z] = new Vector3(newVector.x, newVector.y, newVector.z);
        //     Debug.Log("Quad - Newly stored vertex: " + newVector + " " + v0 + " " + v1);

        // PUSH NEW VERTEX OUT (radius)
        // may need to move quad to new location in the array

        // SPLIT QUAD's EDGES
        //   bottom edge
        Debug.Log("*** BOTTOM EDGE ***");
        GetTheCentreOfTheEdge(v0, v2, out newVector);
        Debug.Log("Pushed out vertex: " + newVector + " " + v0 + " " + v2);
        //   left side edge
        Debug.Log("*** LEFT SIDE  EDGE ***");
        GetTheCentreOfTheEdge(v0, v1, out newVector);
        Debug.Log("Pushed out vertex: " + newVector + " " + v0 + " " + v1);
        //   left side edge
        Debug.Log("*** RIGHT SIDE  EDGE ***");
        GetTheCentreOfTheEdge(v2, v3, out newVector);
        Debug.Log("Pushed out vertex: " + newVector + " " + v2 + " " + v3);
        //   top edge
        Debug.Log("*** TOP EDGE ***");
        GetTheCentreOfTheEdge(v1, v3, out newVector);
        Debug.Log("Pushed out vertex: " + newVector + " " + v1 + " " + v3);

    }

    public void DisplayInitialCube(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {        
        Quad newQuad = new Quad(v0, v1, v2, v3,
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad);
        newQuad.Draw();
    }

    // apply terrain generation
    // Generate vertices
    public void GenerateVertices()
    {

    }

}
