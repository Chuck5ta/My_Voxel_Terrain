using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    // must always be an odd number, or otherwise subdivision fails
    public int planetSize = 2; // number of chunks
    public Vector3 planetCentre = new Vector3(10f, 10f, 10f);
    public int planetRadius = 7; // diameter of 14
    public int chunkSize = 10; // diameter -  size of chunk 4x4x4 cubes
    public Vector3 planetPosition = new Vector3(0,0,0); // coordinates of the planet in the universe

    public static Dictionary<string, PlanetChunk> planetChunks;


    // Planet starting location Positive X, Y, Z axis
    //    public int X = 10;
    //    public int Y = 10;
    //    public int Z = 10;

    public Vector3[,,] planetVertices;
    public Cube[,,] planetData; // 3D array to hold information on all of the cubes and their quads in the planet
    public GameObject planet;

    // The faces of the cube
    // - starting point for the generation of a sphere/planet
    // need to pass the face, so that the cube can be constructed properly????
    public enum QuadSide { Top, Bottom, Front, Back, LeftSide, RightSide }

    // SUBDIVISION RELATED STUFF 

    public int vertexPushDistance = 1; // distance to increase vertex location by after vector equation calculated

    // generate globe 
    public Planet(Vector3 planetPosition)
    {
        this.planetPosition = planetPosition;
        planetData = new Cube[Universe.universeSize, Universe.universeSize, Universe.universeSize];
        planetVertices = new Vector3[Universe.universeSize, Universe.universeSize, Universe.universeSize];
        planet = new GameObject("Planet_" + Universe.BuildPlanetChunkName(planetPosition));
        
        planetChunks = new Dictionary<string, PlanetChunk>();

        // initialise planetVertices ??
        initialiseVerticesArray();

        // build the planet
        GenerateWorld();
    }

    // TODO: this might not be needed now
    private void initialiseVerticesArray()
    {
        // initial planetVertices
        for (int i = 0; i < Universe.universeSize; i++)
        {
            for (int k = 0; k < Universe.universeSize; k++)
            {
                for (int l = 0; l < Universe.universeSize; l++)
                {
                    planetVertices[i, k, l].x = -1;
                    planetVertices[i, k, l].y = -1;
                    planetVertices[i, k, l].z = -1;
                }
            }
        }
    }

    void GenerateWorld()
    {
        for (int chunkYIndex = 0; chunkYIndex < planetSize; chunkYIndex++)
        {
            for (int chunkZIndex = 0; chunkZIndex < planetSize; chunkZIndex++)
            {
                for (int chunkXIndex = 0; chunkXIndex < planetSize; chunkXIndex++)
                {
                    // this.transform.position = planet's coords ?
                    Vector3 chunkPosition = new Vector3(planet.transform.position.x + (chunkXIndex * chunkSize),
                                                        planet.transform.position.y + (chunkYIndex * chunkSize),
                                                        planet.transform.position.z + (chunkZIndex * chunkSize));

                    Debug.Log("Chunk position: " + chunkPosition);
                    PlanetChunk c = new PlanetChunk(this, chunkPosition); // CHANGE THIS!!! include parameter stating biome (desert, jungle, etc.)
                    planetChunks.Add(c.planetChunk.name, c);
                }
            }
        }
        foreach (KeyValuePair<string, PlanetChunk> c in planetChunks)
        {
            //            c.Value.DrawChunk(); // draw the entire chunk
        }

        /* Make the terrain look more natural
         * e.g. Pass over the terrain, working out where to apply gradual blending
         */
        foreach (KeyValuePair<string, PlanetChunk> c in planetChunks)
        {
    //        chunk.Value.MakeTerrainLookReal();
        }

    }

    private void pushVericesOut()
    {
        // iterate through cubes, pushing out vertices
        foreach (Cube cube in planetData)
        {
            Vector3 vectorEquationResult;
            // front quad
            vectorEquationResult = CalculateVector(cube.frontQuadVertices[0]);
            PushVertexOut(vectorEquationResult, vertexPushDistance, cube.frontQuadVertices[0]);
        //    cube.DisplayQuad(cube.frontQuadVertices);
        }
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


}
