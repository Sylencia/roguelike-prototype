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

    private List<ContactPoint2D> contacts = new List<ContactPoint2D>();

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
            Instantiate(damageEffect, collision.GetContact(0).point, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            health -= 1;
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            Vector2 position = new Vector2(direction.x + transform.position.x, direction.y + transform.position.y);
            Instantiate(damageEffect, position, Quaternion.identity);
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
