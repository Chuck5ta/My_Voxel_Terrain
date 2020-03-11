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

    // The faces of the cube
    // - starting point for the generation of a sphere/planet
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
    public PlanetGen(Vector3 planetLocation, float radius)
    {
        // Initialise planet as cube (8 vertices)
    }


    /*
     * This places the vertex at a specified distance beyond its current location, along a calculated vector
     */
    private static Vector3 PushVertexOut(Vector3 cubeCentre, Vector3 vectorEquationResult, int distance, Vector3 pushOutVertex)
    {
        // X
        if (vectorEquationResult.x > cubeCentre.x)
            pushOutVertex.x = vectorEquationResult.x + distance;
        else if (vectorEquationResult.x < cubeCentre.x)
            pushOutVertex.x = vectorEquationResult.x - distance;
        // Y
        if (vectorEquationResult.y > cubeCentre.y)
            pushOutVertex.y = vectorEquationResult.y + distance;
        else if (vectorEquationResult.y < cubeCentre.y)
            pushOutVertex.y = vectorEquationResult.y - distance;
        // Z
        if (vectorEquationResult.z > cubeCentre.z)
            pushOutVertex.z = vectorEquationResult.z + distance;
        else if (vectorEquationResult.z < cubeCentre.z)
            pushOutVertex.z = vectorEquationResult.z - distance;

        return pushOutVertex;
    }

    /*
     * This is the vector equation used to acquire the direction (in 3D space) the vertex is to be pushed out along
     * 
     * TODO: Put this in its own class?
     */
    private static Vector3 CalculateVector(Vector3 newVector, Vector3 cubeCentre)
    {
        // Calculate the direction vector
        Vector3 directionVector;
        directionVector.x = newVector.x - cubeCentre.x;
        directionVector.y = newVector.y - cubeCentre.y;
        directionVector.z = newVector.z - cubeCentre.z;

        //        Debug.Log("Quad - Direction vector: " + directionVector + " " + bottomLeft + " " + topLeft);
        // Position Vector = cubeCentre.x cubeCentre.z cubeCentre.z
        Vector3 vectorEquationResult;
        vectorEquationResult.x = cubeCentre.x + directionVector.x;
        vectorEquationResult.y = cubeCentre.y + directionVector.y;
        vectorEquationResult.z = cubeCentre.z + directionVector.z;

        //        Debug.Log("Quad - Inter result: " + interResult + " " + bottomLeft + " " + topLeft);
        return vectorEquationResult;
    }

    /*
     * Used when figuring out the centre of the quad and the midpoint of the edges around the quad 
     * This returns the midpoint of one edge
     * - the vertical and horizontal edges are used to locate the quad's centre
     */
    private static Vector3 GetEdgeMidpoint(Vector3 vertex1, Vector3 vertex2, out Vector3 vertex)
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

        //       Debug.Log("Quad - Vertical vertex: " + vertical + " *** " + bottomLeft + " *** " + topLeft);

        // HORIZONTAL edge
        Vector3 horizontalMidpoint;
        GetEdgeMidpoint(bottomLeft, bottomRight, out horizontalMidpoint);

        //       Debug.Log("Quad - Horizontal vertex: " + horiz + " *** " + bottomLeft + " *** " + bottomRight);

        // Bottom face
        if (side == Face.Bottom || side == Face.Back)
        {
            quadCentre.x = (verticalMidpoint.x < horizontalMidpoint.x) ? quadCentre.x = verticalMidpoint.x : quadCentre.x = horizontalMidpoint.x;
        }
        else // Front, Top, Left and Right faces
        {
            quadCentre.x = (verticalMidpoint.x > horizontalMidpoint.x) ? quadCentre.x = verticalMidpoint.x : quadCentre.x = horizontalMidpoint.x;
        }
        // Front and Top and Bottom and Back and Left and Right faces (ALL FACES)
        quadCentre.y = (verticalMidpoint.y > horizontalMidpoint.y) ? quadCentre.y = verticalMidpoint.y : quadCentre.y = horizontalMidpoint.y;

        // Left and Back faces
        if (side == Face.LeftSide || side == Face.Back)
        {
            quadCentre.z = (verticalMidpoint.z < horizontalMidpoint.z) ? quadCentre.z = verticalMidpoint.z : quadCentre.z = horizontalMidpoint.z;
        }
        else // Front, Top, Bottom and Right faces
        {
            quadCentre.z = (verticalMidpoint.z > horizontalMidpoint.z) ? quadCentre.z = verticalMidpoint.z : quadCentre.z = horizontalMidpoint.z;
        }

        //        Debug.Log("Quad - Newly created vertex: " + newVector);

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
        Vector3 cubeCentre = new Vector3(349f, 349f, 349f); // TODO: MAGIC NUMBERS!!!!!
        Vector3 vectorEquationResult;
        vectorEquationResult = CalculateVector(quadCentre, cubeCentre);
        int distance = 5; // TODO: this needs to be based on the required redius of a sphere (planet)
        quadCentre = PushVertexOut(cubeCentre, vectorEquationResult, distance, quadCentre);
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
        Vector3 cubeCentre = new Vector3(349f, 349f, 349f); // TODO: MAGIC NUMBERS!!!!!
        Vector3 vectorEquationResult;
        vectorEquationResult = CalculateVector(edgeCentre, cubeCentre);
        int distance = 5; // TODO: this needs to be based on the required redius of a sphere (planet)
        edgeCentre = PushVertexOut(cubeCentre, vectorEquationResult, distance, edgeCentre);
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

        Vector3 quadCentre = new Vector3(0f,0f,0f);
        // find the centre of the quad
        // new vertex v0.x + length/2, v0.y + length/2, v0.z + length/2

        GetTheCentreOfTheQuad(v0, v1, v2, side, out quadCentre);

        // output new vertex
   //     Debug.Log("Quad - Push out vertext: " + newVector + " " + v0 + " " + v1);

        // store the new vertex
        planetVertices[(int)quadCentre.x, (int)quadCentre.y, (int)quadCentre.z] = new Vector3(quadCentre.x, quadCentre.y, quadCentre.z);
        //     Debug.Log("Quad - Newly stored vertex: " + newVector + " " + v0 + " " + v1);

        // PUSH NEW VERTEX OUT (radius)
        // may need to move quad to new location in the array

        // SPLIT QUAD's EDGES
        Vector3 edgeCentre;
        //   bottom edge
        Debug.Log("*** BOTTOM EDGE ***");
        GetTheCentreOfTheEdge(v0, v2, out edgeCentre);
        Debug.Log("Pushed out vertex: " + edgeCentre + " " + v0 + " " + v2);
        //   left side edge
        Debug.Log("*** LEFT SIDE EDGE ***");
        GetTheCentreOfTheEdge(v0, v1, out edgeCentre);
        Debug.Log("Pushed out vertex: " + edgeCentre + " " + v0 + " " + v1);
        //   left side edge
        Debug.Log("*** RIGHT SIDE  EDGE ***");
        GetTheCentreOfTheEdge(v2, v3, out edgeCentre);
        Debug.Log("Pushed out vertex: " + edgeCentre + " " + v2 + " " + v3);
        //   top edge
        Debug.Log("*** TOP EDGE ***");
        GetTheCentreOfTheEdge(v1, v3, out edgeCentre);
        Debug.Log("Pushed out vertex: " + edgeCentre + " " + v1 + " " + v3);
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

    /*
     * this draws the planet, using the vertices generated on the creation of the planet object
     */
    public void DrawPlanet()
    {
        // make use of the planetVertices[...] to draw the quads
        // 599 x 599 x 599 array of vertices
        // planet starts at 100x100x100

        // need to figure out how to build the quads

    }

}
