using System;
using UnityEditorInternal;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    /*
     * Destroy the chunk when we wish to add or remove a cube
     */
    void DestroyChunk(PlanetChunk chunk)
    {
        DestroyImmediate(chunk.planetChunk.GetComponent<MeshFilter>());
        DestroyImmediate(chunk.planetChunk.GetComponent<MeshRenderer>());
        DestroyImmediate(chunk.planetChunk.GetComponent<Collider>());
    }

    /*
     * Used for digging the terrain
     */
    private void RemoveCube(int x, int y, int z, PlanetChunk chunk)
    {
        DestroyChunk(chunk);
        // TODO: NEED TO KEEP THE CHANGED STATE!!!!
        chunk.ReBuildTheChunk();
        chunk.CubeIsSolid[x, y, z] = false; // stop the cube from being drawn
        chunk.DrawChunk();
    }

    /*
     * Used for building the terrain
     */
    private void AddCube(int x, int y, int z, PlanetChunk chunk)
    {
        DestroyChunk(chunk);
        // TODO: NEED TO KEEP THE CHANGED STATE!!!!
        chunk.ReBuildTheChunk();
        chunk.CubeIsSolid[x, y, z] = true; // draw the cube
        chunk.DrawChunk();
    }

    /*
     * checks to see if we can add a cube to the game world
     * 
     * x, y, z - the block location within the chunk (need to see if we are at the extremes of the chunk)
     * hitChunk - the chunk the Raycast has hit (need to see if this is at the extremes of the planet)
     * x,y,z = chunk extreme AND hitChunk = chunk at the extremes of the planet, THEN cannot add anymore blocks/cubes
     */
    bool NotAtEndOfWorld(int x, int y, int z, PlanetChunk hitChunk)
    {
        // Are we at an end of the planet
        if (hitChunk.chunkPosition.x == 0 && x < 0)
        {
            // we are beyond the planet in negative X
            return false;
        }
        if (hitChunk.chunkPosition.y == 0 && y < 0)
        {
            // we are beyond the planet in negative Y
            return false;
        }
        if (hitChunk.chunkPosition.z == 0 && z < 0)
        {
            // we are beyond the planet in negative Z
            return false;
        }
        // positive extreme
        if (hitChunk.chunkPosition.x == Universe.planet.planetSize && x >= Universe.planet.chunkSize)
        {
            // we are beyond the planet in positive X
            return false;
        }
        if (hitChunk.chunkPosition.y == Universe.planet.planetSize && y >= Universe.planet.chunkSize)
        {
            // we are beyond the planet in positive Y
            return false;
        }
        if (hitChunk.chunkPosition.z == Universe.planet.planetSize && z >= Universe.planet.chunkSize)
        {
            // we are beyond the planet in positive Z
            return false;
        }

        // we have gone beyond the end of the world
        return true; // must not add anymore blocks/cubes
    }


    /*
     * Check to see if we have crossed over to another chunk (required knowledge for building)
     * If we have, then we need to make sure we are working on the chunk we are now within
     * If not, then we sticl with the current chunk
     * 
     * blockLocation - location of the block/cube that the Raycast has hit
     * newLocation - location of the block/cube that is to be placed (during building)
     * hitChunk - chunk that was hit by the Raycast
     * 
     */
    PlanetChunk AtChunkBounds(Vector3 blockLocation, out Vector3 newLocation, PlanetChunk hitChunk)
    {
        // initialise cube's new location (initially set to the cube's loaction that the Raycast hit)
        newLocation.x = blockLocation.x;
        newLocation.y = blockLocation.y;
        newLocation.z = blockLocation.z;

        // Are we beyond the positive extreme of X?
        if (blockLocation.x > Universe.planet.chunkSize-1)
        {
        //    print("We are at the X end");  // if location of block to be placed > PlanetSize * ChunkSize, then next chunk

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex + 1, hitChunk.chunkYIndex, hitChunk.chunkZIndex));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
            //    Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = 0;
                newLocation.y = blockLocation.y;
                newLocation.z = blockLocation.z;

                return newChunk;
            }
            else
            {
            //    Debug.Log("New Chunk NOT FOUND!"); // need to account for reaching end of the planet
                return null;
            }
        }
        // Are we beyond the positive extreme of Y?
        if (blockLocation.y > Universe.planet.chunkSize-1)
        {
        //    print("We are at the Y end"); // if location of block to be placed > PlanetSize * ChunkSize, then next chunk

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex, hitChunk.chunkYIndex + 1, hitChunk.chunkZIndex));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
            //    Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = blockLocation.x;
                newLocation.y = 0;
                newLocation.z = blockLocation.z;

                return newChunk;
            }
            else
            {
            //    Debug.Log("New Chunk NOT FOUND!"); // need to account to reaching end of the planet
                return null;
            }
        }
        // Are we beyond the positive extreme of Z?
        if (blockLocation.z > Universe.planet.chunkSize-1)
        {
        //    print("We are at the Z end"); // if location of block to be placed > PlanetSize * ChunkSize, then next chunk

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex, hitChunk.chunkYIndex, hitChunk.chunkZIndex + 1));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
            //    Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = blockLocation.x;
                newLocation.y = blockLocation.y;
                newLocation.z = 0;

                return newChunk;
            }
            else
            {
            //    Debug.Log("New Chunk NOT FOUND!"); // need to account to reaching end of the planet
                return null;
            }
        }
        // Are we at the negative extreme of X?
        if (blockLocation.x < 0) // if location of block to be place is negative, then we have moved into another chunk
        {
        //    print("We are at the X beginning");

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex - 1, hitChunk.chunkYIndex, hitChunk.chunkZIndex));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
            //    Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = Universe.planet.chunkSize - 1;
                newLocation.y = blockLocation.y;
                newLocation.z = blockLocation.z;

                return newChunk;
            }
            else
            {
            //    Debug.Log("New Chunk NOT FOUND!"); // need to account to reaching end of the planet
                return null;
            }
        }
        // Are we at the negative extreme of Y?
        if (blockLocation.y < 0) // if location of block to be place is negative, then we have moved into another chunk
        {
        //    print("We are at the Y beginning");

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex, hitChunk.chunkYIndex - 1, hitChunk.chunkZIndex));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
            //    Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = blockLocation.x;
                newLocation.y = Universe.planet.chunkSize - 1;
                newLocation.z = blockLocation.z;

                return newChunk;
            }
            else
            {
            //    Debug.Log("New Chunk NOT FOUND!"); // need to account to reaching end of the planet
                return null;
            }
        }
        // Are we at the negative extreme of Z?
        if (blockLocation.z < 0) // if location of block to be place is negative, then we have moved into another chunk
        {
        //    print("We are at the Z beginning");

            // Get neighbouring chunk
            string cn = "Chunk_" + 
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex, hitChunk.chunkYIndex, hitChunk.chunkZIndex - 1));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
            //    Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = blockLocation.x;
                newLocation.y = blockLocation.y;
                newLocation.z = Universe.planet.chunkSize-1;

                return newChunk;
            }
            else
            {
            //    Debug.Log("New Chunk NOT FOUND!");
                return null;
            }
        }
        // we are not beyond the extremes of the chunk, therefore carry on with the current chunk
        return hitChunk;
    }


    private void DiggingAndBuilding()
    {
        // Left mouse button = dig (remove block/cube)
        // Middle mouse button = build (place block/cube)
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
            {
                Vector3 newLocation;

                PlanetChunk hitChunk; //retrieve the chunk that was hit by the Raycast
                if (!Universe.planet.planetChunks.TryGetValue(hit.collider.gameObject.name, out hitChunk)) return;

                Vector3 hitBlock;
                if (Input.GetMouseButton(0)) // Digging - remove block
                {
                    hitBlock = hit.point - hit.normal / 2.0f; // remove a block
                }
                else                         // building - add block
                {
                    hitBlock = hit.point + hit.normal / 2.0f; // add a block
                }

                // This changes the world coords into the coords within a chunk ???????? I have forgotten!!!
                int x = (int)(Mathf.Floor(hitBlock.x) - hit.collider.gameObject.transform.position.x);
                int y = (int)(Mathf.Floor(hitBlock.y) - hit.collider.gameObject.transform.position.y);
                int z = (int)(Mathf.Floor(hitBlock.z) - hit.collider.gameObject.transform.position.z);

                if (Input.GetMouseButton(0))
                {
                    // remove cube
                    RemoveCube(x, y, z, hitChunk); // x y z = coords of the block to add/remove, hitChunk = the chunk the block/cube exists in
                }
                else
                {
                    // add cube
                    // check if coords are within the planet's build area
                    if (NotAtEndOfWorld(x, y, z, hitChunk))
                    {
                        //    print("--== BUILDING ==-- @ " + x + " " + y + " " + z);
                        // IF we are at the edge of the chunk (working across chunks), 
                        // then we need to change the chunk we are working on!
                        hitChunk = AtChunkBounds(new Vector3(x, y, z), out newLocation, hitChunk);
                        AddCube((int)newLocation.x, (int)newLocation.y, (int)newLocation.z, hitChunk);
                    }
                    //    else
                    //        Debug.Log("CANNOT BUILD - END OF WORLD AT: " + x + " " + y + " " + z);
                }

            }
            //    else
            //        Debug.Log("BALLS, nothing to hit!");
        }
    }


    // Update is called once per frame
    // https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
    void Update()
    {
        DiggingAndBuilding();
    }

}
