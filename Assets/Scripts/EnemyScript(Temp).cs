using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    public int Temphealth = 50;
    public float moveSpeed = 5f;
    public float detectionRange = 10f;
    public float attackRange = 3f;
    public int damageAmount = 10;
    public float attackCooldown = 2f;

    private GameObject player;
    private float lastAttackTime;

    void Start()
    {
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        if (Temphealth > 0)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                new Vector3(player.transform.position.x, 0f, player.transform.position.z));

                if (distanceToPlayer <= detectionRange)
                {
                    if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown)
                    {
                        AttackPlayer();
                        lastAttackTime = Time.time;
                    }
                    else
                    {
                        MoveTowardsPlayer(player, distanceToPlayer);
                    }
                }
                else
                {
                    StopMoving();
                }
            }
        }
        else
        {
            Die();
        }
    }

    void MoveTowardsPlayer(GameObject player, float distanceToPlayer)
        {
            if (distanceToPlayer > attackRange)
            {
                Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                Vector3 direction = (playerPosition - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
                transform.LookAt(playerPosition);
            }
        }

    void StopMoving()
    {
        // Customize this function for idle or other non-movement behavior
    }

    void AttackPlayer()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        Temphealth -= damageAmount;
        Debug.Log("Took Damage!");
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
