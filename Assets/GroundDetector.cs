using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool grounded;
    private void OnTriggerEnter(Collider other) {
        grounded = true;
    }

    private void OnTriggerStay(Collider other) {
        grounded = true;
    }

    private void OnTriggerExit(Collider other) {
        grounded = false;
    }
}
