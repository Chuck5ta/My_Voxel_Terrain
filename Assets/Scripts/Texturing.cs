using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texturing
{

    /*
     * Pick a material to add to the quad
     */
    public static Material SetMaterial(Vector3 vertex0, int maxTerrainHeight)
    {
        if (vertex0.y > maxTerrainHeight * 0.70)
        {
            return World.rock;
        }
        else if (vertex0.y > maxTerrainHeight * 0.50)
        {
            return World.dirt;
        }
        else if (vertex0.y > maxTerrainHeight * 0.44)
        {
            return World.topBlendGrassDirt;
        }
        else if (vertex0.y > maxTerrainHeight * 0.42)
        {
            return World.blendGrassDirt;
        }
        else if (vertex0.y > maxTerrainHeight * 0.40)
        {
            return World.bottomBlendGrassDirt;
        }
        return World.grass;
    }

}
