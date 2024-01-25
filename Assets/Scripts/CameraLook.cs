using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Look : MonoBehaviour
{
    float mouseX;
    float mouseY;
    public float mouseSensitivity = 100f;
    public Transform player;
    public Transform orientation;
    float xRotation;

    void Update()

    {

        mouseX = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        mouseY = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;



        xRotation -= mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);



        //transform.localRotation = Quaternion.Euler(xRotation, null, null);

        player.Rotate((Vector3.up * mouseY) * -1);
        player.Rotate((Vector3.right * mouseX) * -1);
        player.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);

        orientation.eulerAngles = new Vector3(0f, player.eulerAngles.y, 0f);



    }

}