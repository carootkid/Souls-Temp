using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Look : MonoBehaviour
{
    float mouseX;
    float mouseY;
    public float mouseSensitivityX = 100f;
    public float mouseSensitivityY = 100f;
    public Transform rotator;
    public Transform orientation;
    public Transform player;
    public float cameraSpeed = 10f;
    float xRotation;

    void Update()

    {
        


        mouseX = Input.GetAxis("Mouse Y") * mouseSensitivityX * Time.deltaTime;

        mouseY = Input.GetAxis("Mouse X") * mouseSensitivityY * Time.deltaTime;



        xRotation -= mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);



        //transform.localRotation = Quaternion.Euler(xRotation, null, null);

        rotator.Rotate(Vector3.up * mouseY);
        rotator.Rotate(Vector3.right * mouseX);
        rotator.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);

        orientation.eulerAngles = new Vector3(0f, rotator.eulerAngles.y, 0f);

        


    }

    private void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * cameraSpeed);
    }

}