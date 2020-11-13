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
     */
    bool NotAtEndOfWorld(int x, int y, int z, PlanetChunk hitChunk)
    {
        // Are we at an end chunk
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
            print("X IS OVER: " + x);
            // we are beyond the planet in positive X
            return false;
        }
        if (hitChunk.chunkPosition.y == Universe.planet.planetSize && y >= Universe.planet.chunkSize)
        {
            print("Y IS OVER: " + y);
            // we are beyond the planet in positive Y
            return false;
        }
        if (hitChunk.chunkPosition.z == Universe.planet.planetSize && z >= Universe.planet.chunkSize)
        {
            print("Z IS OVER: " + z);
            // we are beyond the planet in positive Z
            return false;
        }
        print("X Y Z ARE ALL GOOD: " + x + " " + y + " " + z);

        // we have gone beyond the end of the world
        return true;
    }


    /*
     * Check to see if we have crossed over to another chunk (required knowledge for building)
     * If we have, then we need to make sure we are working on the chunk we are now within
     */
    PlanetChunk AtChunkBounds(Vector3 blockLocation, out Vector3 newLocation, PlanetChunk hitChunk)
    {
        // initialise cube's new location
        newLocation.x = blockLocation.x;
        newLocation.y = blockLocation.y;
        newLocation.z = blockLocation.z;

        if (blockLocation.x > Universe.planet.chunkSize-1)
        {
            print("We are at the X end");  // if location of block to be placed > PlanetSize * ChunkSize, then next chunk

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex + 1, hitChunk.chunkYIndex, hitChunk.chunkZIndex));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
                Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = 0;
                newLocation.y = blockLocation.y;
                newLocation.z = blockLocation.z;

                return newChunk;
            }
            else
            {
                Debug.Log("New Chunk NOT FOUND!"); // need to account to reaching end of the planet
                return null;
            }
        }
        if (blockLocation.y > Universe.planet.chunkSize-1)
        {
            print("We are at the Y end"); // if location of block to be placed > PlanetSize * ChunkSize, then next chunk

            Debug.Log("Current chunk is: " + hitChunk.chunkXIndex + " " + hitChunk.chunkYIndex + " " + hitChunk.chunkZIndex);
            Debug.Log("Next chunk is: " + hitChunk.chunkXIndex + " " + hitChunk.chunkYIndex + " " + (hitChunk.chunkZIndex + 1));

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex, hitChunk.chunkYIndex + 1, hitChunk.chunkZIndex));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
                Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = blockLocation.x;
                newLocation.y = 0;
                newLocation.z = blockLocation.z;

                return newChunk;
            }
            else
            {
                Debug.Log("New Chunk NOT FOUND!"); // need to account to reaching end of the planet
                return null;
            }
        }
        if (blockLocation.z > Universe.planet.chunkSize-1)
        {
            print("We are at the Z end"); // if location of block to be placed > PlanetSize * ChunkSize, then next chunk

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex, hitChunk.chunkYIndex, hitChunk.chunkZIndex + 1));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
                Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = blockLocation.x;
                newLocation.y = blockLocation.y;
                newLocation.z = 0;

                return newChunk;
            }
            else
            {
                Debug.Log("New Chunk NOT FOUND!"); // need to account to reaching end of the planet
                return null;
            }
        }
        if (blockLocation.x < 0) // if location of block to be place is negative, then we have moved into another chunk
        {
            print("We are at the X beginning");

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex - 1, hitChunk.chunkYIndex, hitChunk.chunkZIndex));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
                Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = Universe.planet.chunkSize - 1;
                newLocation.y = blockLocation.y;
                newLocation.z = blockLocation.z;

                return newChunk;
            }
            else
            {
                Debug.Log("New Chunk NOT FOUND!"); // need to account to reaching end of the planet
                return null;
            }
        }
        if (blockLocation.y < 0) // if location of block to be place is negative, then we have moved into another chunk
        {
            print("We are at the Y beginning");

            // Get neighbouring chunk
            string cn = "Chunk_" +
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex, hitChunk.chunkYIndex - 1, hitChunk.chunkZIndex));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
                Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = blockLocation.x;
                newLocation.y = Universe.planet.chunkSize - 1;
                newLocation.z = blockLocation.z;

                return newChunk;
            }
            else
            {
                Debug.Log("New Chunk NOT FOUND!"); // need to account to reaching end of the planet
                return null;
            }
        }
        if (blockLocation.z < 0) // if location of block to be place is negative, then we have moved into another chunk
        {
            print("We are at the Z beginning");

            // Get neighbouring chunk
            string cn = "Chunk_" + 
                Universe.BuildPlanetChunkName(new Vector3(hitChunk.chunkXIndex, hitChunk.chunkYIndex, hitChunk.chunkZIndex - 1));

            // does that chunk exist?
            PlanetChunk newChunk;
            if (Universe.planet.planetChunks.TryGetValue(cn, out newChunk))
            {
                Debug.Log("New Chunk FOUND!");

                // set the cube's location
                newLocation.x = blockLocation.x;
                newLocation.y = blockLocation.y;
                newLocation.z = Universe.planet.chunkSize-1;

                return newChunk;
            }
            else
            {
                Debug.Log("New Chunk NOT FOUND!");
                return null;
            }
        }
        return hitChunk;
    }


    // Update is called once per frame
    // https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
            {
                Vector3 newLocation;

                PlanetChunk hitChunk; //retrieve the chunk that was hit by the raytrace
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

                // This changes the world coords into the coords within a chunk
                int x = (int)(Mathf.Floor(hitBlock.x) - hit.collider.gameObject.transform.position.x);
                int y = (int)(Mathf.Floor(hitBlock.y) - hit.collider.gameObject.transform.position.y);
                int z = (int)(Mathf.Floor(hitBlock.z) - hit.collider.gameObject.transform.position.z);

                if (Input.GetMouseButton(0))
                {
                    // remove cube
                    RemoveCube(x, y, z, hitChunk); // x y z = coords of the block to add/remove, hitChunk = the chunk the block exists in
                }
                else
                {
                    // add cube
                    // check if coords are within the planet's build area
                    if (NotAtEndOfWorld(x, y, z, hitChunk))
                    {
                        print("--== BUILDING ==-- @ " + x + " " + y + " " + z);
                        // IF we are at the edge of the chunk (working across chunks), then we need to change the chunk we are working on!
                        hitChunk = AtChunkBounds(new Vector3(x, y, z), out newLocation, hitChunk);
                        x = (int)newLocation.x;
                        y = (int)newLocation.y;
                        z = (int)newLocation.z;

                        print("PLACING NEW CUBE AT: " + x + " " + y + " " + z);

                        AddCube(x, y, z, hitChunk);
                    }
                    else
                        Debug.Log("CANNOT BUILD - END OF WORLD AT: " + x + " " + y + " " + z);
                }  

            }
            else
                Debug.Log("BALLS, nothing to hit!");
        }
    }


}
