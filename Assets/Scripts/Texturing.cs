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
            terrainType = CustomMaterials.rockQuad; // sent back to the quad object (out)
            return CustomMaterials.RetrieveMaterial(terrainType);
        }
        else if (vertex0.y > maxTerrainHeight * 0.50)
        {
            terrainType = CustomMaterials.dirtQuad;
            return CustomMaterials.RetrieveMaterial(terrainType);
        }  
        terrainType = CustomMaterials.grassQuad;
        return CustomMaterials.RetrieveMaterial(terrainType);
    }
    
}
