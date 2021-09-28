using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float acceleration;
    public float verticalacceleration;
    public float jumpHeight;
    private Vector3 jumpStartPosition;
    private float distToGround;
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
        distToGround = 0;
        ground = GameObject.Find("Ground");
        jumpStartPosition = positionInit;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
        //distToGround = GetComponent<Collider2D>().bounds.extents.y - 0.5f * collision.gameObject.transform.lossyScale.y;
    }

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
        //en train de sauter
        if(isJumpingUp)
        {
            positionFin += Vector3.up * verticalacceleration * Time.deltaTime;
        }

        if(positionFin.y >= jumpStartPosition.y+ jumpHeight)
        {
            isJumpingUp = false;
        }

        if(!IsGrounded() && !isJumpingUp)
        {
            positionFin += Vector3.down * verticalacceleration * Time.deltaTime;
        }


        //Collisions



        transform.position = positionFin;
    }

    bool IsGrounded() 
    {
        
        return Physics2D.Raycast(transform.position - 0.51f * Vector3.up, Vector3.down, 0.01f);
    }
}
