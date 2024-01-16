using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public static class PerlinNoise
{
    /*
     * References:
     https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3 
     */


    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scaleFactor, int octaves, float persistance, float lacunarity, int seed, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random rand = new System.Random(seed);
        
        
        //Offset Logic
        Vector2[] octaveOffset = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float xOffest = rand.Next(-100000, 100000) + offset.x;
            float yOffset = rand.Next(-100000, 100000) - offset.y;
            octaveOffset[i] = new Vector2(xOffest, yOffset);

        }

        if (scaleFactor <= 0)
            scaleFactor = 0.0001f;
        
        //Find Range
        float maxNoise = float.MinValue;
        float minNoise = float.MaxValue;


        float centerScaleWidth = mapWidth / 2f;
        float centerScaleHeight = mapHeight / 2f;



        for (int j = 0; j < mapHeight; j++)
        {
            for (int i = 0; i < mapWidth; i++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int k = 0; k < octaves; k++)
                {
                    //Iterates through each octaves and recalculates the value for the noise function
                     float x = (i-centerScaleWidth) / scaleFactor * frequency + octaveOffset[k].x * frequency;
                     float y = (j-centerScaleHeight) / scaleFactor * frequency - octaveOffset[k].y * frequency;                    
                    
                    float perlinValue = Mathf.PerlinNoise(x, y) * 2 - 1;
                    noiseMap[i, j] = perlinValue;
                    noiseHeight += perlinValue * amplitude;

                    //Decreases 
                    amplitude *= persistance;
                    
                    //Increases
                    frequency *= lacunarity;
                }
                if (noiseHeight < minNoise)
                {
                    minNoise = noiseHeight;
                } else if (noiseHeight > maxNoise)
                {
                    maxNoise = noiseHeight;
                }
                noiseMap[i, j] = noiseHeight;
            }
        }

        //Normalize Noisemap
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                //Below min = 0, above max = 1
                noiseMap[i, j] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[i,j]);

            }
        }

                return noiseMap;
    }

}
