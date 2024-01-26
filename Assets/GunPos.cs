using UnityEngine;

public class GunPos : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Transform orientation;
    public float lerpSpeed;

    private Quaternion targetRotation;

    void Update()
    {
        float rotationY = 0f;
        int keysPressed = 0;
        if(playerMovement.aiming == false){

            if (Input.GetKey(playerMovement.forward))
            {
                rotationY += orientation.eulerAngles.y;
                keysPressed++;
            }

            if (Input.GetKey(playerMovement.right))
            {
                rotationY += orientation.eulerAngles.y + 90f;
                keysPressed++;
            }

            if (Input.GetKey(playerMovement.left))
            {
                rotationY += orientation.eulerAngles.y - 90f;
                keysPressed++;
            }

            if (Input.GetKey(playerMovement.backward))
            {
                rotationY += orientation.eulerAngles.y + 180f;
                keysPressed++;
            }

            if (keysPressed > 0)
            {
                float averageRotationY = rotationY / keysPressed;
                targetRotation = Quaternion.Euler(0f, averageRotationY, 0f);
                
            }
        } else{
            targetRotation = orientation.rotation;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
    }
}
