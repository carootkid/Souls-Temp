using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptReal : MonoBehaviour
{
    public int Temphealth = 50;
    public float moveSpeed = 5f;
    public float detectionRange = 10f;
    public float attackRange = 3f;
    public int damageAmount = 10;
    public float attackCooldown = 2f;

    public HurtPlayer hurtPlayer;

    private GameObject player;
    private float lastAttackTime;
    public LevelUpScript levelUpScript;
    public Animator enemyAnimator;
    public Collider attackCollider;

    public bool alreadyHit = false;

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
            levelUpScript.playerEXP = levelUpScript.playerEXP + 50;
        }
    }

    void MoveTowardsPlayer(GameObject player, float distanceToPlayer)
        {
            if (distanceToPlayer > attackRange)
            {
                enemyAnimator.SetBool("idle", false);
                Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                Vector3 direction = (playerPosition - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
                transform.LookAt(playerPosition);
            } else {
                enemyAnimator.SetBool("idle", true);
            }
        }

    void StopMoving()
    {
        enemyAnimator.SetBool("idle", true);
    }

    void AttackPlayer()
    {
        enemyAnimator.SetTrigger("attack");
    }

    public void TakeDamage(int damageAmount)
    {
        Temphealth -= damageAmount;
        Debug.Log("Took Damage!");
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void EnableCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableCollider()
    {
        attackCollider.enabled = true;
        alreadyHit = false;
    }
}
