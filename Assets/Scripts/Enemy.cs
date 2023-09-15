using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Movement variables
    public float movespeed;
    #endregion

    #region Physics components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targetting variables
    public Transform player;
    #endregion

    #region Attack variables
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObj;
    #endregion

    #region Health variables
    public float maxHealth;
    float currHealth;
    #endregion

    #region Unity functions
    private void Awake()
    {
        EnemyRB = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        Move();
    }
    #endregion

    #region Movement functions
    private void Move()
    {
        Vector2 direction = player.position - transform.position;
        EnemyRB.velocity = direction.normalized * movespeed;
    }

    #endregion


    #region Attack functions
    private void Explode()
    {
        FindObjectOfType<AudioManager>().Play("Explosion");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("Hit Player");
                Instantiate(explosionObj, transform.position, transform.rotation);
                hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);
                Destroy(this.gameObject);
            }
        }
        Destroy(this.gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Explode();
        }
    }
    #endregion

    #region health functions
    public void TakeDamage(float value)
    {
        FindObjectOfType<AudioManager>().Play("BatHurt");

        currHealth -= value;

        if (currHealth <= 0)
        {
            Die();
        }
    }
    #endregion

    private void Die()
    {
        Destroy(this.gameObject);
    }

}
