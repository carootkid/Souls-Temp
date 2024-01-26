using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform aim;

    private void Update() {
        transform.LookAt(aim);
    }
}
