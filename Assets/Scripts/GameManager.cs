using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public float x;
    public float y;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Player", 1.0f);
    }

    public void KillPlayer(GameObject player)
    {
        Destroy(player);
        Invoke("Player", 1.0f);
    }

    void Player()
    {
        Instantiate(player, new Vector3(x, y, 0), Quaternion.identity);
    }
}
