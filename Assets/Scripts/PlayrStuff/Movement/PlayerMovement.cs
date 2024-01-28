using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public bool canDamage;
    private PlayerHealth playerHealth;
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
    public KeyCode reload;
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

    private float regenTimer = 0f;
    public float regenInterval = 1f;

    private float sprintTimer = 0f;
    public float sprintInterval = 1f;


    private void Start()
    {
        tempSpeed = speed;
        halfSpeed = speed / 1.5f;

        normalFov = playerCam.fieldOfView;
        halfFov = playerCam.fieldOfView / 2;

        StaminaBar.size = 1f; 
        playerHealth = GetComponent<PlayerHealth>();
        canDamage = true;
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

        if (Input.GetKeyDown(rollKey) && !isRolling && currentStamina >= rollStaminaCost &&!playerHealth.atCampfire)
        {
            StartCoroutine(Roll());
            currentStamina -= rollStaminaCost;  
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

       
        if (Input.GetKey(sprint) && !sprinting && currentStamina >= sprintStaminaCost)
        {
            sprinting = true;
        }

        if (!Input.GetKey(sprint) || currentStamina < sprintStaminaCost)
        {
            sprinting = false;
            sprintTimer = 0f;
        }

        if (!sprinting)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenInterval)
            {
                float regenAmount = 1f; 
                currentStamina += Mathf.CeilToInt(regenAmount);
                currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina);
                regenTimer = 0f;
            }
        }

        if (sprinting && currentStamina >= sprintStaminaCost)
        {
            sprintTimer += Time.deltaTime;

            if (sprintTimer >= sprintInterval)
            {
                float sprintStaminaConsumption = 1f; 
                currentStamina -= Mathf.CeilToInt(sprintStaminaConsumption);
                sprintTimer = 0f;
            }
        }

        UpdateStaminaBar();
    }

    IEnumerator Roll()
    {
        if (!isRolling && !playerHealth.atCampfire)
        {
            isRolling = true;
            canWalkAnimation = false;
            canDamage = false;
            legs.SetBool("walking", false);
            rollTimer = 0f;

            legs.SetTrigger("roll");
            
            while (rollTimer < rollDuration)
            {
                rb.AddForce(gunPos.forward * rollForce);
                rollTimer += Time.deltaTime;
                yield return null;
            }
            canDamage = true;
            isRolling = false;
            canWalkAnimation = true;
        }
    }

    void FixedUpdate()
    {
        if(!playerHealth.atCampfire){
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
                if (GetComponent<Rigidbody>().velocity.magnitude > 0.4f)
                {
                    legs.SetBool("walking", true);

                    if(sprinting){
                        legs.SetBool("running", true);
                        legs.SetBool("walking", false);
                    } else {
                        legs.SetBool("running", false);
                        legs.SetBool("walking", true);
                    }
                } else {
                    legs.SetBool("running", false);
                    legs.SetBool("walking", false);
                }
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