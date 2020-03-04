/*
 * This holds functions that are used to make the terrain look more natural by making use of shaders to
 * blend textures together.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBlending
{
    // Grass to Dirt
    // Grass to rock
    // Dirt to Rock
    // Sand to Grass
    // Sand to Dirt
    // Sand to rock

    /*
        * Make the join between grass and dirt look more natural
        */
    private static void BlendGrassToDirt(Quad[,] chunkData, int x, int z)
    {
        // check for dirt Top and Bottom
        //  Vetical Test    IF Top (PosZ) and bottom (NegZ) quads are dirt
        if (z < Universe.chunkSize - 1 && z > 0 &&
            chunkData[x, z + 1].terrainType == CustomMaterials.dirtQuad &&
            chunkData[x, z - 1].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.dirtQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad));
        }
        // check for dirt Top and Bottom
        //  horiz Test    IF Left (NegX) and right (PosX) quads are dirt
        else if (x < Universe.chunkSize - 1 && x > 0 &&
            chunkData[x - 1, z].terrainType == CustomMaterials.dirtQuad &&
            chunkData[x + 1, z].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.dirtQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.dirtQuad));
        }
        //  diag gradient blend Test    IF Left (NegX) and bottom (NegZ) quads are dirt
        else if (z > 0 && x > 0 &&
            chunkData[x - 1, z].terrainType == CustomMaterials.dirtQuad &&
            chunkData[x, z - 1].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.diagBlendGrassToLargeDirtBottomLeftQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.diagBlendGrassToLargeDirtBottomLeftQuad));
        }
        //  diag gradient blend Test    IF Top (PosZ) and right (PosX) quads are dirt
        else if (x < Universe.chunkSize - 1 && z < Universe.chunkSize - 1 &&
            chunkData[x, z + 1].terrainType == CustomMaterials.dirtQuad &&
            chunkData[x + 1, z].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.diagBlendGrassToLargeDirtTopRightQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.diagBlendGrassToLargeDirtTopRightQuad));
        }
        //  diag gradient blend Test    IF Top (PosZ) and left (NegX) quads are dirt
        else if (x < Universe.chunkSize - 1 && z < Universe.chunkSize - 1 && x > 0 &&
            chunkData[x, z + 1].terrainType == CustomMaterials.dirtQuad &&
            chunkData[x + 1, z].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.diagBlendGrassToLargeDirtTopLeftQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.diagBlendGrassToLargeDirtTopLeftQuad));
        }
        //  diag gradient blend Test    IF Bottom (NegZ) and left (NegX) quads are dirt
        else if (x > 0 && z > 0 &&
            chunkData[x, z - 1].terrainType == CustomMaterials.dirtQuad &&
            chunkData[x - 1, z].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.diagBlendGrassToLargeDirtBottomLeftQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.diagBlendGrassToLargeDirtBottomLeftQuad));
        }
        //  diag gradient blend Test    IF Bottom (NegZ) and left (PosX) quads are dirt
        else if (x < Universe.chunkSize - 1 && x > 0 && z > 0 &&
            chunkData[x, z - 1].terrainType == CustomMaterials.dirtQuad &&
            chunkData[x + 1, z].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.diagBlendGrassToLargeDirtBottomRightQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.diagBlendGrassToLargeDirtBottomRightQuad));
        }
        // Vertical gradient blending
        //     IF positiveZ quad is dirt
        else if (z < Universe.chunkSize - 1 &&
            chunkData[x, z + 1].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.vertBlendGrassToDirtQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.vertBlendGrassToDirtQuad));
        }
        //     IF negativeZ quad is dirt
        else if (z > 0 && chunkData[x, z - 1].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.vertBlendDirtToGrassQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.vertBlendDirtToGrassQuad));
        }
        // horiz gradient blending
        //     IF positiveX quad is dirt
        else if (x < Universe.chunkSize - 1 &&
            chunkData[x + 1, z].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.horizBlendGrassToDirtQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.horizBlendGrassToDirtQuad));
        }
        //     IF negativeX quad is dirt
        else if (x > 0 && chunkData[x - 1, z].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.horizBlendDirtToGrassQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.horizBlendDirtToGrassQuad));
        }
        //  diag gradient blend Test    IF Top Right quad is dirt
        else if (x < Universe.chunkSize - 1 && z < Universe.chunkSize - 1 &&
            chunkData[x + 1, z + 1].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.diagBlendGrassToSmallDirtTopRightQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.diagBlendGrassToSmallDirtTopRightQuad));
        }
        //  diag gradient blend Test    IF Top Left quad is dirt
        else if (z < Universe.chunkSize - 1 && x > 0 &&
            chunkData[x - 1, z + 1].terrainType == CustomMaterials.dirtQuad)
        {
            Debug.Log("Found dirt in the corner");
            chunkData[x, z].terrainType = CustomMaterials.diagBlendGrassToSmallDirtTopLeftQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.diagBlendGrassToSmallDirtTopLeftQuad));
        }
        //  diag gradient blend Test    IF Bottom Right quad is dirt
        else if (x < Universe.chunkSize - 1 && z > 0 &&
            chunkData[x + 1, z - 1].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.diagBlendGrassToSmallDirtBottomRightQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.diagBlendGrassToSmallDirtBottomRightQuad));
        }
        //  diag gradient blend Test    IF Bottom Left quad is dirt
        else if (z > 0 && x > 0 &&
            chunkData[x - 1, z - 1].terrainType == CustomMaterials.dirtQuad)
        {
            chunkData[x, z].terrainType = CustomMaterials.diagBlendGrassToSmallDirtBottomLeftQuad;
            chunkData[x, z].SetMaterial(CustomMaterials.RetrieveMaterial(CustomMaterials.diagBlendGrassToSmallDirtBottomLeftQuad));
        }
    }


    // blending, vegetaton, paths/roads, volcanos, water, caves, towns/villages????
    public static void GradientBlend(Quad[,] chunkData, int x, int z)
    {
        // IF Grass quad
        if (chunkData[x, z].terrainType == CustomMaterials.grassQuad)
        {
            BlendGrassToDirt(chunkData, x, z);
            // TODO BlendGrassToRock(chunkData, x, z);
            // TODO BlendGrassToSand(chunkData, x, z);
        } 
        // TODO 
        // IF Sand quad
        //    BlendSandToRock(chunkData, x, z);
        //    BlendSandToDirt(chunkData, x, z);
        // IF Rock quad
        //    BlendRockToDirtk(chunkData, x, z);
    }


}
