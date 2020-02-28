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
        else if (vertex0.y > maxTerrainHeight * 0.49)
        {
            return World.blendGrassDirt;
        }
        return World.grass;
    }

}
