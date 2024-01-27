using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    private int MaxStamina = 100;
    public int currentStamina = 100;
    public int sprintStaminaCost = 5;
    public int rollStaminaCost = 25;

    public Scrollbar StaminaBar;
    private bool canWalkAnimation = true;

    private void Start()
    {
        tempSpeed = speed;
        halfSpeed = speed / 1.5f;

        normalFov = playerCam.fieldOfView;
        halfFov = playerCam.fieldOfView / 2;

        StaminaBar.size = 1f; // Initialize StaminaBar to full
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

        if (Input.GetKeyDown(rollKey) && !isRolling && currentStamina >= rollStaminaCost)
        {
            StartCoroutine(Roll());
            currentStamina -= rollStaminaCost;  // Consume stamina for rolling
        }

        if (Input.GetKey(forward))
        {
            isForward = true;
        }
        else
        {
            isForward = false;
        }

        if (Input.GetKey(right))
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }

        if (Input.GetKey(left))
        {
            isLeft = true;
        }
        else
        {
            isLeft = false;
        }

        if (Input.GetKey(backward))
        {
            isBackward = true;
        }
        else
        {
            isBackward = false;
        }

        // Only allow sprinting if not already sprinting and enough stamina is available
        if (Input.GetKey(sprint) && !sprinting && currentStamina >= sprintStaminaCost)
        {
            sprinting = true;
        }

        // Gradual stamina consumption during sprinting
        if (sprinting && currentStamina >= sprintStaminaCost)
        {
            float sprintStaminaConsumption = sprintStaminaCost * Time.deltaTime;
            currentStamina -= Mathf.CeilToInt(sprintStaminaConsumption);
        }

        // Stamina regeneration
        if (!sprinting)
        {
            float regenAmount = 5f; // Adjust this value based on your preference
            currentStamina += Mathf.CeilToInt(regenAmount * Time.deltaTime);
            currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina);
        }

        // Allow sprinting only when the key is held down
        if (!Input.GetKey(sprint) || currentStamina < sprintStaminaCost)
        {
            sprinting = false;
        }

        UpdateStaminaBar();
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
            if (rb.velocity.magnitude > sprintSpeed)
            {
                rb.velocity = rb.velocity.normalized * sprintSpeed;
            }
        }
        else
        {
            if (rb.velocity.magnitude > walkSpeed)
            {
                rb.velocity = rb.velocity.normalized * walkSpeed;
            }
        }

        if (canWalkAnimation)
        {
            legs.SetBool("walking", rb.velocity.magnitude > 0.4f);
        }

        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, aiming ? halfFov : normalFov, speedChange * Time.deltaTime);

        if (isForward)
        {
            rb.AddForce(orientation.forward * speed);
        }

        if (isBackward)
        {
            rb.AddForce(orientation.forward * (-speed));
        }

        if (isRight)
        {
            rb.AddForce(orientation.right * speed);
        }

        if (isLeft)
        {
            rb.AddForce(orientation.right * (-speed));
        }
    }

    void UpdateStaminaBar()
    {
        if (StaminaBar != null)
        {
            float staminaPercentage = currentStamina / (float)MaxStamina;
            StaminaBar.size = staminaPercentage;
        }
    }
}