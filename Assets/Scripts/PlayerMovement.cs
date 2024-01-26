using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode forward;
    public KeyCode right;
    public KeyCode left;
    public KeyCode backward;
    public KeyCode jump;
    public KeyCode sprint;
    public Rigidbody rb;
    public bool isGrounded;
    public bool sprinting;
    public LayerMask ground;
    public GroundDetector groundDetectorScript;
    public Transform orientation;

    public float jumpForce = 10f;

    public float speed = 10f;
    public float walkSpeed = 10f;
    public float sprintSpeed = 20f;

    void Update()
    {
        if(groundDetectorScript.grounded && Input.GetKeyDown(jump)){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        if(Input.GetKey(forward)){
            rb.AddForce(orientation.forward * speed);
        }

        if(Input.GetKey(right)){
            rb.AddForce(orientation.right * speed);
        }

        if(Input.GetKey(left)){
            rb.AddForce(orientation.right * (speed * -1));
        }

        if(Input.GetKey(backward)){
            rb.AddForce(orientation.forward * ((speed * -1) * 0.75f));
        }

        if(Input.GetKey(sprint)){
            sprinting = true;
        }
    }

    void FixedUpdate() 
    {
        if(sprinting)
        {
            if(GetComponent<Rigidbody>().velocity.magnitude > sprintSpeed)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * sprintSpeed;
            } 
        } else {
            if(GetComponent<Rigidbody>().velocity.magnitude > walkSpeed)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * walkSpeed;
            }
        }
        
    } 
}