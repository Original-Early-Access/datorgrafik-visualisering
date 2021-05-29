using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController3D : MonoBehaviour
{
    public float ZoomSpeed;
    public float mouseX;
    public float mouseY;
    public float sensitivity;
    public Camera Camera3D;

    public float RotateSensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;
    void Start()
    {
        Camera3D = GetComponent<Camera>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(transform.forward * scroll * ZoomSpeed, Space.World);

        Panning();
        Rotate();
    }

    public void Rotate()
    {
        if (Input.GetMouseButton(2))
        {
            currentRotation.x += Input.GetAxis("Mouse X") * RotateSensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * RotateSensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }
    }

    public void Panning()
    {
        if (Input.GetMouseButton(0))
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            transform.position += transform.right * (mouseX * -1) * sensitivity;
            transform.position += transform.up * (mouseY * -1) * sensitivity;
        }
    }
}
