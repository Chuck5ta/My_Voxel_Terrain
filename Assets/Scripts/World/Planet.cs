//using System.Collections;
using System.Collections.Generic; // Dictionary structure
//using System.Threading;
using UnityEngine;

public class Planet
{

 //   public int planetSize = 10; // number of chunks
 //   public Vector3 planetCentre = new Vector3(25f, 25f, 25f);
 //   public int planetRadius = 22; // diameter of 14
 //   public int chunkSize = 4; // diameter -  size of chunk 4x4x4 cubes
 //   public Vector3 planetPosition = new Vector3(0, 0, 0); // coordinates of the planet in the universe

    public int planetSize = 2; // number of chunks
    public Vector3 planetCentre = new Vector3(6f, 6f, 6f);
    public int planetRadius = 4; // diameter of 14
    public int chunkSize = 6; // diameter -  size of chunk 4x4x4 cubes
    public Vector3 planetPosition = new Vector3(0,0,0); // coordinates of the planet in the universe

    public Dictionary<string, PlanetChunk> planetChunks;


    // Planet starting location Positive X, Y, Z axis
    //    public int X = 10;
    //    public int Y = 10;
    //    public int Z = 10;

    public Cube[,,] planetData; // 3D array to hold information on all of the cubes and their quads in the planet
    public GameObject planet;

    // The faces of the cube
    // - starting point for the generation of a sphere/planet
    // need to pass the face, so that the cube can be constructed properly????
    public enum QuadSide { Top, Bottom, Front, Back, LeftSide, RightSide }

    // SUBDIVISION RELATED STUFF 

    public int vertexPushDistance = 1; // distance to increase vertex location by after vector equation calculated

    private Material chunkMaterial;

    // generate globe 
    public Planet(Vector3 planetPosition)
    {
        this.planetPosition = planetPosition;
        // planetData = new Cube[Universe.universeSize, Universe.universeSize, Universe.universeSize];
        planet = new GameObject("Planet_" + Universe.BuildPlanetChunkName(planetPosition));
        
        planetChunks = new Dictionary<string, PlanetChunk>();

        // build the planet
        GenerateWorld();
    }

    Material GetNextMaterial(int cubeCount)
    {
        switch (cubeCount)
        {
            case 1:
                return CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad);
            case 2:
                return CustomMaterials.RetrieveMaterial(CustomMaterials.sandQuad);
            case 3:
                return CustomMaterials.RetrieveMaterial(CustomMaterials.grassQuad);
            case 4:
                return CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad);
            case 5:
                return CustomMaterials.RetrieveMaterial(CustomMaterials.sandQuad);
            case 6:
                return CustomMaterials.RetrieveMaterial(CustomMaterials.grassQuad);
            case 7:
                return CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad);
            default:
                return CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad);
        }
    }


    void GenerateWorld()
    {
        // Thread[] rowOfChunks = new Thread[Universe.chunkSize * Universe.chunkSize * Universe.chunkSize];
        // Thread chunkThread;
        int cubeCount = 1;
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
                    //            GenerateChunk(chunkPosition);
                    //             Debug.Log("Chunk position: " + chunkPosition);

                    // THREADING http://www.albahari.com/threading/
                    chunkMaterial = GetNextMaterial(cubeCount);

                    PlanetChunk planetChunk = new PlanetChunk(planet.gameObject, this, chunkPosition, chunkMaterial); // CHANGE THIS!!! include parameter stating biome (desert, jungle, etc.)
                    Debug.Log("Adding chunk to planetChunks : " + planetChunk.planetChunk.name + " at coords : " + chunkPosition);
                    planetChunk.planetChunk.transform.parent = planet.transform;

                    planetChunks.Add(planetChunk.planetChunk.name, planetChunk);
                    planetChunk.BuildTheChunk();
                    planetChunk.DrawChunk();

                    cubeCount++;

                    //           t2 = new Thread(() => Console.WriteLine(text));
                    //        Debug.Log("----------------------------");
                    //        Debug.Log("Generating chunk @ " + chunkPosition);
                    //        Debug.Log("============================");
                    //            chunkThread = new Thread(() => GenerateChunk(chunkPosition));
                    //            chunkThread.Start();
                    //            chunkThread.IsBackground = true;
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

    private void GenerateChunk(Vector3 chunkPosition)
    {
        PlanetChunk c = new PlanetChunk(planet.gameObject, this, chunkPosition, chunkMaterial);

        planetChunks.Add(c.planetChunk.name, c);
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

        return pushOutVertex;
    }


}
