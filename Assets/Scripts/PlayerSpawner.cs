using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    
    public void SpawnPlayer()
    {
        GameObject instance = Instantiate(player, transform.position, Quaternion.identity);
        Camera.main.GetComponent<CameraFollow>().followTarget = instance.transform;
        RemoveSpawner();
    }

    public void RemoveSpawner()
    {
        Destroy(gameObject);
    }
}
