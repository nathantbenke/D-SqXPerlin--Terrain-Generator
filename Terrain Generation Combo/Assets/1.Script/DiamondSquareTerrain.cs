using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiamondSquareTerrain : MonoBehaviour
{
    /*
     * References:
     * https://www.youtube.com/watch?v=1HV8GbFnCik
     */

    // 3 main variables to make algorithm
    public int mDivisions;
    public float mSize;
    public float mHeight;

    public TMP_InputField divisionInputField;
    public Slider divisionSlider;

    public Vector2Int divisionRange;

    public Toggle toggle;

    // Add InputField
    public void OnChangedDivisionInput(string str)
    {
        //Debug.Log(str);

        float curValue = 0;

        if (float.TryParse(str, out curValue))
        {
            if (curValue > divisionRange.y)
            {
                curValue = divisionRange.y;
            }
            else if (curValue < divisionRange.x)
            {
                curValue = divisionRange.x;
            }

            divisionSlider.value = curValue / divisionRange.y;
        }
        mDivisions = (int)curValue;

    }

    //Add Slider
    public void OnChangeDivisionSlider(float v)
    {
        Debug.Log(v);
        mDivisions = (int)(v * divisionRange.y);

        divisionInputField.text = mDivisions.ToString();
    }



    public TMP_InputField sizeInputField;
    public Slider sizeSlider;

    public Vector2Int sizeRange;

    public void OnChangedSizeInput(string str)
    {
        //Debug.Log(str);

        float curValue = 0;

        if (float.TryParse(str, out curValue))
        {
            if (curValue > sizeRange.y)
            {
                curValue = sizeRange.y;
            }
            else if (curValue < sizeRange.x)
            {
                curValue = sizeRange.x;
            }

            sizeSlider.value = curValue / sizeRange.y;
        }
        mSize = (int)curValue;

    }

    public void OnChangeSizeSlider(float v)
    {
        //Debug.Log(v);
        mSize = (int)(v * sizeRange.y);

        sizeInputField.text = mSize.ToString();
    }

    // maxHeight, minHeight values for coloring
    public float maxHeight;
    public float minHeight;

    public TMP_InputField heightInputField;
    public Slider heightSlider;

    public Vector2Int heightRange;

    public void OnChangedHeightInput(string str)
    {
        //Debug.Log(str);

        float curValue = 0;

        if (float.TryParse(str, out curValue))
        {
            if (curValue > heightRange.y)
            {
                curValue = heightRange.y;
            }
            else if (curValue < heightRange.x)
            {
                curValue = heightRange.x;
            }

            heightSlider.value = curValue / heightRange.y;
        }
        mHeight = (int)curValue;

    }

    public void OnChangeHeightSlider(float v)
    {
        //Debug.Log(v);
        mHeight = (int)(v * heightRange.y);

        heightInputField.text = mHeight.ToString();
    }

    Vector3[] mVerts;
    int mVertCount;

    private void Start()
    {
        //Initial value Setup
        divisionInputField.text = mDivisions.ToString();
        divisionSlider.value = (float)mDivisions / (float)divisionRange.y;
        sizeInputField.text = mSize.ToString();
        sizeSlider.value = (float)mSize / (float)sizeRange.y;
        heightInputField.text = mHeight.ToString();
        heightSlider.value = (float)mHeight / (float)heightRange.y;


        //CreateTerrain();
    }

    // Terrain Generation
    public void CreateTerrain()
    {

        float height = mHeight;

        maxHeight = float.MinValue;
        minHeight = float.MaxValue;

        mVertCount = (mDivisions + 1) * (mDivisions + 1);
        mVerts = new Vector3[mVertCount];
        Vector2[] uvs = new Vector2[mVertCount];
        int[] tris = new int[mDivisions * mDivisions * 6];

        float halfSize = mSize * 0.5f;
        float divisionSize = mSize / mDivisions;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Process for making two triangles in a square
        int triOffset = 0;

        for (int i = 0; i <= mDivisions; i++)
        {
            for (int j = 0; j <= mDivisions; j++)
            {
                mVerts[i * (mDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
                uvs[i * (mDivisions + 1) + j] = new Vector2((float)i / mDivisions, (float)j / mDivisions);

                if (i < mDivisions && j < mDivisions)
                {
                    int topLeft = i * (mDivisions + 1) + j;
                    int botLeft = (i + 1) * (mDivisions + 1) + j;

                    //First triangle
                    tris[triOffset] = topLeft;
                    tris[triOffset + 1] = topLeft + 1;
                    tris[triOffset + 2] = botLeft + 1;
                    //Second triangle
                    tris[triOffset + 3] = topLeft;
                    tris[triOffset + 4] = botLeft + 1;
                    tris[triOffset + 5] = botLeft;
                    triOffset += 6;
                }

            }
        }
        // Get Random Height by each Vertex
        mVerts[0].y = Random.Range(-height, height);
        mVerts[mDivisions].y = Random.Range(-height, height);
        mVerts[mVerts.Length - 1].y = Random.Range(-height, height);
        mVerts[mVerts.Length - 1 - mDivisions].y = Random.Range(-height, height);

        int iterations = (int)Mathf.Log(mDivisions, 2);
        int numSquares = 1;
        int squareSize = mDivisions;

        // iterations in row and column & execute DiamondSquare Function
        for (int i = 0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {
                    DiamondSqure(row, col, squareSize, height);

                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            height *= 0.5f; // we add this code not for too steep height generation
        }


        mesh.vertices = mVerts;
        mesh.uv = uvs;
        mesh.triangles = tris;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        MeshRenderer meshRdr = GetComponent<MeshRenderer>();
        Texture2D tex = new Texture2D(mDivisions, mDivisions);

        if (toggle.isOn)
        {
            //find maxHeight & minHeight
            for (int i = 0; i <= mDivisions; i++)
            {
                for (int j = 0; j <= mDivisions; j++)
                {
                    float y = mVerts[i * (mDivisions + 1) + j].y;

                    if (y > maxHeight)
                        maxHeight = y;

                    if (y < minHeight)
                        minHeight = y;
                }
            }

            float totalHeight = Mathf.Abs(maxHeight) + Mathf.Abs(minHeight);

            float div = totalHeight / 4;  // divided by 4

            // 4 colors are assigned to each of the four evenly divided parts
            for (int i = 0; i <= mDivisions; i++)
            {
                for (int j = 0; j <= mDivisions; j++)
                {
                    float y = mVerts[i * (mDivisions + 1) + j].y + Mathf.Abs(minHeight);

                    if (y <= div)
                    {
                        tex.SetPixel(i, j, Color.blue);
                    }
                    else if (y <= 2 * div)
                    {
                        tex.SetPixel(i, j, new Color(0.33f, 0.22f, 0.09f));   // brown
                    }
                    else if (y <= 3 * div)
                    {
                        tex.SetPixel(i, j, new Color(0.08f, 0.42f, 0.08f)); // green
                    }
                    else if (y <= totalHeight)
                    {
                        tex.SetPixel(i, j, Color.white);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < mDivisions; i++)
            {
                for (int j = 0; j < mDivisions; j++)
                {
                    tex.SetPixel(i, j, Color.white);
                }
            }
        }


        tex.Apply();
        meshRdr.material.SetTexture("_MainTex", tex);

        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
    }

    //main part of this method
    void DiamondSqure(int row, int col, int size, float offset)
    {
        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (mDivisions + 1) + col;
        int botLeft = (row + size) * (mDivisions + 1) + col;

        int mid = (int)(row + halfSize) * (mDivisions + 1) + (int)(col + halfSize);
        //Diamond Step - get a random height in the middle point of Square
        mVerts[mid].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[botLeft].y + mVerts[botLeft + size].y) * 0.25f + Random.Range(-offset, offset);
        //Square Step -  get a random height in the middle point of edges
        mVerts[topLeft + halfSize].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid - halfSize].y = (mVerts[topLeft].y + mVerts[botLeft].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid + halfSize].y = (mVerts[topLeft + size].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[botLeft + halfSize].y = (mVerts[botLeft].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
    }

}