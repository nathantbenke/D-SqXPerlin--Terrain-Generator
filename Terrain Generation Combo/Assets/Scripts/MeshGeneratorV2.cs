using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MeshGeneratorV2
{

    /*
    * References:
    https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3 
    */

    public static MeshData GenerateTerrainMesh (float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve)
    {
        //heightMap is noise map
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f; 
        float topLeftZ = (height - 1) / 2f; //Postive 

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                //Calculates new adapted height map based off of height multiplier and smoothstep curve                 
                meshData.verticies[vertexIndex] = new Vector3(topLeftX + i, heightCurve.Evaluate(heightMap[i, j]) * heightMultiplier, topLeftZ - j);
                // meshData.verticies[vertexIndex] = new Vector3(topLeftX + i, heightMap[i, j] * heightMultiplier, topLeftZ - j);


                meshData.uvs[vertexIndex] = new Vector2(i / (float)width, j / (float)height);
                if (i < width - 1 && j < height - 1)
                {
                    //Defines triangles used to make up single square of mesh
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return meshData;
    }

}


public class MeshData
{

    public Vector3[] verticies;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        //Defines required mesh data
        verticies = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth-1) * (meshHeight-1) * 6];
    }

    public void AddTriangle (int a, int b, int c)
    {
        triangles[triangleIndex++] = a;
        triangles[triangleIndex++] = b;
        triangles[triangleIndex++] = c;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.triangles= triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

}