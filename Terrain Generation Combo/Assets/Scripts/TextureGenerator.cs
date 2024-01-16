using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    /*
    * References:
    https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3 
    */


    public static Texture2D TextureFromColorMap(Color[] textureMap, int width, int height)
    {
        //Used to create colored texture map
        Texture2D texture = new Texture2D (width, height);

        //Fix border
        texture.wrapMode = TextureWrapMode.Clamp;
        //Blur fix
        texture.filterMode = FilterMode.Point;

        
        texture.SetPixels(textureMap);
        texture.Apply();
        return texture;
    }
    
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        //Used to create noise map
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Texture2D perlinTexture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //Colors values black to white depending on heihgt value.
                colorMap[j * width + i] = Color.Lerp(Color.black, Color.white, heightMap[i, j]);
            }
        }
        return TextureFromColorMap(colorMap, width, height);
    }
    
}
