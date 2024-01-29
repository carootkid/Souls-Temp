using UnityEngine;

public class bullets : MonoBehaviour
{
    public LayerMask enemiesLayerMask;
    public GameObject hitEffectEnemies;

    public int attackDamage = 42;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Hit Collider");
        // Check if the collided GameObject is on the "enemies" layer
        if (((1 << other.layer) & enemiesLayerMask) != 0)
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
