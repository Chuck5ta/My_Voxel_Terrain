using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlanetChunk
{
    public Cube[,,] chunkData; // 3D array to hold information on all of the cubes
//    public Vector3[,,] chunkVertices; // 2D array to hold all the coordinates of the vertices that make up the terrain in the chunk
    public GameObject planetChunk;
    public Vector3 chunkPosition;
    public Planet parentPlanet;
    public bool[,,] CubeIsSolid; // states if a block/cube is space or a solid 
    
    /*
     * Constructor
     * chunkZIndex, chunkXIndex is the location of the chunk we are currently working on
     * 
     * e.g. chunk 0 will be based at 0,0,0 in the Universe
     */
    public PlanetChunk(Planet planet,  Vector3 position)
    {
        parentPlanet = planet;
        chunkPosition = position;
        planetChunk = new GameObject("Chunk_" + Universe.BuildPlanetChunkName(position));
        planetChunk.transform.position = position;
        chunkData = new Cube[parentPlanet.chunkSize, parentPlanet.chunkSize, parentPlanet.chunkSize];
        CubeIsSolid = new bool[parentPlanet.chunkSize, parentPlanet.chunkSize, parentPlanet.chunkSize];
    }

    /*
     * Generate the planet, but do not draw the cubes at this point.
     * We need generate first, so that we can see where quads need not be
     * drawn, due to being next to a cube that is or will be drawn during
     * the initial generation of the planet.
     */
    private void SetUpChunk()
    {
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
                    chunkData[x, y, z] = new Cube(planetChunk.gameObject, this,
                                            x, y, z,
                                            CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                            CustomMaterials.rockQuad, cubePosition, planetChunk.name);
                    // create new cube
                    if (IsOuterLayer(cubePosition))
                    {
                        CubeIsSolid[x, y, z] = true;
                    }
                    else // set cube to SPACE
                    {
                        CubeIsSolid[x, y, z] = false;
                    }

                }
            }
        }
    }

    /*
     * This is run after the first time the world is built, when we need to
     * initialise the cubes again - e.g. when digging
     */
    public void ReSetUpChunk()
    {
        for (int y = 0; y < parentPlanet.chunkSize; y++)
        {
            for (int z = 0; z < parentPlanet.chunkSize; z++)
            {
                for (int x = 0; x < parentPlanet.chunkSize; x++)
                {
                    Vector3 cubePosition = new Vector3(planetChunk.transform.position.x + x,
                                                        planetChunk.transform.position.y + y,
                                                        planetChunk.transform.position.z + z);

                    chunkData[x, y, z] = new Cube(planetChunk.gameObject, this,
                                            x, y, z,
                                            CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                            CustomMaterials.rockQuad, cubePosition, planetChunk.name);
                }
            }
        }
    }

    /*
     * Draw the cubes that are on the surface of the planet.
     * Cubes within the planet will be drawn as and when digging/terrain 
     * manipulation occurs.
     */
    public void DrawChunk()
    {
        Debug.Log("***********************************");
        // DRAW THE CHUNK
        for (int y = 0; y < parentPlanet.chunkSize; y++)
        {
            for (int z = 0; z < parentPlanet.chunkSize; z++)
            {
                for (int x = 0; x < parentPlanet.chunkSize; x++)
                {
                    // display cubes that are set to SOLID (surface area cubes only)
                    if (CubeIsSolid[x, y, z])
                    {
                        Debug.Log("SOLID cube @ " + x + "," + y + "," + z);
                        // draw the cube and set it to SOLID
                        chunkData[x, y, z].DrawCube();
                        // combine quads
                //        CombineQuads(chunkData[x, y, z]);
                    }
                }
            }
        }

        CombineQuads();
    }

    public void BuildTheChunk()
    {
        // holds quad info within the chunk
    //    chunkData = new Cube[parentPlanet.chunkSize, parentPlanet.chunkSize, parentPlanet.chunkSize];
        // holds vertices coordinates within the chunk
    //    chunkVertices = new Vector3[parentPlanet.chunkSize + 1, parentPlanet.chunkSize + 1, parentPlanet.chunkSize + 1];

        // SET THE CHUNK UP
        SetUpChunk();
    //        Thread chunkThread;
    //        chunkThread = new Thread(DrawChunk);
    //        chunkThread.Start();
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


    void CombineQuads()
    {
        // combine all children meshes
        MeshFilter[] meshFilters = planetChunk.GetComponentsInChildren<MeshFilter>();
        Debug.Log("Meshfilters : " + meshFilters.Length);

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        // Total quads = meshFilters.Length
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            i++;
        }

        MeshFilter mf = (MeshFilter)planetChunk.gameObject.AddComponent(typeof(MeshFilter));
        mf.GetComponent<MeshFilter>().mesh = new Mesh();
        mf.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        //   MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        MeshRenderer renderer = planetChunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad);
        MeshCollider boxCollider2 = planetChunk.AddComponent<MeshCollider>();

        // Delete all children (quad meshes)
        foreach (Transform quad in planetChunk.transform)
        {
            Object.Destroy(quad.gameObject);
        }
    }



    void CombineQuads(Cube cube)
    {
        // combine all children meshes
        MeshFilter[] meshFilters = cube.cube.GetComponentsInChildren<MeshFilter>();
        Debug.Log("Meshfilters : " + meshFilters.Length);

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        // Total quads = meshFilters.Length
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            i++;
        }

        MeshFilter mf = (MeshFilter)cube.cube.gameObject.AddComponent(typeof(MeshFilter));
        mf.GetComponent<MeshFilter>().mesh = new Mesh();
        mf.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        //   MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        MeshRenderer renderer = cube.cube.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad); 
        MeshCollider boxCollider2 = cube.cube.AddComponent<MeshCollider>();

        // Delete all children (quad meshes)
        foreach (Transform quad in cube.cube.transform)
        {
            Object.Destroy(quad.gameObject);
        } 
    }

}
