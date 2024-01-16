using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TerrainModeController : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private int dropdownSelection;

    //Diamond Square
    public GameObject DSUI;
    public GameObject DSMesh;
    public Camera mainCamera;

    //Perlin Noise
    public GameObject PerlinNoiseContainer;
    public GameObject PerlinUI;

    private CameraMovement movementScript;



    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        movementScript = mainCamera.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //0 - Diamond Square, 1 - Perlin Noise
        if (dropdownSelection == 0)
        {
            DSUI.SetActive(true);
            DSMesh.SetActive(true);
            PerlinNoiseContainer.SetActive(false);
            PerlinUI.SetActive(false);

            //Remaps camera 
            movementScript.sensitivity = 2;
            movementScript.scrollSpeed = 4;

        }
        else if (dropdownSelection == 1)
        {
            PerlinNoiseContainer.SetActive(true);
            PerlinUI.SetActive(true);
            DSUI.SetActive(false);
            DSMesh.SetActive(false);
   
            //Remaps camera sensitivity
            movementScript.sensitivity = 6;
            movementScript.scrollSpeed = 14;
        }
    }

    public void GetDropdownSelection()
    {
        //Reads in dropdown selection when made
        dropdownSelection = dropdown.value;
        Debug.Log(dropdownSelection);
    }
}

