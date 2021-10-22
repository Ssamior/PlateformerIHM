using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpParticle : MonoBehaviour
{
    public float particleLifetime;
    private Color col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        col.a -= Time.deltaTime / particleLifetime;
        transform.localScale = new Vector3(transform.localScale.x + 2 * Time.deltaTime / particleLifetime, transform.localScale.y, transform.localScale.z);
        GetComponent<SpriteRenderer>().color = col;
        if (GetComponent<SpriteRenderer>().color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
