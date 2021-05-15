using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(pos.x, pos.y, pos.z + speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(pos.x, pos.y, pos.z - speed * Time.deltaTime);
        }
    }
}
