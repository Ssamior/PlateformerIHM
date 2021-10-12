using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float acceleration;
    public float verticalAcceleration;
    public float jumpHeight;
    public float gravity = 9.81f;
    public float baseAirFriction;
    public float fallingAirFriction;
    public float baseGroundFriction;
    public float sprintGroundFriction;
    public float dashCooldown;
    public LayerMask Walls;

    private Vector3 jumpStartPosition;
    private GameObject ground;
    private bool isJumpingUp;
    private Vector3 moveDirection;
    private Vector3 positionInit;
    private Vector3 positionFin;
    private int jumpsNumber;
    private float horizontalInput;
    private float lastDash;
    private float groundFriction;
    private float airFriction;
    private float yVel = 0;

    // Start is called before the first frame update
    void Start()
    {
        positionInit = transform.position;
        positionFin = positionInit;
        ground = GameObject.Find("Ground");
        jumpStartPosition = positionInit;
        jumpsNumber = 0;
        airFriction = baseAirFriction;
        groundFriction = baseGroundFriction;
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
        horizontalInput = Input.GetAxis("Horizontal");


        //Inputs
        if (Input.GetKeyDown(KeyCode.E))
        {
            Dash();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            groundFriction = sprintGroundFriction;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            groundFriction = baseGroundFriction;
        }

        //Deplacement horizontal
        moveDirection = horizontalInput * acceleration * groundFriction * airFriction * Vector3.right;
        positionFin += moveDirection;

        //Saut
        //Input de saut
        if (CanJump() && Input.GetKeyDown(KeyCode.Space))
        {
            jumpStartPosition = positionInit;
            isJumpingUp = true;
            jumpsNumber += 1;
            yVel = verticalAcceleration; //impulsion
        }
        //Hauteur maximale atteinte
        if(positionFin.y >= jumpStartPosition.y + jumpHeight)
        {
            isJumpingUp = false;
        }
        //Chute
        if(!IsGrounded() && !isJumpingUp)
        {
            airFriction = fallingAirFriction;
            yVel -= gravity * Time.deltaTime;
        }

        //Cooldowns
        if (Time.time - lastDash <= dashCooldown)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, Color.red, (Time.time - lastDash)/dashCooldown);
        }


        //Collisions
        if(IsBlockedRight())
        {
            jumpsNumber = 1;
            positionFin.x = Mathf.Min(positionInit.x,positionFin.x);
        }
        if (IsBlockedLeft())
        {
            jumpsNumber = 1;
            positionFin.x = Mathf.Max(positionInit.x, positionFin.x);
        }
        if (IsGrounded())
        {
            jumpsNumber = 0;
            airFriction = baseAirFriction;
            yVel = 0;
        }
        if(IsInsidePlatform())
        {
            positionFin += 10 * Time.deltaTime * Vector3.up;
        }

        //Mouvement
        positionFin += Vector3.up * yVel * Time.deltaTime;
        transform.position = positionFin;
    }



    //TODO : dimensions relatives au Player
    
    bool IsInsidePlatform()
    {
        return Physics2D.Raycast(transform.position - transform.localScale.y / 2 * Vector3.up + transform.localScale.x * 0.4f * Vector3.left, Vector3.down, 0.01f, Walls) || Physics2D.Raycast(transform.position - transform.localScale.y / 2 * Vector3.up - transform.localScale.x * 0.4f * Vector3.left, Vector3.down, 0.01f, Walls);
    }
    bool IsGrounded()
    {
        return IsInsidePlatform() && !isJumpingUp;
    }
    bool IsBlockedRight()
    {
        return Physics2D.Raycast(transform.position + transform.localScale.x / 2 * Vector3.right, Vector3.right, 0.01f, Walls);
    }

    bool IsBlockedLeft()
    {
        return Physics2D.Raycast(transform.position + transform.localScale.x / 2 * Vector3.left, Vector3.left, 0.01f, Walls);
    }



    bool CanJump()
    {
        return (jumpsNumber < 2);
    }

    void Dash()
    {
        Vector3 dashPosition = positionInit + 3 * Mathf.Sign(horizontalInput) * Vector3.right;
        //Si le chemin est libre
        if (!Physics2D.Raycast(positionInit, Mathf.Sign(horizontalInput) * Vector3.right, Vector3.Distance(positionInit, dashPosition), Walls) && (Time.time - lastDash) > dashCooldown)
        {
            positionFin = dashPosition;
            lastDash = Time.time;
        }

    }
}
