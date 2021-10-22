using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintParticle : MonoBehaviour
{
    public float particleLifetime;
    private Color col;
    private float yvelocity;
    private float xvelocity;
    private float q;
    private float direction;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SpriteRenderer>().color;
        q = Random.Range(0f, 0.5f);
        yvelocity = (1-q) * 15;
    }

    // Update is called once per frame
    void Update()
    {
        
        col.a -= Time.deltaTime / particleLifetime;
        GetComponent<SpriteRenderer>().color = col;

        yvelocity -= 50 * Time.deltaTime;
        xvelocity = q * direction * yvelocity;

        transform.Translate(new Vector3(xvelocity, yvelocity, 0) * Time.deltaTime);


        if (GetComponent<SpriteRenderer>().color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void setDirection(float dir)
    {
        direction = dir;
    }
}
