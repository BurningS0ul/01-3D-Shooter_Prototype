using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy;
    [HideInInspector] public WeaponManager weapon;
    [HideInInspector] public Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(weapon.damage);

            if (enemyHealth.currentHealth <= 0 && enemyHealth.isDead == false)
            {
                Rigidbody bulletRb = collision.gameObject.GetComponent<Rigidbody>();
                bulletRb.AddForce(dir * weapon.enemyKickbackForce, ForceMode.Impulse);
            }
        }
        Destroy(this.gameObject);
    }
}
