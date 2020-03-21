using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    // must always be an odd number, or otherwise subdivision fails
    public int planetSize = 1; // number of chunks
    public Vector3 planetCentre = new Vector3(10f, 10f, 10f);
    public int planetRadius = 30; // diameter of 14
    public int chunkSize = 20; // diameter -  size of chunk 4x4x4 cubes
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
    //    BuildThePlanet();



        // create initial structure (cube)
        //        generateInitialCubeStructure();

        // store cube's quads


        // subdive quads
    //    subdivideQuads();

        // push vertices out bt 1

        // store quads

        // display quads
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
                    PlanetChunk c = new PlanetChunk(this, chunkXIndex, chunkYIndex, chunkZIndex, chunkPosition); // CHANGE THIS!!! include parameter stating biome (desert, jungle, etc.)
              //      c.planetChunk.transform.parent = planet.transform;
              //      Debug.Log("Chunk created: " + c.planetChunk.name);
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



    void BuildThePlanet()
    {
        for (int y = 0; y < Universe.universeSize - 1; y++)
        {
            for (int z = 0; z < Universe.universeSize - 1; z++)
            {
                for (int x = 0; x < Universe.universeSize - 1; x++)
                {
                    // generate cube - solid or space/air?
                    GenerateCube(x, y, z);
                }
            }
        }
    }

    private void GenerateCube(int X, int Y, int Z)
    {
        // Front quad
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y + 1, Z] = new Vector3(X, Y + 1, Z);
        planetVertices[X + 1, Y, Z] = new Vector3(X + 1, Y, Z);
        planetVertices[X + 1, Y + 1, Z] = new Vector3(X + 1, Y + 1, Z);

        // Top quad
        planetVertices[X, Y + 1, Z] = new Vector3(X, Y + 1, Z);
        planetVertices[X, Y + 1, Z + 1] = new Vector3(X, Y + 1, Z + 1);
        planetVertices[X + 1, Y + 1, Z] = new Vector3(X + 1, Y + 1, Z);
        planetVertices[X + 1, Y + 1, Z + 1] = new Vector3(X + 1, Y + 1, Z + 1);

        // Bottom quad
        planetVertices[X + 1, Y, Z] = new Vector3(X + 1, Y, Z);
        planetVertices[X + 1, Y, Z + 1] = new Vector3(X + 1, Y, Z + 1);
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y, Z + 1] = new Vector3(X, Y, Z + 1);

        // Back quad
        planetVertices[X + 1, Y, Z + 1] = new Vector3(X + 1, Y, Z + 1);
        planetVertices[X + 1, Y + 1, Z + 1] = new Vector3(X + 1, Y + 1, Z + 1);
        planetVertices[X, Y, Z + 1] = new Vector3(X, Y, Z + 1);
        planetVertices[X, Y + 1, Z + 1] = new Vector3(X, Y + 1, Z + 1);

        // Left quad
        planetVertices[X, Y, Z + 1] = new Vector3(X, Y, Z + 1);
        planetVertices[X, Y + 1, Z + 1] = new Vector3(X, Y + 1, Z + 1);
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y + 1, Z] = new Vector3(X, Y + 1, Z);

        // Right quad
        planetVertices[X + 1, Y, Z] = new Vector3(X + 1, Y, Z);
        planetVertices[X + 1, Y + 1, Z] = new Vector3(X + 1, Y + 1, Z);
        planetVertices[X + 1, Y, Z + 1] = new Vector3(X + 1, Y, Z + 1);
        planetVertices[X + 1, Y + 1, Z + 1] = new Vector3(X + 1, Y + 1, Z + 1);

        // STORE THE ABOVE AS THE FIRST CUBE

        // create new cube
 //       planetData[X, Y, Z] = new Cube(planetVertices, X, Y, Z,
 //                               CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
 //                               CustomMaterials.rockQuad);
        // store front quad
        planetData[X, Y, Z].storeFrontQuadData(planetVertices[X, Y, Z],
                                            planetVertices[X, Y + 1, Z],
                                            planetVertices[X + 1, Y, Z],
                                            planetVertices[X + 1, Y + 1, Z]);
        // store top quad
        planetData[X, Y, Z].storeTopQuadData(planetVertices[X, Y + 1, Z],
                                            planetVertices[X, Y + 1, Z + 1],
                                            planetVertices[X + 1, Y + 1, Z],
                                            planetVertices[X + 1, Y + 1, Z + 1]);
        // store bottom quad
        planetData[X, Y, Z].storeBottomQuadData(planetVertices[X + 1, Y, Z],
                                            planetVertices[X + 1, Y, Z + 1],
                                            planetVertices[X, Y, Z],
                                            planetVertices[X, Y, Z + 1]);
        // store back quad
        planetData[X, Y, Z].storeBackQuadData(planetVertices[X + 1, Y, Z + 1],
                                            planetVertices[X + 1, Y + 1, Z + 1],
                                            planetVertices[X, Y, Z + 1],
                                            planetVertices[X, Y + 1, Z + 1]);
        // store left quad
        planetData[X, Y, Z].storeLeftQuadData(planetVertices[X, Y, Z + 1],
                                            planetVertices[X, Y + 1, Z + 1],
                                            planetVertices[X, Y, Z],
                                            planetVertices[X, Y + 1, Z]);
        // store right quad
        planetData[X, Y, Z].storeRightQuadData(planetVertices[X + 1, Y, Z],
                                            planetVertices[X + 1, Y + 1, Z],
                                            planetVertices[X + 1, Y, Z + 1],
                                            planetVertices[X + 1, Y + 1, Z + 1]);
    }


    /*
     * The cube (1x1x1) is the base shape for generating the quad filled sphere/planet
     */
    private void generateInitialCubeStructure()
    {
        /*
        // Front quad
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y + planetSize - 1, Z] = new Vector3(X, Y + planetSize - 1, Z);
        planetVertices[X + planetSize - 1, Y, Z] = new Vector3(X + planetSize - 1, Y, Z);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z);

        // Top quad
        planetVertices[X, Y + planetSize - 1, Z] = new Vector3(X, Y + planetSize - 1, Z);
        planetVertices[X, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X, Y + planetSize - 1, Z + planetSize - 1);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1);

        // Bottom quad
        planetVertices[X + planetSize - 1, Y, Z] = new Vector3(X + planetSize - 1, Y, Z);
        planetVertices[X + planetSize - 1, Y, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y, Z + planetSize - 1);
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y, Z + planetSize - 1] = new Vector3(X, Y, Z + planetSize - 1);

        // Back quad
        planetVertices[X + planetSize - 1, Y, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y, Z + planetSize - 1);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1);
        planetVertices[X, Y, Z + planetSize - 1] = new Vector3(X, Y, Z + planetSize - 1);
        planetVertices[X, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X, Y + planetSize - 1, Z + planetSize - 1);

        // Left quad
        planetVertices[X, Y, Z + planetSize - 1] = new Vector3(X, Y, Z + planetSize - 1);
        planetVertices[X, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X, Y + planetSize - 1, Z + planetSize - 1);
        planetVertices[X, Y, Z] = new Vector3(X, Y, Z);
        planetVertices[X, Y + planetSize - 1, Z] = new Vector3(X, Y + planetSize - 1, Z);

        // Right quad
        planetVertices[X + planetSize - 1, Y, Z] = new Vector3(X + planetSize - 1, Y, Z);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z);
        planetVertices[X + planetSize - 1, Y, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y, Z + planetSize - 1);
        planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1] = new Vector3(X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1);
        
        // STORE THE ABOVE AS THE FIRST CUBE

        // create new cube
        planetData[X, Y, Z] = new Cube(planetVertices, X, Y, Z,
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad);
        // store front quad
        planetData[X, Y, Z].storeFrontQuadData(planetVertices[X, Y, Z],
                                            planetVertices[X, Y + planetSize - 1, Z],
                                            planetVertices[X + planetSize - 1, Y, Z],
                                            planetVertices[X + planetSize - 1, Y + planetSize - 1, Z]);
        // store top quad
        planetData[X, Y, Z].storeTopQuadData(planetVertices[X, Y + planetSize - 1, Z],
                                            planetVertices[X, Y + planetSize - 1, Z + planetSize - 1],
                                            planetVertices[X + planetSize - 1, Y + planetSize - 1, Z],
                                            planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1]);
        // store bottom quad
        planetData[X, Y, Z].storeBottomQuadData(planetVertices[X + planetSize - 1, Y, Z],
                                            planetVertices[X + planetSize - 1, Y, Z + planetSize - 1],
                                            planetVertices[X, Y, Z],
                                            planetVertices[X, Y, Z + planetSize - 1]);
        // store back quad
        planetData[X, Y, Z].storeBackQuadData(planetVertices[X + planetSize - 1, Y, Z + planetSize - 1],
                                            planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1],
                                            planetVertices[X, Y, Z + planetSize - 1],
                                            planetVertices[X, Y + planetSize - 1, Z + planetSize - 1]);
        // store left quad
        planetData[X, Y, Z].storeLeftQuadData(planetVertices[X, Y, Z + planetSize - 1],
                                            planetVertices[X, Y + planetSize - 1, Z + planetSize - 1],
                                            planetVertices[X, Y, Z],
                                            planetVertices[X, Y + planetSize - 1, Z]);
        // store right quad
        planetData[X, Y, Z].storeRightQuadData(planetVertices[X + planetSize - 1, Y, Z],
                                            planetVertices[X + planetSize - 1, Y + planetSize - 1, Z],
                                            planetVertices[X + planetSize - 1, Y, Z + planetSize - 1],
                                            planetVertices[X + planetSize - 1, Y + planetSize - 1, Z + planetSize - 1]);

        */
    }

    private void subdivideQuads()
    {
        // subdivide and push out all external surfaces (only external surfaces???)
        // internal surface should be subdived only

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
