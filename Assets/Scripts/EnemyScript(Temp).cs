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
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                if (distanceToPlayer <= detectionRange)
                {
                    if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown)
                    {
                        AttackPlayer();
                        lastAttackTime = Time.time;
                    }
                    else
                    {
                        MoveTowardsPlayer(player);
                    }
                }
            }
        }
        else
        {
            Die();
        }
    }

    void MoveTowardsPlayer(GameObject player)
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        Temphealth -= damageAmount;
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
