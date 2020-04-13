//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;
using UnityEngine;

public class PlanetChunk
{
    public Cube[,,] chunkData; // 3D array to hold information on all of the cubes
                               //    public Vector3[,,] chunkVertices; // 2D array to hold all the coordinates of the vertices that make up the terrain in the chunk
    public GameObject planetChunk;
    public GameObject parent;
    public Planet owner;
    public Vector3 chunkPosition;
    public bool[,,] CubeIsSolid; // states if a block/cube is space or a solid 

    private Material chunkMaterial;
    
    /*
     * Constructor
     * chunkZIndex, chunkXIndex is the location of the chunk we are currently working on
     * 
     * e.g. chunk 0 will be based at 0,0,0 in the Universe
     */
    public PlanetChunk(GameObject parent, Planet owner,  Vector3 position, Material material)
    {
        this.parent = parent;
        this.owner = owner;
        chunkPosition = position;
        chunkMaterial = material;
        planetChunk = new GameObject("Chunk_" + Universe.BuildPlanetChunkName(position));

        // TODO: THIS IS NOT PLACING THE CHUNK IN THE RIGHT LOCATION!!!!
    //    planetChunk.transform.position = position;
        Debug.Log("Chunk location : " + planetChunk.transform.position);

        chunkData = new Cube[owner.chunkSize, owner.chunkSize, owner.chunkSize];
        CubeIsSolid = new bool[owner.chunkSize, owner.chunkSize, owner.chunkSize];

     //   planetChunk.transform.parent = parent.transform; // make the planetChunk a child of the planet

    }

    /*
     * Generate the planet, but do not draw the cubes at this point.
     * We need generate first, so that we can see where quads need not be
     * drawn, due to being next to a cube that is or will be drawn during
     * the initial generation of the planet.
     */
    public void BuildTheChunk()
    {
        for (int y = 0; y < owner.chunkSize; y++)
        {
            for (int z = 0; z < owner.chunkSize; z++)
            {
                for (int x = 0; x < owner.chunkSize; x++)
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

                    chunkData[x, y, z].cube.transform.parent = planetChunk.transform; // make the quad a child of the cube
                    // create new cube
                    if (IsOuterLayer(cubePosition))
                    {
                        CubeIsSolid[x, y, z] = true;
                    }
                    else // set cube to SPACE
                    {
                        CubeIsSolid[x, y, z] = false;
                    }
                    CubeIsSolid[x, y, z] = true;

                }
            }
        }
    }

    /*
     * This is run after the first time the world is built, when we need to
     * initialise the cubes again - e.g. when digging
     */
    public void ReBuildTheChunk()
    {
        for (int y = 0; y < owner.chunkSize; y++)
        {
            for (int z = 0; z < owner.chunkSize; z++)
            {
                for (int x = 0; x < owner.chunkSize; x++)
                {
                    //         Vector3 cubePosition = new Vector3(planetChunk.transform.position.x + x,
                    //                                             planetChunk.transform.position.y + y,
                    //                                             planetChunk.transform.position.z + z);

                    Vector3 cubePosition = new Vector3(x, y, z);
                    chunkData[x, y, z] = new Cube(planetChunk.gameObject, this,
                                            x, y, z,
                                            CustomMaterials.RetrieveMaterial(CustomMaterials.rockQuad),
                                            CustomMaterials.rockQuad, cubePosition, planetChunk.name);

                    chunkData[x, y, z].cube.transform.parent = planetChunk.transform; // make the quad a child of the cube
                }
            }
        }

      //  planetChunk.transform.parent = parent.transform; // make the planetChunk a child of the planet
    }

    /*
     * Draw the cubes that are on the surface of the planet.
     * Cubes within the planet will be drawn as and when digging/terrain 
     * manipulation occurs.
     */
    public void DrawChunk()
    {
        // DRAW THE CHUNK
        for (int y = 0; y < owner.chunkSize; y++)
        {
            for (int z = 0; z < owner.chunkSize; z++)
            {
                for (int x = 0; x < owner.chunkSize; x++)
                {
                    // display cubes that are set to SOLID (surface area cubes only)
                    if (CubeIsSolid[x, y, z])
                    {
                        // draw the cube and set it to SOLID
                        chunkData[x, y, z].DrawCube();
                    }
                }
            }
        }
        CombineCubes();
        MeshCollider collider = planetChunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        collider.sharedMesh = planetChunk.transform.GetComponent<MeshFilter>().mesh;
    }

    /*
     * Tests to see if the cube is part of the outer layer of the planet. If so
     * then we want to have it visible.
     * If the cube is not on the outer layer, then it is set to SPACE and is invisible
     */
    private bool IsOuterLayer(Vector3 cubePosition)
    {
        float d = Mathf.Sqrt(((cubePosition.x - owner.planetCentre.x) * (cubePosition.x - owner.planetCentre.x)) +
                    ((cubePosition.y - owner.planetCentre.y) * (cubePosition.y - owner.planetCentre.y)) +
                    ((cubePosition.z - owner.planetCentre.z) * (cubePosition.z - owner.planetCentre.z)));

        if (d < owner.planetRadius && owner.planetRadius - d < 2) // ensures that only the surface cube are generated
            return true;

        return false;
    }

    /*
     * TODO: Might not need this anymore
     * Tests to see if the current cube will spawn within the planet
     * If not, then it must not be generated/displayed (or do we set it to space/air?)
     */
    private bool WithinPlanet(Vector3 cubePosition)
    {   
        float d = Mathf.Sqrt(((cubePosition.x - owner.planetCentre.x) * (cubePosition.x - owner.planetCentre.x)) +
                    ((cubePosition.y - owner.planetCentre.y) * (cubePosition.y - owner.planetCentre.y)) +
                    ((cubePosition.z - owner.planetCentre.z) * (cubePosition.z - owner.planetCentre.z)));

        if (d < owner.planetRadius)
            return true;

        return false;
    }


    public void CombineCubes() // https://answers.unity.com/questions/231249/instanced-meshes-are-being-offset-to-weird-positio.html
    {
        // https://answers.unity.com/questions/231249/instanced-meshes-are-being-offset-to-weird-positio.html
        Matrix4x4 myTransform = parent.transform.worldToLocalMatrix;

        // combine all children meshes
        MeshFilter[] meshFilters = planetChunk.GetComponentsInChildren<MeshFilter>();
        Debug.Log("-----===== COMBINE : Meshfilters CUBES : " + meshFilters.Length + " for chunk : " + planetChunk.name);

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        // Total cubes = meshFilters.Length
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = myTransform * meshFilters[i].transform.localToWorldMatrix;

            i++;
        }

        MeshFilter mf = (MeshFilter)planetChunk.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();
        mf.mesh.CombineMeshes(combine);

        planetChunk.GetComponent<MeshFilter>().sharedMesh = mf.mesh;

        MeshRenderer renderer = planetChunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = chunkMaterial;

        planetChunk.transform.position = chunkPosition;

        // Delete all children (quad meshes)
        int cubeCounter = 0;
        foreach (Transform cube in planetChunk.transform)
        {
           Object.Destroy(cube.gameObject);
            cubeCounter++;
        }
    }

}
