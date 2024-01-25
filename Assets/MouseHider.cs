using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HideMouse();
    }

    public void HideMouse(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowMouse(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
