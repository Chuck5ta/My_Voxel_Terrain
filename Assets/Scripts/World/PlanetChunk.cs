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
    public Planet parentPlanet;
    
    /*
     * Constructor
     * chunkZIndex, chunkXIndex is the location of the chunk we are currently working on
     * 
     * e.g. chunk 0 will be based at 0,0,0 in the Universe
     */
    public PlanetChunk(Planet planet,  Vector3 position)
    {
        this.parentPlanet = planet;
        this.chunkPosition = position;
        planetChunk = new GameObject("Chunk_" + Universe.BuildPlanetChunkName(position));
        planetChunk.transform.position = position;
        BuildTheChunk();
    }

    void BuildTheChunk()
    {
        // holds quad info within the chunk
        chunkData = new Cube[parentPlanet.chunkSize, parentPlanet.chunkSize, parentPlanet.chunkSize];
        // holds vertices coordinates within the chunk
        chunkVertices = new Vector3[parentPlanet.chunkSize + 1, parentPlanet.chunkSize + 1, parentPlanet.chunkSize + 1];

        // SET THE CHUNK UP
        for (int y = 0; y < parentPlanet.chunkSize; y++)
        {
            for (int z = 0; z < parentPlanet.chunkSize; z++)
            {
                for (int x = 0; x < parentPlanet.chunkSize; x++)
                {
                    // generate cube - solid or space/air?
                    Vector3 cubePosition = new Vector3(planetChunk.transform.position.x + x,
                                                        planetChunk.transform.position.y + y,
                                                        planetChunk.transform.position.z + z);

           //         Debug.Log(" CHUNK NAME : " + planetChunk.name);
                    chunkData[x, y, z] = new Cube(chunkVertices, x, y, z,
                                            CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                            CustomMaterials.rockQuad, cubePosition, planetChunk.name, this);
                    // create new cube
                    if (IsOuterLayer(cubePosition))
                    {
              //          Debug.Log(x + "," + y + "," + z + " Cube is SOLID");
                        chunkData[x, y, z].SetPhysicalState(Cube.CubePhysicalState.SOLID);
                    }
                    else // set cube to SPACE
                        chunkData[x, y, z].SetPhysicalState(Cube.CubePhysicalState.SPACE);

                }
            }
        }
        // DRAW THE CHUNK
        for (int y = 0; y < parentPlanet.chunkSize; y++)
        {
            for (int z = 0; z < parentPlanet.chunkSize; z++)
            {
                for (int x = 0; x < parentPlanet.chunkSize; x++)
                {
                    // display cubes that are set to SOLID (surface area cubes only)
                    if (chunkData[x, y, z].GetPhysicalState() == Cube.CubePhysicalState.SOLID)
                    {
                        // draw the cube and set it to SOLID
                        chunkData[x, y, z].DrawCube();
                    }
                }
            }
        }
    }

    /*
     * Tests to see if the cube is part of the outer layer of the planet. If so
     * then we want to have it visible.
     * If the cube is not on the outer layer, then it is set to SPACE and is invisible
     */
    private bool IsOuterLayer(Vector3 cubePosition)
    {
        float d = Mathf.Sqrt(((cubePosition.x - parentPlanet.planetCentre.x) * (cubePosition.x - parentPlanet.planetCentre.x)) +
                    ((cubePosition.y - parentPlanet.planetCentre.y) * (cubePosition.y - parentPlanet.planetCentre.y)) +
                    ((cubePosition.z - parentPlanet.planetCentre.z) * (cubePosition.z - parentPlanet.planetCentre.z)));

        if (d < parentPlanet.planetRadius && parentPlanet.planetRadius - d < 2) // ensures that only the surface cube are generated
            return true;
        else 
            return false;
    }

    /*
     * Tests to see if the current cube will spawn within the planet
     * If not, then it must not be generated/displayed (or do we set it to space/air?)
     */
    private bool WithinPlanet(Vector3 cubePosition)
    {   
        float d = Mathf.Sqrt(((cubePosition.x - parentPlanet.planetCentre.x) * (cubePosition.x - parentPlanet.planetCentre.x)) +
                    ((cubePosition.y - parentPlanet.planetCentre.y) * (cubePosition.y - parentPlanet.planetCentre.y)) +
                    ((cubePosition.z - parentPlanet.planetCentre.z) * (cubePosition.z - parentPlanet.planetCentre.z)));

        if (d < parentPlanet.planetRadius)
            return true;
  //      else Debug.Log("***** Cube not within planet!" + cubePosition.x + " : " + cubePosition.y + " : " + cubePosition.z);
        return false;
    }

}
