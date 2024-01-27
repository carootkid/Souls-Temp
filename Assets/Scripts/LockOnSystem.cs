using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnSystem : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraTransform;
    public float lockOnRange = 10f;
    public LayerMask enemyLayer;
    public KeyCode lockOnKey = KeyCode.L;
    public float rotationSpeed = 5f;
    public float cameraRotationSpeed = 5f;
    public float raycastMaxDistance = 100f;

    private Transform currentTarget;
    private bool isLockedOn = false;

    void Update()
    {
        if (Input.GetKeyDown(lockOnKey))
        {
            ToggleLockOn();
        }

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
            Transform closestEnemy = GetClosestEnemy(enemies);

            if (IsTargetVisible(closestEnemy))
            {
                currentTarget = closestEnemy;
                isLockedOn = !isLockedOn;
            }
        }
        else
        {
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

        Vector3 direction = (target.position - playerTransform.position).normalized;
        Ray ray = new Ray(playerTransform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastMaxDistance))
        {
            if (hit.transform == target)
            {
                return true;
            }
        }

        return false;
    }

    void FaceTarget()
    {
        Vector3 direction = (currentTarget.position - playerTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void UpdateCamera()
    {
        if (isLockedOn)
        {
            Vector3 direction = (currentTarget.position - cameraTransform.position).normalized;
            Quaternion cameraLookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraLookRotation, Time.deltaTime * cameraRotationSpeed);
        }
    }
}