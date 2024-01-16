using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public float scrollSpeed;
    public Vector2 scrollRange;

    public float sensitivity;
    Vector2 turn;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //camera rotation 
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            turn.x += Input.GetAxis("Mouse X") * sensitivity;
            turn.y += Input.GetAxis("Mouse Y") * sensitivity;

            transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
        }




            //keyboard Camera Movement
            if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position += Camera.main.transform.forward * scrollSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position -= Camera.main.transform.forward * scrollSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position += Camera.main.transform.right * scrollSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position -= Camera.main.transform.right * scrollSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        Camera.main.transform.position += Camera.main.transform.forward * scroll;


    }
}
