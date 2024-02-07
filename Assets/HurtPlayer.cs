using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public LayerMask playerLayer;
    
    public GameObject hitEffectPlayer;
    
    public int attackDamage = 25;

    public EnemyScriptReal enemyScriptReal;
    void OnTriggerEnter(Collider other)
    {
        if (enemyScriptReal.alreadyHit == false && ((1 << other.gameObject.layer) & playerLayer) != 0)
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

            SpawnHitEffect(hitPoint, hitNormal, hitEffectPlayer);
            
            other.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);

            Debug.Log("Hit Player");
            enemyScriptReal.alreadyHit = true;
            
        }

    }

    private void SpawnHitEffect(Vector3 position, Vector3 normal, GameObject effectPrefab)
    {
        Quaternion rotation = Quaternion.LookRotation(normal);

        Instantiate(effectPrefab, position, rotation);
    }
}
