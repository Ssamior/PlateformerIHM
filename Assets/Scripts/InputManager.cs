using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float acceleration;
    public float verticalacceleration;
    public float jumpHeight;

    private Vector3 jumpStartPosition;
    private GameObject ground;
    private bool isJumpingUp;
    private Vector3 moveDirection;
    private Vector3 positionInit;
    private Vector3 positionFin;
    // Start is called before the first frame update
    void Start()
    {
        positionInit = transform.position;
        positionFin = positionInit;
        ground = GameObject.Find("Ground");
        jumpStartPosition = positionInit;
    }


    /*void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
    }*/

    // Update is called once per frame
    void Update()
    {
        positionInit = transform.position;
        positionFin = positionInit;

        //Deplacement horizontal
        moveDirection = Input.GetAxis("Horizontal") * acceleration * Vector3.right;
        positionFin += moveDirection;

        //Saut
        if(IsGrounded() && Input.GetKeyDown(KeyCode.Space) &&!isJumpingUp)
        {
            jumpStartPosition = positionInit;
            isJumpingUp = true;
        }
        //En train de sauter
        if(isJumpingUp)
        {
            positionFin += Vector3.up * verticalacceleration * Time.deltaTime;
        }
        
        //Hauteur maximale atteinte
        if(positionFin.y >= jumpStartPosition.y + jumpHeight)
        {
            isJumpingUp = false;
        }

        //Chute
        if(!IsGrounded() && !isJumpingUp)
        {
            positionFin += Vector3.down * verticalacceleration * Time.deltaTime;
        }


        //Collisions
        if(IsBlockedRight())
        {
            positionFin.x = Mathf.Min(positionInit.x,positionFin.x);
        }
        if (IsBlockedLeft())
        {
            positionFin.x = Mathf.Max(positionInit.x, positionFin.x);
        }

        //Mouvement
        transform.position = positionFin;
    }



    //TODO : dimensions relatives au Player
    bool IsGrounded() 
    {
        return Physics2D.Raycast(transform.position - 0.51f * Vector3.up, Vector3.down, 0.01f);
    }

    bool IsBlockedRight()
    {
        return Physics2D.Raycast(transform.position + 0.51f * Vector3.right, Vector3.right, 0.01f);
    }

    bool IsBlockedLeft()
    {
        return Physics2D.Raycast(transform.position + 0.51f * Vector3.left, Vector3.left, 0.01f);
    }
}
