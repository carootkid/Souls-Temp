using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode rollKey;
    public KeyCode forward;
    public KeyCode right;
    public KeyCode left;
    public KeyCode backward;
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode aim;
    public KeyCode pickup;
    public KeyCode switchWeapons;
    public KeyCode shoot;
    public Rigidbody rb;
    public bool isGrounded;
    public bool sprinting;
    public bool aiming;
    public LayerMask ground;
    public GroundDetector groundDetectorScript;
    public Transform orientation;

    public float jumpForce = 10f;
    public float speed = 10f;
    public float walkSpeed = 10f;
    public float sprintSpeed = 20f;

    private float tempSpeed;
    private float halfSpeed;
    private float targetSpeed;
    private float speedChange = 15f;

    public Camera playerCam;

    private float normalFov;
    private float halfFov;

    public bool isRolling = false;
    private float rollForce = 10f;
    private float rollDuration = 0.4f;
    private float rollTimer = 3f;

    private void Start()
    {
        tempSpeed = speed;
        halfSpeed = speed / 1.5f;

        normalFov = playerCam.fieldOfView;
        halfFov = playerCam.fieldOfView / 2;
    }

    void Update()
    {
        if (Input.GetKey(aim))
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }

        if (aiming)
        {
            targetSpeed = halfSpeed;
        }
        else
        {
            targetSpeed = tempSpeed;
        }

        speed = Mathf.Lerp(speed, targetSpeed, speedChange * Time.deltaTime);

        if (groundDetectorScript.grounded && Input.GetKeyDown(jump))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKey(rollKey) && !isRolling)
        {
            StartCoroutine(Roll());
        }

        if (Input.GetKey(forward))
        {
            rb.AddForce(orientation.forward * speed);
        }

        if (Input.GetKey(right))
        {
            rb.AddForce(orientation.right * speed);
        }

        if (Input.GetKey(left))
        {
            rb.AddForce(orientation.right * (speed * -1));
        }

        if (Input.GetKey(backward))
        {
            rb.AddForce(orientation.forward * ((speed * -1)));
        }

        if (Input.GetKey(sprint))
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }
    }

    IEnumerator Roll()
    {
        if (!isRolling)
        {
            isRolling = true;
            rollTimer = 0f;

       
            Vector3 rollDirection = (orientation.forward * Input.GetAxis("Vertical") + orientation.right * Input.GetAxis("Horizontal")).normalized;

            while (rollTimer < rollDuration)
            {
           
                rb.AddForce(rollDirection * rollForce);

                rollTimer += Time.deltaTime;

                yield return null;
            }

            isRolling = false;
        }
    }

    void FixedUpdate()
    {
        if (sprinting)
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > sprintSpeed)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * sprintSpeed;
            }
        }
        else
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > walkSpeed)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * walkSpeed;
            }
        }

        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, aiming ? halfFov : normalFov, speedChange * Time.deltaTime);
    }
}