using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTurret : MonoBehaviour
{
    public ParticleSystem flameEmitter;
    public float flameOnTime;
    public float flameOffTime;

    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(ControlFlames());
    }

    private IEnumerator ControlFlames()
    {
        flameEmitter.Play();
        boxCollider.enabled = true;
        yield return new WaitForSeconds(flameOnTime);
        flameEmitter.Stop();
        boxCollider.enabled = false;
        yield return new WaitForSeconds(flameOffTime);
        StartCoroutine(ControlFlames());
    }
}
