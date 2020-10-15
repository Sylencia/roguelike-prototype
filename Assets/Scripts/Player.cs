using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth;
    public Vector2 spawnPosition;
    private int health;
    public GameObject deathEffect;
    public GameObject damageEffect;

    private void Start()
    {
        health = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle") {
            health -= 1;
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            GetComponent<PlayerController>().OnHitCallback(new Vector2(direction.x, direction.y));
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }
    }

    private void Reset()
    {
        health = maxHealth;
        transform.position = spawnPosition;
    }

    private void Update()
    {
        if(health <= 0) {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            //Destroy(gameObject);
            Reset();
        }
    }
}
