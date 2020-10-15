using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var collision = Physics2D.OverlapCircle(transform.position, 0.1f);
        if(collision != null) {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
