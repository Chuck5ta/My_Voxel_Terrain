using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * The World class controls the building of the world in chunks
 * Each chunk is made up of quads. Each quad is made up of 2 triangles.
 * 
 * 19 Feb 2020 - Multi chunk construction implemented
 * 
 * 
 * Useful links
 * ============
 * Courses
 * https://holistic3d.com/
 * https://www.gamedev.tv/
 * 
 * Empyrion terrain generation information
 * https://docs.google.com/document/d/1MCvuCMtFvnCV8UHAglEi-IhLJgcD60TzNBjVbAZCptw/edit#heading=h.ygrn9c9eg1fd
 * 
 * Blending textures
 * https://answers.unity.com/questions/1351772/how-to-blend-two-textures.html
 *       
 * Threads
 * https://www.tutorialspoint.com/csharp/csharp_multithreading.htm
 * C# Job System
 * https://docs.unity3d.com/Manual/JobSystem.html?_ga=2.217102486.185736627.1581613830-307768343.1578037165
 * 
 * 
 */

public class World : MonoBehaviour
{
    [Header("Materials")]
    [Tooltip("Any material can go here")] public Material quadMaterial;
    public Material grassMaterial;
    public Material rockMaterial;
    public Material sandMaterial;
    public Material dirtMaterial;

    // these are accessible from other classes (the above are not, but are settable in the Inspector)
    // these are assigned to in the Startup function
    public static Material grass;
    public static Material rock;
    public static Material sand;
    public static Material dirt;

    public static int worldSize = 1; // # of chunks in the world
    public static int chunkSize = 50;    // dimensions of a chunk 4x4x4 quads
    public static Dictionary<string, Chunk> chunks;

    public static string BuildChunkName(Vector3 position)
    {
        return (int)position.x + "_" +
               (int)position.z;
 //       return (int)position.x + "_" +   // leave this, as we may need to implement a cubish world, instead of the quad one we have
 //              (int)position.y + "_" +
 //              (int)position.z;
    }

    IEnumerator BuildWorld()
    {
        for (int chunkZIndex = 0; chunkZIndex < worldSize; chunkZIndex++)
        {
            for (int chunkXIndex = 0; chunkXIndex < worldSize; chunkXIndex++)
            {
                Vector3 chunkPosition = new Vector3(this.transform.position.x + (chunkXIndex * chunkSize),
                                                    chunkXIndex * chunkSize,
                                                    this.transform.position.z + (chunkZIndex * chunkSize));

                Chunk c = new Chunk(chunkZIndex, chunkXIndex, chunkPosition); // CHANGE THIS!!! include parameter stating biome (desert, jungle, etc.)
                c.chunk.transform.parent = this.transform;
                chunks.Add(c.chunk.name, c);
            }
        }
        int index = 0;
        foreach (KeyValuePair<string, Chunk> c in chunks)
        {
            index++;
            c.Value.DrawChunk(); // draw the entire chunk
            yield return null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // replace this with a sprite atlas
        grass = grassMaterial;
        rock = rockMaterial;
        sand = sandMaterial;
        dirt = dirtMaterial;

        chunks = new Dictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        StartCoroutine(BuildWorld());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
