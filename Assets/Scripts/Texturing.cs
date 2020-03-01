using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texturing
{

    /*
     * Pick a material to add to the quad
     */
    public static Material SetMaterial(Vector3 vertex0, int maxTerrainHeight, out int terrainType)
    {
        if (vertex0.y > maxTerrainHeight * 0.70)
        {
            terrainType = World.rockQuad;
            return World.rock;
        }
        else if (vertex0.y > maxTerrainHeight * 0.50)
        {
            terrainType = World.dirtQuad;
            return World.dirt;
        }
        terrainType = World.grassQuad;
        return World.grass;
    }

    // function to apply blending????
    // blending, vegetaton, paths/roads, volcanos, water, caves, towns/villages????
  //  public static void PostWorldCreationActions(Dictionary<string, Chunk> chunks)
 //   {
        // Blend the world
        // go through the chunks, seeing which quads need to have a texture applied

 //       foreach (KeyValuePair<string, Chunk> chunk in chunks)
 //       {
 //           chunk.Value.chunk.
 //       }
 //   }

}
