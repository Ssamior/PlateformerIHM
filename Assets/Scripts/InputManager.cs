using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public GameObject player;
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
    public LayerMask Water;
    public LayerMask Obstacles;
    public LayerMask Victory;
    public GameObject jumpParticle;
    public GameObject sprintParticle;


    private Vector3 jumpStartPosition;
    private GameObject ground;
    private bool isJumpingUp;
    private bool isSprinting;
    private float lastSprintParticle = 0;
    private Vector3 moveDirection;
    private Vector3 positionInit;
    private Vector3 positionFin;
    private int jumpsNumber;
    private float horizontalInput;
    private float lastDash;
    private float groundFriction;
    private float airFriction;
    private float yVel = 0;
    private float timeqt = 0;
    private bool isInWater;
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
        timeqt = Time.deltaTime;
        isInWater = IsInWater(positionInit);

        //Inputs
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyUp(KeyCode.JoystickButton4))
        {
            Dash();
        }
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Fire1") != 0)
        {
            groundFriction = isInWater ? sprintGroundFriction/2 : sprintGroundFriction;
            isSprinting = true;
        }
        else if (Input.GetAxis("Fire1") == 0)
        {
            groundFriction = isInWater ? baseGroundFriction/2 : baseGroundFriction;
            isSprinting = false;
        }

        //Deplacement horizontal
        moveDirection = timeqt * horizontalInput * acceleration * groundFriction * airFriction * Vector3.right;
        positionFin += moveDirection;
        if(isSprinting && IsGrounded(positionFin) && !isInWater)
        {
            SprintParticles(horizontalInput);
        }

        //Saut
        //Input de saut
        if (CanJump() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.JoystickButton0)))
        {
            jumpStartPosition = positionInit;
            isJumpingUp = true;
            jumpsNumber += isInWater ? 0 : 1; //on ne compte pas les sauts dans l'eau
            yVel = isInWater ? verticalAcceleration/1.5f : verticalAcceleration; //impulsion
            JumpParticles();
        }
        //Hauteur maximale atteinte
        if(positionFin.y >= jumpStartPosition.y + jumpHeight)
        {
            isJumpingUp = false;
        }
        //Chute
        if(!IsGrounded(positionFin) && !isJumpingUp)
        {
            airFriction = isInWater ? fallingAirFriction/2 : fallingAirFriction;
            yVel -= isInWater ? gravity * timeqt / 2 : gravity * timeqt;
        }

        //Cooldowns
        if (Time.time - lastDash <= dashCooldown)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, Color.red, (Time.time - lastDash)/dashCooldown);
        }


        //Collisions
        if(IsBlockedRight(positionFin))
        {
            jumpsNumber = 1;
            positionFin.x = Mathf.Min(positionInit.x,positionFin.x);
        }
        if (IsBlockedLeft(positionFin))
        {
            jumpsNumber = 1;
            positionFin.x = Mathf.Max(positionInit.x, positionFin.x);
        }
        if (IsGrounded(positionFin))
        {
            jumpsNumber = 0;
            airFriction = isInWater ? baseAirFriction/2 : baseAirFriction;
            yVel = 0;
        }
        if(IsInsidePlatform(positionFin))
        {
            positionFin += 10 * timeqt * Vector3.up;
        }
        if (IsInObstacle(positionFin))
        {
            Die();
        }
        if (GetVictoryToken(positionFin))
        {
            Win();
        }


        //Mouvement
        positionFin.y += yVel * timeqt;
        transform.position = positionFin;

        //Si hors de l'écran 
        if (Mathf.Abs(positionFin.x)>50 || Mathf.Abs(positionFin.y)>50)
        {
            Die();
        }
    }


    //TODO : dimensions relatives au Player

    bool IsInWater(Vector3 position)
    {
        return Physics2D.Raycast(position, Vector3.up, 0.5f, Water);
    }
    bool IsInObstacle(Vector3 position)
    {
        return Physics2D.Raycast(position + transform.localScale.x * 0.4f * Vector3.left, Vector3.down, transform.localScale.y * 0.45f, Obstacles) || Physics2D.Raycast(position - transform.localScale.x * 0.4f * Vector3.left, Vector3.down, transform.localScale.y * 0.45f, Obstacles);
    }
    bool GetVictoryToken(Vector3 position)
    {
        return Physics2D.Raycast(position + transform.localScale.x * 0.4f * Vector3.left, Vector3.down, transform.localScale.y * 0.45f, Victory) || Physics2D.Raycast(position - transform.localScale.x * 0.4f * Vector3.left, Vector3.down, transform.localScale.y * 0.45f, Victory);
    }
    bool IsInsidePlatform(Vector3 position)
    {
        return Physics2D.Raycast(position + transform.localScale.x * 0.4f * Vector3.left, Vector3.down, transform.localScale.y * 0.45f, Walls) || Physics2D.Raycast(position - transform.localScale.x * 0.4f * Vector3.left, Vector3.down, transform.localScale.y * 0.45f, Walls);
    }
    bool IsGrounded(Vector3 position)
    {
        return (Physics2D.Raycast(position + transform.localScale.x *0.4f * Vector3.left + transform.localScale.y / 2 * Vector3.down, Vector3.down,0.01f, Walls) || Physics2D.Raycast(position - transform.localScale.x * 0.4f * Vector3.left + transform.localScale.y / 2 * Vector3.down, Vector3.down, 0.01f, Walls)) && !isJumpingUp;
    }
    bool IsBlockedRight(Vector3 position)
    {
        return Physics2D.Raycast(position + transform.localScale.x / 2 * Vector3.right, Vector3.right, 0.01f, Walls);
    }
    bool IsBlockedLeft(Vector3 position)
    {
        return Physics2D.Raycast(position + transform.localScale.x / 2 * Vector3.left, Vector3.left, 0.01f, Walls);
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

    void JumpParticles()
    {
        Instantiate(jumpParticle, new Vector3(transform.position.x, transform.position.y - transform.localScale.y/2, 0), Quaternion.identity);
    }
    void SprintParticles(float direction)
    {
        if(direction != 0 && Time.time > lastSprintParticle + 0.08f && Random.Range(0, 5) == 0)
        {
            GameObject goParticle = Instantiate(sprintParticle, new Vector3(transform.position.x - direction * transform.localScale.x, transform.position.y - transform.localScale.y / 2, 0), Quaternion.identity) as GameObject;
            goParticle.SendMessage("setDirection", -direction);
            lastSprintParticle = Time.time;
        }
        
    }

    void Die()
    {
        GameManager gm = FindObjectOfType <GameManager>();
        gm.KillPlayer(gameObject);
    }

    void Win()
    {
        SceneManager.LoadScene("VictoryMenu");
    }
}
