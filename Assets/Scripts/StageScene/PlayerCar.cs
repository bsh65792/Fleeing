using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    private Rigidbody rigidbody;
    public GameObject realCar;
    private float limitVelocity = 1.8f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Mathf.Sqrt(rigidbody.velocity.x * rigidbody.velocity.x + 
                                 rigidbody.velocity.y * rigidbody.velocity.y +
                                 rigidbody.velocity.z * rigidbody.velocity.z);
        
        if (speed > 0f)
        {
            float rotationY;
            if (rigidbody.velocity.z > 0f)
            {
                rotationY = 180f + Mathf.Asin(rigidbody.velocity.x / speed) * 180 / Mathf.PI;
            }
            else if(rigidbody.velocity.x > 0f)
            {
                rotationY = 180f + 90f - Mathf.Asin(rigidbody.velocity.z / speed) * 180 / Mathf.PI;
            }
            else
            {
                rotationY = -Mathf.Asin(rigidbody.velocity.x / speed) * 180 / Mathf.PI;
            }
            realCar.transform.rotation = Quaternion.Euler(new Vector3(0f, rotationY, 0f));
            //Debug.Log(rotationY);
        }
        

        
        if (speed > limitVelocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x / speed * limitVelocity,
                rigidbody.velocity.y / speed * limitVelocity,
                rigidbody.velocity.z / speed * limitVelocity);
        }
        //Debug.Log("PlayerCar Speed : " + speed);
    }


    private void FixedUpdate()
    {
        float forceSize = 1;
        float decreaseSize = 0.2f;
        float xForce = 0f;
        float zForce = 0f;
        
        if (Input.GetKey(KeyCode.LeftArrow) == true || Input.GetKey(KeyCode.A) == true)
        {
            xForce -= forceSize;
        }
        if (Input.GetKey(KeyCode.RightArrow) == true || Input.GetKey(KeyCode.D) == true)
        {
            xForce += forceSize;
        }
        if (Input.GetKey(KeyCode.UpArrow) == true || Input.GetKey(KeyCode.W) == true)
        {
            zForce += forceSize;
        }
        if (Input.GetKey(KeyCode.DownArrow) == true || Input.GetKey(KeyCode.S) == true)
        {
            zForce -= forceSize;
        }

        float speed = Mathf.Sqrt(xForce * xForce + zForce * zForce);
        if (speed <= 0f)
        {
            rigidbody.AddForce(new Vector3(-rigidbody.velocity.x * decreaseSize,
                                            -rigidbody.velocity.y * decreaseSize,
                                            -rigidbody.velocity.z * decreaseSize), ForceMode.Impulse);
            return;
        }
        
        rigidbody.AddForce(new Vector3(xForce / speed * 0.1f, 0f, zForce / speed * 0.1f), ForceMode.Impulse);
        //Debug.Log("xForce : " + xForce + ", zForce : " + zForce);
    }


}
