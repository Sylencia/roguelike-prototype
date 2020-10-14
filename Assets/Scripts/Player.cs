using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public GameObject deathEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle") {
            health -= 1;
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            GetComponent<PlayerController>().OnHitCallback(new Vector2(direction.x, direction.y));
        }
    }

    private void Update()
    {
        if(health == 0) {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
