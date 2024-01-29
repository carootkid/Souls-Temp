using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMover : MonoBehaviour
{
    public Transform targetObject;

    public Camera mainCamera;

    public LayerMask playerLayer;
    void Update()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f,  ~playerLayer))
        {
            targetObject.transform.position = hit.point;
        } else {
            targetObject.transform.position = ray.origin + ray.direction * 100;
        }
    }
}
