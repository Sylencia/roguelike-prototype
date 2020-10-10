using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public enum OPENING_DIRECTIONS
    {
        LEFT = 1,
        BOTTOM,
        RIGHT,
        TOP
    }

    public OPENING_DIRECTIONS openingRequirement;
    public bool spawned = false;
    private RoomTemplates templates;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("SpawnRoom", 0.1f);
    }

    private void SpawnRoom()
    {
        if (!spawned) {
            if (openingRequirement == OPENING_DIRECTIONS.LEFT) {
                var rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            if (openingRequirement == OPENING_DIRECTIONS.RIGHT) {
                var rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            if (openingRequirement == OPENING_DIRECTIONS.TOP) {
                var rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            if (openingRequirement == OPENING_DIRECTIONS.BOTTOM) {
                var rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }

            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("SpawnPoint")) {
            if(!collision.GetComponent<RoomSpawner>().spawned && !spawned) {
                // close off the room
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            spawned = true;
        }
    }
}
