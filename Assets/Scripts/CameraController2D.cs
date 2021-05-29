using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2D : MonoBehaviour
{

    public static CameraController2D Instance;

    public Camera Camera2D;
    public Vector3 lastPosition;
    
    [Header("Sensitivity")]
    public float zoomSpeed = 1;
    public float mouseSensitivity = 1;
    public float smoothSpeed = 2.0f;

    [Header("Zoom Settings")]
    public float ShrinkThreshold;
    public float GrowthThreshold;
    public float targetOrtho;
    public float minOrtho = 1.0f;
    public float maxOrtho = 20.0f;
    public Vector3 ShrinkFactor;
    public Vector3 GrowthFactor;


    public bool HasShrunk;
    public float lastTargetOrtho;
    void Start()
    {
        Instance = this;
        Camera2D = GetComponent<Camera>();
        targetOrtho = Camera2D.orthographicSize;
    }

    void Update()
    {

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {   
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }
        Camera2D.orthographicSize = Mathf.MoveTowards(Camera2D.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);


        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            transform.Translate(-delta.x * mouseSensitivity, -delta.y * mouseSensitivity, 0);
            lastPosition = Input.mousePosition;
        }

        if(Camera2D.orthographicSize < ShrinkThreshold && !HasShrunk)
        {
            DataPointSpawner.Instance.SpawnedDatapoints.ForEach(x => x.transform.localScale = Vector3.Scale(x.transform.localScale, ShrinkFactor));
            HasShrunk = true;
        }
        if (targetOrtho > Camera2D.orthographicSize && DataPointSpawner.Instance.DataPointPrefab.transform.localScale.x >= DataPointSpawner.Instance.SpawnedDatapoints[0].transform.localScale.x)
            DataPointSpawner.Instance.SpawnedDatapoints.ForEach(x => x.transform.localScale = Vector3.Scale(x.transform.localScale, GrowthFactor));

        if (targetOrtho != Camera2D.orthographicSize)
        {
            HasShrunk = false;
        }
    }
}
