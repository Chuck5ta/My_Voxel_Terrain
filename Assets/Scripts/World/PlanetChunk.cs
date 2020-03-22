using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetChunk
{
    public Cube[,,] chunkData; // 2D array to hold information on all of the quads in the chunk
                              // I may have to build the Universe in blocks, if this quad attempt does not work out,
                              // then chunkData will become a 3D array
    public Vector3[,,] chunkVertices; // 2D array to hold all the coordinates of the vertices that make up the terrain in the chunk
    public GameObject planetChunk;
    public Vector3 chunkPosition;
    public Planet planet;

    // represents current chunk being worked on
    private int chunkXIndex, chunkYIndex, chunkZIndex;

    /*
     * Constructor
     * chunkZIndex, chunkXIndex is the location of the chunk we are currently working on
     * 
     * e.g. chunk 0 will be based at 0,0,0 in the Universe
     */
    public PlanetChunk(Planet planet,  int chunkXIndex, int chunkYIndex, int chunkZIndex, Vector3 position)
    {
        this.planet = planet;
        this.chunkPosition = position;
        this.chunkXIndex = chunkXIndex;
        this.chunkYIndex = chunkYIndex;
        this.chunkZIndex = chunkZIndex;
    //    planetChunk = new GameObject(Universe.BuildChunkName(position));
        planetChunk = new GameObject("Chunk_" + Universe.BuildPlanetChunkName(position));
        planetChunk.transform.position = position;
        BuildTheChunk(chunkXIndex, chunkYIndex, chunkZIndex);
    }

    void BuildTheChunk(int chunkXIndex, int chunkYIndex, int chunkZIndex)
    {
        // holds quad info within the chunk
        chunkData = new Cube[planet.chunkSize, planet.chunkSize, planet.chunkSize];
        // holds vertices coordinates within the chunk
        chunkVertices = new Vector3[planet.chunkSize + 1, planet.chunkSize + 1, planet.chunkSize + 1];

        for (int y = 0; y < planet.chunkSize; y++)
        {
            for (int z = 0; z < planet.chunkSize; z++)
            {
                for (int x = 0; x < planet.chunkSize; x++)
                {
                    // generate cube - solid or space/air?
                    Vector3 cubePosition = new Vector3(planetChunk.transform.position.x + x,
                                                        planetChunk.transform.position.y + y,
                                                        planetChunk.transform.position.z + z);
                    // is cube within planet's area
                    //       if (WithinPlanet(cubePosition))
                    //           GenerateCube(x, y, z, cubePosition); //TODO: need set this to solid
                    // create new cube
                    Debug.Log(" CHUNK NAME : " + planetChunk.name);
                    chunkData[x, y, z] = new Cube(chunkVertices, x, y, z,
                                            CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                            CustomMaterials.rockQuad, cubePosition, planetChunk.name);

        //            GenerateCube(x, y, z, cubePosition); //TODO: need set this to solid
                                                         //                else
                                                         //                    GenerateCube(x, y, z, cubePosition); //TODO: need set this to air

                }
            }
        }
    }

    /*
     * Tests to see if the current cube will spawn within the planet
     * If not, then it must not be generated/displayed (or do we set it to space/air?)
     */
    private bool WithinPlanet(Vector3 cubePosition)
    {
        // d = ((x2 - x1)2 + (y2 - y1)2 + (z2 - z1)2)
   //     float distance = Mathf.Sqrt((cubePosition.x * cubePosition.x) + (cubePosition.y * cubePosition.y) + (cubePosition.z * cubePosition.z));
        //    d =      
        float d = Mathf.Sqrt(((cubePosition.x - planet.planetCentre.x) * (cubePosition.x - planet.planetCentre.x)) +
                    ((cubePosition.y - planet.planetCentre.y) * (cubePosition.y - planet.planetCentre.y)) +
                    ((cubePosition.z - planet.planetCentre.z) * (cubePosition.z - planet.planetCentre.z)));

        Debug.Log("Planet centre: " + d + " : " + planet.planetCentre.x + " : " + planet.planetCentre.y + " : " + planet.planetCentre.z);
        Debug.Log("SQR ROOT: " + d + " : " + cubePosition.x + " : " + cubePosition.y + " : " + cubePosition.z);
        if (d < planet.planetRadius)
            return true;
        else Debug.Log("***** Cube not within planet!" + cubePosition.x + " : " + cubePosition.y + " : " + cubePosition.z);
        return false;
    }


    private void GenerateCube2(int X, int Y, int Z, Vector3 cubePosition)
    {
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

        // create new cube
        Debug.Log(" CHUNK NAME : " + planetChunk.name);
        chunkData[X, Y, Z] = new Cube(chunkVertices, X, Y, Z,
                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                CustomMaterials.rockQuad, cubePosition, planetChunk.name);
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
    }

}
