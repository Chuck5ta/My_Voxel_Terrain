﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{


    /*
     * Fractal Brownian Motion
     * this function is used to generate the height (Y position in Unity) of each vertex
     * 
     * x, z the X axis and Z axis location 
     * octave is the number of curves to use/generate 
     * persistance is the amount each successive curve will get smaller by
     * 
     * return the height (Y axis)
     */
    public static float fBM(float x, float z, int octave, float persistance)
    {
        float total = 0; // used to keep the result between 0 and 1
        float frequency = 0.2f; // how close the waves are to each other
        float amplitude = 0.4f; // influences the persistance
        float maxValue = 0; // used to keep the result between 0 and 1
        for (int i = 0; i < octave; i++)
        {
            total += Mathf.PerlinNoise(x * frequency,
                                       z * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistance;
            frequency *= 2;
        }

        return total / maxValue;
    }

}
