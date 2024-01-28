// Some stupid rigidbody based movement by Dani with some modifications by ME!

using System;
using UnityEngine;

public class Forklift : MonoBehaviour {

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    
    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    public float rotationSpeed = 100f; // You can adjust the rotation speed as needed
    
    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;
    
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

   
    
    //Input
    float x, y;
    bool jumping, sprinting, crouching;
    
    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    //Steering Wheel
    public Transform SteeringWheel;
    public float rotationAngle = 45f; // Adjust the rotation angle as needed
    public float wheelSpeed = 5f; // Adjust the rotation speed as needed

    //sounds
    public AudioSource forkliftSource;
    public float idleVol = 0.5f;
    public float idlePitch = 1f;
    public float movingVol = 1f;
    public float movingPitch = 1.5f;
    public float changeSpeed = 1f;
    private bool moving;

    //wheels
    public Transform[] objectsToSpin;
    public float spinSpeed = 100f; // Adjust the spin speed as needed

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    private void FixedUpdate() {
        Movement();
    }

    private void Update() {
        MyInput();
        Look();
        SteeringWheelRotation();
        ForkliftSounds();

        float currentSpeed = rb.velocity.magnitude;

        // Assuming rb is your Rigidbody component

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);

        if (localVelocity.z > 0) {
            SpinObjects((spinSpeed * -1) * currentSpeed);
        } else if (localVelocity.z < 0) {
            SpinObjects(spinSpeed * currentSpeed);
        }

    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput() {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }

    private void Movement()
    {
        // Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        // Find actual velocity relative to where the player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        // Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        // Set max speed
        float maxSpeed = this.maxSpeed;

        // If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        // Some multipliers
        float multiplier = 1f, multiplierV = 1f, rotationMultiplier = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        if (y > 0)
        {
            rotationMultiplier = 1f;
            moving = true;
        }
        else if (y < 0)
        {
            rotationMultiplier = -1f;
            moving = true;
        }
        else
        {
            rotationMultiplier = 0f;
            moving = false;
        }

        // Movement while sliding
        if (grounded && crouching) multiplierV = 0f;

        // Apply forces to move player
        rb.AddForce(transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);

        float currentSpeed = rb.velocity.magnitude;
        float rotationSpeedWithForwardSpeed = rotationSpeed * currentSpeed;

        transform.Rotate(Vector3.up, x * rotationSpeedWithForwardSpeed * Time.deltaTime * rotationMultiplier);

        Debug.Log(y + " " + rotationMultiplier);
    }


    public void SteeringWheelRotation(){
        // Calculate the target rotation based on user input
        Quaternion targetRotation = Quaternion.Euler(45f, 0f , (x * -1) * rotationAngle);

        // Smoothly interpolate between the current rotation and the target rotation
        SteeringWheel.transform.localRotation = Quaternion.Lerp(SteeringWheel.transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void SpinObjects(float speed) {
    foreach (Transform objTransform in objectsToSpin) {
        // Spin the object around the Z-axis
        objTransform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}

    public void ForkliftSounds(){
        float targetVol;
        float targetPitch;

        if(moving){
            targetPitch = movingPitch;
            targetVol = movingVol;
        } else {
            targetPitch = idlePitch;
            targetVol = idleVol;
        }

        forkliftSource.pitch = Mathf.Lerp(forkliftSource.pitch, targetPitch, Time.deltaTime * changeSpeed);
        forkliftSource.volume = Mathf.Lerp(forkliftSource.volume, targetVol, Time.deltaTime * changeSpeed);
    }
    
    private float desiredX;
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.rotation = transform.rotation;
    }

    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping) return;

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;
    
    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other) {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() {
        grounded = false;
    }
    
}