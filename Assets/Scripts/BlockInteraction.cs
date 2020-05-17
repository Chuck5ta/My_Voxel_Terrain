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
        Debug.Log("CUBE IS AT : " + x + " " + y + " " + z);
        chunk.CubeIsSolid[x, y, z] = true; // stop the cube from being drawn
        chunk.DrawChunk();
    }

    /*
     * checks to see if we can add a cube to the game world
     */
    bool NotAtEndOfWorld(int x, int y, int z)
    {
        if ((x >= 0 && x <= 5) && // change this to match the values in Planet!
            (y >= 0 && y <= 5) &&
            (z >= 0 && z <= 5))
            return true;
        // we have gone beyond the end of the world
        return false;
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
                PlanetChunk chunk; //retrieve the chunk
                if (!Universe.planet.planetChunks.TryGetValue(hit.collider.gameObject.name, out chunk)) return;

                Vector3 hitBlock;
                if (Input.GetMouseButton(0))
                {
                    hitBlock = hit.point - hit.normal / 2.0f; // remove a block
                }
                else
                    hitBlock = hit.point + hit.normal / 2.0f; // add a block

                int x = (int)(Mathf.Floor(hitBlock.x) - hit.collider.gameObject.transform.position.x);
                int y = (int)(Mathf.Floor(hitBlock.y) - hit.collider.gameObject.transform.position.y);
                int z = (int)(Mathf.Floor(hitBlock.z) - hit.collider.gameObject.transform.position.z);

                if (Input.GetMouseButton(0))
                {
                    // remove cube
                    RemoveCube(x, y, z, chunk);
                }
                else
                {
                    // add cube
                    // check if coords are within the planet's build area
                    if (NotAtEndOfWorld(x, y, z))
                    {
                        AddCube(x, y, z, chunk);
                    }
                    else
                        Debug.Log("Unable to add a cube. We are at the end of the world!");
                }  
            }
            else
                Debug.Log("BALLS, nothing to hit!");
        }
    }
}
