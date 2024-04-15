using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100;
    RagdollManager ragdollManager;
    public bool isDead;

    private void Start()
    {
        ragdollManager = GetComponent<RagdollManager>();
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                EnemyDeath();
            }
            else
            {
                Debug.Log("Hit");
            }
        }
    }

    void EnemyDeath()
    {
        ragdollManager.ActivateRagdoll();
        Debug.Log("Death");
        isDead = true;
        Destroy(gameObject, 5);
        GetComponent<EnemyStateManager>().enabled = false;
    }
}
