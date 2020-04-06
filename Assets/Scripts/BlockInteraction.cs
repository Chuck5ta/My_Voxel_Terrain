using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("************ Mouse button pressed ***************");
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.black);
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
            {
                Vector3 hitBlock = hit.point - hit.normal / 2.0f;

                Debug.Log("Hit point: " + hit.point);

                int x = (int)(Mathf.Floor (hitBlock.x) - hit.collider.gameObject.transform.position.x);
                int y = (int)(Mathf.Floor(hitBlock.y) - hit.collider.gameObject.transform.position.y);
                int z = (int)(Mathf.Floor(hitBlock.z) - hit.collider.gameObject.transform.position.z);

                Debug.Log("X Y Z : " + x + "," + y + "," + z);

                foreach (KeyValuePair<string, PlanetChunk> c in Universe.planet.planetChunks)
                {
                    Debug.Log("Planet chunk is : " + c.Key);
                }
                PlanetChunk chunk; //retrieve the chunk
                Debug.Log("Hit object's name : " + hit.collider.gameObject.name);
                if (Universe.planet.planetChunks.TryGetValue(hit.collider.gameObject.name, out chunk)) //TODO: ?????????????????
                {
                    Debug.Log("DESTROY BLOCK : " + hitBlock);
                    DestroyImmediate(chunk.planetChunk.GetComponent<MeshFilter>());
                    DestroyImmediate(chunk.planetChunk.GetComponent<MeshRenderer>());
                    DestroyImmediate(chunk.planetChunk.GetComponent<Collider>());
                    // TODO: NEED TO KEEP THE CHANGED STATE!!!!
                    chunk.ReBuildTheChunk();
                    chunk.CubeIsSolid[x, y, z] = false; // stop the cube from being drawn
                    chunk.DrawChunk();
                }
                else
                    Debug.Log("Failed to find: " + hit.collider.gameObject.name); // TODO: name NOT being saved to the Dictionary????
            }
        }
    }
}
