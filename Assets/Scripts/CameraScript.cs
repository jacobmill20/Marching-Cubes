using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed;
    public float zoomSpeed;

    public float leftAndRightBound;
    public float southBound;
    public float ceiling;
    public float floor;

    private float northbound;
    private Rigidbody myBody;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Faster();
        CalculateNorthBound();
        Move();
        CalculateNorthBound();
        CheckBoundaries();
    }

    private void Faster()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= 2;
            zoomSpeed *= 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= 2;
            zoomSpeed /= 2;
        }
    }

    private void Move()
    {
        if(Input.GetAxisRaw("Horizontal") > 0 && transform.position.x < leftAndRightBound)
        {
            transform.Translate(Vector3.right * Time.unscaledDeltaTime * speed);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && transform.position.x > -leftAndRightBound)
        {
            transform.Translate(Vector3.left * Time.unscaledDeltaTime * speed);
        }

        if (Input.GetAxisRaw("Vertical") > 0 && transform.position.z < northbound)
        {
            transform.Translate(Vector3.forward * Time.unscaledDeltaTime * speed, Space.World);
        }
        else if (Input.GetAxisRaw("Vertical") < 0 && transform.position.z > southBound)
        {
            transform.Translate(Vector3.back * Time.unscaledDeltaTime * speed, Space.World);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && transform.position.y > floor)
        {
            transform.Translate(Vector3.forward * Time.unscaledDeltaTime * zoomSpeed);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && transform.position.y < ceiling)
        {
            transform.Translate(Vector3.back * Time.unscaledDeltaTime * zoomSpeed);
        }
    }

    private void CalculateNorthBound()
    {
        northbound = (-10f / 9f) * transform.position.y + 136f + (1f / 9f);
    }

    private void CheckBoundaries()
    {
        if (transform.position.z < southBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, southBound);
        }
        if (transform.position.z > northbound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, northbound);
        }
    }
}
