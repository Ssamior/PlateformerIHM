using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float particleLifetime;
    private Color col;
    // Start is called before the first frame update
    void Start()
    {
        col = transform.gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        col.a -= Time.deltaTime / particleLifetime;
        transform.gameObject.GetComponent<SpriteRenderer>().color = col;
        if (transform.gameObject.GetComponent<SpriteRenderer>().color.a <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
