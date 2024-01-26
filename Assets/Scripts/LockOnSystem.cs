using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraTransform; // Assign your camera's transform
    public float lockOnRange = 10f;
    public LayerMask enemyLayer;
    public KeyCode lockOnKey = KeyCode.L; // Change this to your desired key
    public float rotationSpeed = 5f;
    public float cameraRotationSpeed = 5f;
    public float raycastMaxDistance = 100f;

    private Transform currentTarget;
    private bool isLockedOn = false;

    void Update()
    {
        // Check for user input to toggle lock-on
        if (Input.GetKeyDown(lockOnKey))
        {
            ToggleLockOn();
        }

        // If there's a target, face it and update the camera
        if (currentTarget != null)
        {
            FaceTarget();
            UpdateCamera();
        }
    }

    void ToggleLockOn()
    {
        Collider[] enemies = Physics.OverlapSphere(playerTransform.position, lockOnRange, enemyLayer);

        if (enemies.Length > 0)
        {
            // Find the closest enemy and set it as the current target
            Transform closestEnemy = GetClosestEnemy(enemies);

            // Check line of sight before locking on
            if (IsTargetVisible(closestEnemy))
            {
                currentTarget = closestEnemy;

                // Toggle lock-on state
                isLockedOn = !isLockedOn;
            }
        }
        else
        {
            // If no enemies in range, release the lock-on
            currentTarget = null;
            isLockedOn = false;
        }
    }

    Transform GetClosestEnemy(Collider[] enemies)
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(playerTransform.position, enemy.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    bool IsTargetVisible(Transform target)
    {
        if (target == null)
            return false;

        // Raycast from player to target to check for obstacles
        Vector3 direction = (target.position - playerTransform.position).normalized;
        Ray ray = new Ray(playerTransform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastMaxDistance))
        {
            // Check if the hit object is the target or if it's an obstacle
            if (hit.transform == target)
            {
                return true; // Target is visible
            }
        }

        return false; // Target is not visible
    }

    void FaceTarget()
    {
        // Rotate player towards the target
        Vector3 direction = (currentTarget.position - playerTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void UpdateCamera()
    {
        // Rotate camera only if locked-on
        if (isLockedOn)
        {
            Vector3 direction = (currentTarget.position - cameraTransform.position).normalized;
            Quaternion cameraLookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraLookRotation, Time.deltaTime * cameraRotationSpeed);
        }
    }
}
