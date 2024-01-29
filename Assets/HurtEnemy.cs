using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    public LayerMask enemyLayer;
    
    public GameObject hitEffectEnemies;
    
    public float attackDamage = 25f;
    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Vector3 hitPoint = other.transform.position;
            Vector3 hitNormal = other.transform.forward;
            RaycastHit hitInfo;
            Debug.Log("Hit Enemy");
            
            if (other.TryGetComponent(out Collider collider))
            {
                if (collider.Raycast(new Ray(transform.position, transform.forward), out hitInfo, Mathf.Infinity))
                {
                    hitPoint = hitInfo.point;
                    hitNormal = hitInfo.normal;
                }
            }

            SpawnHitEffect(hitPoint, hitNormal, hitEffectEnemies);
            
            other.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
            
        }

    }

    private void SpawnHitEffect(Vector3 position, Vector3 normal, GameObject effectPrefab)
    {
        Quaternion rotation = Quaternion.LookRotation(normal);

        Instantiate(effectPrefab, position, rotation);
    }
}
