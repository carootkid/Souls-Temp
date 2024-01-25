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

    public float jumpForce = 10f;

    void Update()
    {
        if(groundDetectorScript.grounded && Input.GetKeyDown(jump)){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
