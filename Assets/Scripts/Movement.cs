using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float lookSpeedH;

    [SerializeField]
    private float lookSpeedV;

    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private float dragSpeed;

    private float yaw = 0f;
    private float pitch = 0f;

    Rigidbody rb;

    void Start()
    {
        // Initialize the correct initial rotation
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Look around with right Mouse
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeedH * Input.GetAxis("Mouse X");
            pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

            //transform.eulerAngles = new Vector3(0f, yaw, 0f);
        }

        //Go Forward
        if (Input.GetKey("w")) 
        {
            transform.Translate(0f, 0f, 0.1f * zoomSpeed * Time.deltaTime, Space.Self);
        }
        //Go Backwards
        if (Input.GetKey("s"))
        {
            transform.Translate(0f, 0f, 0.1f * -zoomSpeed * Time.deltaTime, Space.Self);
        }
        //Turn Left
        if (Input.GetKey("a"))
        {
            transform.eulerAngles = transform.eulerAngles + new Vector3(0f, -dragSpeed, 0f);
        }
        //Turn Right
        if (Input.GetKey("d"))
        {
            transform.eulerAngles = transform.eulerAngles + new Vector3(0f, dragSpeed, 0f);
        }

    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("Pickup")) 
        {
            Destroy(coll.gameObject);
        }
    }
}
