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
 * Free seamless textures
 * http://www.cadhatch.com/seamless-textures/4588167680
 * 
 * Blending textures
 * https://answers.unity.com/questions/1351772/how-to-blend-two-textures.html
 * 
 * Shaders
 * https://www.google.com/search?q=unity+write+a+shader&rlz=1C1CHBD_en-GBGB888GB888&oq=unity+write+a+&aqs=chrome.2.0j69i57j0l6.5959j0j8&sourceid=chrome&ie=UTF-8#kpvalbx=_hMxTXrCWLMqd1fAPvt6p8AM32
 * http://guidohenkel.com/2013/04/a-simple-cross-fade-shader-for-unity/
 * https://www.youtube.com/watch?v=bR8DHcj6Htg&feature=youtu.be  
 * https://docs.unity3d.com/Manual/ShaderTut1.html Shader programming
 * https://answers.unity.com/questions/1108472/3-color-linear-gradient-shader.html - this might be good for gradual blend?
 * https://docs.unity3d.com/520/Documentation/Manual/SL-VertexFragmentShaderExamples.html - detailed shader stuff
 * https://docs.unity3d.com/Manual/SL-SurfaceShaderExamples.html
 * https://www.reddit.com/r/Unity3D/comments/511irt/how_to_combine_vertex_shader_with_unity_standard/d78nfzz/
 * Master Class
 * https://www.youtube.com/watch?v=9WW5-0N1DsI
 * Lots of techniques explained!!!
 * https://andreashackel.de/tech-art/stripes-shader-1/
 * Rotate texture???
 * https://forum.unity.com/threads/rotation-of-texture-uvs-directly-from-a-shader.150482/
 * 
 * Combine vertex fragment and surface
 * https://answers.unity.com/questions/1221197/can-you-specify-both-surface-and-vertexfragment-sh.html
 * https://forum.unity.com/threads/achieving-a-multi-pass-effect-with-a-surface-shader.96393/?_ga=2.216324822.1192732292.1582647687-307768343.1578037165#post-628149
 * 
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
    public Material blendGrassDirtMaterial;
    public Material blendDirtGrassMaterial;
    public Material blendDirtRockMaterial;
    public Material horizontalBlendGrassDirtMaterial;
    public Material horizontalBlendDirtGrassMaterial;
    public Material diagonalBlendGrassDirtMaterial;
    public Material diagonalBlendDirtGrassMaterial;
    public Material diagonalBlendGrassDirtTopLeftMaterial;
    public Material diagonalBlendGrassDirtBottomRightMaterial;
    public Material diagonalBlendGrassDirtTopRightMaterial;
    public Material diagonalBlendSmallDirtToLargelGrassBottomLeftMaterial;
    public Material diagonalBlendDirtGrassBottomLeftMaterial;
    public Material diagonalBlendGrassToLargeDirtTopRightMaterial;
    public Material diagonalBlendGrassToLargeDirtTopLeftMaterial;
    public Material diagonalBlendGrassToLargeDirtBottomLeftMaterial;
    public Material diagonalBlendGrassToLargeDirtBottomRightMaterial;
    public Material TESTBlendMaterial;

    // grass 0, dirt 1, sand 2, rock 4 - bitflags and bitwise operations ??????
    public static int grassQuad = 0;
    public static int dirtQuad = 1;
    public static int sandQuad = 2;
    public static int rockQuad = 3;
    public static int blendDirtToGrassQuad = 4;
    public static int blendGrassToDirtQuad = 5;
    public static int horizontalBlendGrassToDirtQuad = 6;
    public static int horizontalBlendDirtToGrassQuad = 7;
    public static int diagonalBlendGrassToDirtQuad = 8;
    public static int diagonalBlendDirtToGrassQuad = 9;
    public static int diagonalBlendGrassToDirtTopLeftQuad = 10;
    public static int diagonalBlendGrassToDirtBottomRightQuad = 11;
    public static int diagonalBlendGrassToDirtTopRightQuad = 12;
    public static int diagonalBlendSmallDirtToLargelGrassBottomLeftQuad = 13;
    public static int diagonalBlendDirtGrassBottomLeftQuad = 14;
    public static int diagonalBlendGrassToLargeDirtTopRightQuad = 15;
    public static int diagonalBlendGrassToLargeDirtTopLeftQuad = 16;
    public static int diagonalBlendGrassToLargeDirtBottomLeftQuad = 17;
    public static int diagonalBlendGrassToLargeDirtBottomRightQuad = 18;
    public static int TESTBlendQuad = 50;

    // these are accessible from other classes (the above are not, but are settable in the Inspector)
    // these are assigned to in the Startup function
    public static Material grass;
    public static Material rock;
    public static Material sand;
    public static Material dirt;
    public static Material blendGrassDirt;
    public static Material blendDirtGrass;
    public static Material horizontalBlendGrassDirt;
    public static Material horizontalBlendDirtGrass;
    public static Material diagonalBlendGrassDirt;
    public static Material diagonalBlendDirtGrass;
    public static Material diagonalBlendTopLeftGrassDirt;
    public static Material diagonalBlendBottomRightGrassDirt;
    public static Material diagonalBlendTopRightGrassDirt;
    public static Material diagonalBlendBottomLeftSmallDirtToLargeGrass;
    public static Material diagonalBlendBottomLeftDirtGrass;
    public static Material diagonalBlendTopRightGrassToLargeDirt;
    public static Material diagonalBlendTopLeftGrassToLargeDirt;
    public static Material diagonalBlendBottomLeftGrassToLargeDirt;
    public static Material diagonalBlendBottomRightGrassToLargeDirt;
    public static Material blendDirtRock;
    public static Material TESTBlend;

    public static int worldSize = 1; // # of chunks in the world
    public static int chunkSize = 20;    // dimensions of a chunk 4x4x4 quads
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
        foreach (KeyValuePair<string, Chunk> c in chunks)
        {
            c.Value.DrawChunk(); // draw the entire chunk
            yield return null;
        }

        // Pass over the terrain, working out where to apply gradual blending
        // Blend the world
        // go through the chunks, seeing which quads need to have a texture applied
        foreach (KeyValuePair<string, Chunk> chunk in chunks)
        {
            chunk.Value.BlendTheQuads();
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
        blendGrassDirt = blendGrassDirtMaterial;
        blendDirtGrass = blendDirtGrassMaterial;
        blendDirtRock = blendDirtRockMaterial;
        horizontalBlendGrassDirt = horizontalBlendGrassDirtMaterial;
        horizontalBlendDirtGrass = horizontalBlendDirtGrassMaterial;
        diagonalBlendGrassDirt = diagonalBlendGrassDirtMaterial;
        diagonalBlendDirtGrass = diagonalBlendDirtGrassMaterial;
        diagonalBlendTopLeftGrassDirt = diagonalBlendGrassDirtTopLeftMaterial;
        diagonalBlendBottomRightGrassDirt = diagonalBlendGrassDirtBottomRightMaterial;
        diagonalBlendTopRightGrassDirt = diagonalBlendGrassDirtTopRightMaterial;
        diagonalBlendBottomLeftSmallDirtToLargeGrass = diagonalBlendSmallDirtToLargelGrassBottomLeftMaterial;
        diagonalBlendBottomLeftDirtGrass = diagonalBlendDirtGrassBottomLeftMaterial;
        diagonalBlendTopRightGrassToLargeDirt = diagonalBlendGrassToLargeDirtTopRightMaterial;
        diagonalBlendTopLeftGrassToLargeDirt = diagonalBlendGrassToLargeDirtTopLeftMaterial;
        diagonalBlendBottomLeftGrassToLargeDirt = diagonalBlendGrassToLargeDirtBottomLeftMaterial;
        diagonalBlendBottomRightGrassToLargeDirt = diagonalBlendGrassToLargeDirtBottomRightMaterial;
        TESTBlend = TESTBlendMaterial;

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
