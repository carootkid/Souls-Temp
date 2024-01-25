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
    public Rigidbody rb;
    public bool isGrounded;
    public LayerMask ground;
    public GroundDetector groundDetectorScript;
    public Transform orientation;

    public float jumpForce = 10f;

    public float walkSpeed = 10f;

    void Update()
    {
        if(groundDetectorScript.grounded && Input.GetKeyDown(jump)){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        if(Input.GetKey(forward)){
            rb.AddForce(orientation.forward * walkSpeed);
        }

        if(Input.GetKey(right)){
            rb.AddForce(orientation.right * walkSpeed);
        }

        if(Input.GetKey(left)){
            rb.AddForce(orientation.right * (walkSpeed * -1));
        }

        if(Input.GetKey(backward)){
            rb.AddForce(orientation.forward * (walkSpeed * -1));
        }
    }
}
