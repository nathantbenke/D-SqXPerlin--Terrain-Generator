using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    /*
    * References:
    https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3 
    */

    public enum DrawMode { NoiseMap, TextureMap, MeshMap};
    public DrawMode drawMode;

    //Perlin Noise Parameters
    public int mapWidth;
    public int mapHeight;
    public float scaleFactor;

    public int octaves;
    [Range(0f, 1f)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;
    
    public TerrainType[] regions;

    public float meshHeightMulti;
    public AnimationCurve meshHeightCurve;


    //Sliders for UI
    public Slider scaleFactorSlider;
    public Slider heightSlider;
    public Slider octavesSlider;
    public Slider persistanceSlider;
    public Slider lacunaritySlider;

    public Slider offSetXSlider;
    public Slider offSetYSlider;

    void Update()
    {
        scaleFactor = scaleFactorSlider.value;
        meshHeightMulti = heightSlider.value;
        octaves = (int)octavesSlider.value;
        persistance = persistanceSlider.value;
        lacunarity = lacunaritySlider.value;
        offset.x = offSetXSlider.value;
        offset.y = offSetYSlider.value;
        
        drawMode = DrawMode.NoiseMap;
        GenerateMap();

        drawMode = DrawMode.MeshMap;
        GenerateMap();
    }



    public void GenerateMap()
    {
        float[,] noiseMap = PerlinNoise.GenerateNoiseMap(mapWidth, mapHeight,scaleFactor, octaves, persistance, lacunarity, seed, offset);


        Color[] textureMap = new Color[mapWidth * mapHeight];

        for (int i = 0; i<mapHeight; i++)
        {
            for (int j = 0; j<mapWidth; j++)
            {
                
                float currentHeight = noiseMap[j,i];
                for (int k = 0; k<regions.Length; k++)
                {
                    if (currentHeight <= regions[k].height)
                    {
                        //Texture values in noise map
                        textureMap[i*mapWidth+j] = regions[k].color;
                        break;
                    }
                }

            }
        }

        //Depending on DrawMode, will generate Noise, Texture, or mesh map
        TextureDisplay display = FindObjectOfType<TextureDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));

        } else if (drawMode == DrawMode.TextureMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(textureMap, mapWidth, mapHeight));
        } else if(drawMode == DrawMode.MeshMap)
        {
            display.DrawMesh(MeshGeneratorV2.GenerateTerrainMesh(noiseMap, meshHeightMulti, meshHeightCurve),TextureGenerator.TextureFromColorMap(textureMap,mapWidth,mapHeight));
        }

    }

    //Check for making sure that parameter values are valuable. (Used for outside of runtime)
    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }

        if (mapHeight< 1)
        {
            mapHeight = 1;
        }

        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

    }
}

//Defines region elements used for texture
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
