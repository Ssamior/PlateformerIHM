using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float platformSpeed;
    public float distance;
    [Tooltip("True = moving horizontally / False = moving vertically")]
    public bool horizontalPlatform;

    private bool returnToPosition;
    private Vector3 positionInit;
    // Start is called before the first frame update
    void Start()
    {
        positionInit = transform.position;
        returnToPosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Deplacement
        if (horizontalPlatform)
        {
            if (returnToPosition)
            {
                transform.position = transform.position + Vector3.right * Time.deltaTime * platformSpeed;
            }
            else
            {
                transform.position = transform.position - Vector3.right * Time.deltaTime * platformSpeed;
            }
        }
        else
        {
            if (returnToPosition)
            {
                transform.position = transform.position + Vector3.up * Time.deltaTime * platformSpeed;
            }
            else
            {
                transform.position = transform.position - Vector3.up * Time.deltaTime * platformSpeed;
            }
        }

        //Inversion du sens
        if (Vector3.Distance(positionInit,transform.position) >= distance)
        {
            returnToPosition = !returnToPosition;
            positionInit = transform.position;
        }
    }
}
