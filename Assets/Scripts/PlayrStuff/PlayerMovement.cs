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
    public KeyCode sprint;
    public KeyCode aim;
    public KeyCode pickup;
    public KeyCode switchWeapons;
    public KeyCode shoot;
    public Rigidbody rb;
    public bool isGrounded;
    public bool sprinting;
    public bool aiming;
    public Transform orientation;
    public Transform gunPos;
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
    public float rollForce = 10f;
    public float rollDuration = 0.4f;
    private float rollTimer = 3f;
    public Animator legs;

    private bool isForward;
    private bool isBackward;
    private bool isRight;
    private bool isLeft;

    private bool canWalkAnimation = true;

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

        if (Input.GetKeyDown(rollKey) && !isRolling)
        {
            StartCoroutine(Roll());
        }

        if (Input.GetKey(forward))
        {
            isForward = true;
        } else {
            isForward = false;
        }

        if (Input.GetKey(right))
        {
            isRight = true;
        } else {
            isRight = false;
        }

        if (Input.GetKey(left))
        {
            isLeft = true;
        } else {
            isLeft = false;
        }

        if (Input.GetKey(backward))
        {
            isBackward = true;
        } else {
            isBackward = false;
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
            canWalkAnimation = false;
            legs.SetBool("walking", false);
            rollTimer = 0f;

            legs.SetTrigger("roll");

            while (rollTimer < rollDuration)
            {
           
                rb.AddForce(gunPos.forward * rollForce);

                rollTimer += Time.deltaTime;

                yield return null;
            }

            isRolling = false;
            canWalkAnimation = true;
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

        if(canWalkAnimation)
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > 0.4f)
            {
                legs.SetBool("walking", true);
            } else {
                legs.SetBool("walking", false);
            }
        } else {
            Debug.Log("FUCK YOu!!1");
        }
        

        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, aiming ? halfFov : normalFov, speedChange * Time.deltaTime);

        if(isForward){
            rb.AddForce(orientation.forward * speed);
        }

        if(isBackward){
            rb.AddForce(orientation.forward * ((speed * -1)));
        }

        if(isRight){
            rb.AddForce(orientation.right * speed);
        }

        if(isLeft){
            rb.AddForce(orientation.right * (speed * -1));
        }
    }
}