using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMaterials : MonoBehaviour
{
    //    [Header("Materials")]
    //    [Tooltip("Any material can go here")] 
    // Base materials
    [Header("Base Materials")]
    [SerializeField] Material quad;
    [SerializeField] Material grass;
    [SerializeField] Material rock;
    [SerializeField] Material sand;
    [SerializeField] Material dirt;

    // Blending materials
    // GRASS TO DIRT
    // =============
    [Header("Grass to Dirt Blending Materials")]
    [SerializeField] Material vertBlendDirtToGrass;
    // vertical blends
    [SerializeField] Material vertBlendGrassToDirt;
    [SerializeField] Material vertBlendDirtToRock;
    // horizontal blends
    [SerializeField] Material horizBlendGrassToDirt;
    [SerializeField] Material horizBlendDirtToGrass;
    // diagonal blends - grass to dirt
    // grass to small dirt
    [SerializeField] Material diagBlendGrassToSmallDirtTopRight;
    [SerializeField] Material diagBlendGrassToSmallDirtTopLeft;
    [SerializeField] Material diagBlendGrassToSmallDirtBottomRight;
    [SerializeField] Material diagBlendGrassToSmallDirtBottomLeft;
    // grass to large dirt
    [SerializeField] Material diagBlendGrassToLargeDirtTopRight;
    [SerializeField] Material diagBlendGrassToLargeDirtTopLeft;
    [SerializeField] Material diagBlendGrassToLargeDirtBottomRight;
    [SerializeField] Material diagBlendGrassToLargeDirtBottomLeft;
    [SerializeField] Material TESTBlend;

    // grass 0, dirt 1, sand 2, rock 4 - bitflags and bitwise operations ??????
    public static int grassQuad = 0;
    public static int dirtQuad = 1;
    public static int sandQuad = 2;
    public static int rockQuad = 3;
    // vertical blends
    public static int vertBlendDirtToGrassQuad = 4;
    public static int vertBlendGrassToDirtQuad = 5;
    // horizontal blends
    public static int horizBlendGrassToDirtQuad = 6;
    public static int horizBlendDirtToGrassQuad = 7;
    // diagonal blends - grass to dirt
    // grass to small dirt
    public static int diagBlendGrassToSmallDirtTopRightQuad = 8;
    public static int diagBlendGrassToSmallDirtTopLeftQuad = 9;
    public static int diagBlendGrassToSmallDirtBottomRightQuad = 10;
    public static int diagBlendGrassToSmallDirtBottomLeftQuad = 11;
    // small grass to large dirt section
    public static int diagBlendGrassToLargeDirtTopRightQuad = 12;
    public static int diagBlendGrassToLargeDirtTopLeftQuad = 13;
    public static int diagBlendGrassToLargeDirtBottomRightQuad = 14;
    public static int diagBlendGrassToLargeDirtBottomLeftQuad = 15;
    public static int TESTBlendQuad = 50;

    // Required so that the material set in the inspector is accessible from other classes
    private static Material[] material;

    public static Material RetrieveMaterial(int materialIndex)
    {
        return material[materialIndex];
    }

    private void InitiliseMaterials()
    {
        material = new Material[16];
        // Basic materials
        material[grassQuad] = grass;
        material[dirtQuad] = dirt;
        material[sandQuad] = sand;
        material[rockQuad] = rock;
        // vertical blends
        material[vertBlendDirtToGrassQuad] = vertBlendGrassToDirt;
        material[vertBlendGrassToDirtQuad] = vertBlendDirtToGrass;
        // horizontal blends
        material[horizBlendGrassToDirtQuad] = horizBlendGrassToDirt;
        material[horizBlendDirtToGrassQuad] = horizBlendDirtToGrass;

        // grass to small dirt
        material[diagBlendGrassToSmallDirtTopLeftQuad] = diagBlendGrassToSmallDirtTopLeft;
        material[diagBlendGrassToSmallDirtTopRightQuad] = diagBlendGrassToSmallDirtTopRight;
        material[diagBlendGrassToSmallDirtBottomRightQuad] = diagBlendGrassToSmallDirtBottomRight;
        material[diagBlendGrassToSmallDirtBottomLeftQuad] = diagBlendGrassToSmallDirtBottomLeft;

        // grass to large dirt
        material[diagBlendGrassToLargeDirtTopRightQuad] = diagBlendGrassToLargeDirtTopRight;
        material[diagBlendGrassToLargeDirtTopLeftQuad] = diagBlendGrassToLargeDirtTopLeft;
        material[diagBlendGrassToLargeDirtBottomRightQuad] = diagBlendGrassToLargeDirtBottomRight;
        material[diagBlendGrassToLargeDirtBottomLeftQuad] = diagBlendGrassToLargeDirtBottomLeft;   
    }


    private void Awake()
    {
        InitiliseMaterials();
    }


    // Start is called before the first frame update
    void Start()
    {
    }
}
