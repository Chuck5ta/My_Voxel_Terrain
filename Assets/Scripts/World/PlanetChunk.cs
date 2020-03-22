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
    
    /*
     * Constructor
     * chunkZIndex, chunkXIndex is the location of the chunk we are currently working on
     * 
     * e.g. chunk 0 will be based at 0,0,0 in the Universe
     */
    public PlanetChunk(Planet planet,  Vector3 position)
    {
        this.planet = planet;
        this.chunkPosition = position;
        planetChunk = new GameObject("Chunk_" + Universe.BuildPlanetChunkName(position));
        planetChunk.transform.position = position;
        BuildTheChunk();
    }

    void BuildTheChunk()
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
                    // create new cube
                    if (WithinPlanet(cubePosition))
                    {
                        Debug.Log(" CHUNK NAME : " + planetChunk.name);
                        chunkData[x, y, z] = new Cube(chunkVertices, x, y, z,
                                                CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                                CustomMaterials.rockQuad, cubePosition, planetChunk.name);
                    }

        //            GenerateCube(x, y, z, cubePosition); //TODO: need set this to solid
    //              else
    //                  GenerateCube(x, y, z, cubePosition); //TODO: need set this to air

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
        float d = Mathf.Sqrt(((cubePosition.x - planet.planetCentre.x) * (cubePosition.x - planet.planetCentre.x)) +
                    ((cubePosition.y - planet.planetCentre.y) * (cubePosition.y - planet.planetCentre.y)) +
                    ((cubePosition.z - planet.planetCentre.z) * (cubePosition.z - planet.planetCentre.z)));

        if (d < planet.planetRadius)
            return true;
        else Debug.Log("***** Cube not within planet!" + cubePosition.x + " : " + cubePosition.y + " : " + cubePosition.z);
        return false;
    }

}
