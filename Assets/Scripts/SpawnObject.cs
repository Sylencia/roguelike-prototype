using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objectPool;
    public bool allowRandomRotation;

    // Start is called before the first frame update
    void Start()
    {
        var idx = Random.Range(0, objectPool.Length);
        var rotation = allowRandomRotation ? Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 4) * 90f)) : Quaternion.identity;
        var instance = Instantiate(objectPool[idx], transform.position, rotation);
        instance.transform.parent = gameObject.transform.parent;
        Destroy(gameObject);
    }
}
