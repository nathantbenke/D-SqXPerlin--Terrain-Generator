using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureDisplay : MonoBehaviour
{
    /*
    * References:
    https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3 
    */


    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture)
    {
        //PreviewMap - Used for texturing plane.
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(5, 5, 5);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        //Used for texturing mesh
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }

}
