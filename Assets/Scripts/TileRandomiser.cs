using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRandomiser : MonoBehaviour
{
    public Sprite[] tileSprites;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        var rand = Random.Range(0, tileSprites.Length);
        sr.sprite = tileSprites[rand];
    }
}
