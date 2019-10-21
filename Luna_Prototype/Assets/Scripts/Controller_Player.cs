using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    [SerializeField]
    private bool facingRight;
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;
    private float attackDelay;
    private float iFrameDistance;
    private bool isGrounded;
    Player player;
    private int jumpCount;
    private Rigidbody2D rb;

    private float dashRightWindow;
    private float dashLeftWindow;

    [SerializeField]
    private bool isPlayerMoving;
    private float ratio;
    [SerializeField]
    private float totalAccelTime;
    [SerializeField]
    private float accelPercentagePerFrame;

    [SerializeField]
    private float airMoveRatio;
    private float playerSpeed;


    [SerializeField]
    private float joystickX;
    private void Move()
    {
        Flip();
        //for movement, keyboard has priority over joystick
        if (Input.GetAxisRaw(InputManager.GetMoveInput()) != 0.00f)
        {
            x = Mathf.Abs(Input.GetAxisRaw(InputManager.GetMoveInput()) * Time.deltaTime);
            isPlayerMoving = true;
        }
        else if(joystickX != 0)
        {
            x = Mathf.Abs(Input.GetAxisRaw(InputManager.GetJoystickHorizontal()) * Time.deltaTime);
            isPlayerMoving = true;
        }
        else
        {
            x = 0.0f;
            isPlayerMoving = false;
        }

        if(isPlayerMoving)
        {
            if (ratio < totalAccelTime)
            {
                ratio += accelPercentagePerFrame * Time.deltaTime;
            }

            x *= ratio;

            if (!isGrounded)
            {
                playerSpeed = player.GetMoveSpeed() * airMoveRatio;
                x *= playerSpeed;
            }
            else
            {
                playerSpeed = player.GetMoveSpeed();
                x *= playerSpeed;
            }
        }
        else
        {
            ratio = 0.0f;
        }

        transform.Translate(x, 0.0f, 0.0f);
        Vector3 pos = transform.position;
        pos.z = 0.0f;
        transform.position = pos;
    }

    private void Jump()
    {
        if (InputManager.IsButtonDown(InputManager.GetJumpInput()) && jumpCount > 0)
        {
            y = Input.GetAxisRaw(InputManager.GetJumpInput()) * Time.deltaTime * player.GetJumpStrength();
            //y = player.GetJumpStrength();
            //transform.Translate(0f, y, 0f);
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            rb.AddForce(new Vector2(0.0f, y), ForceMode2D.Impulse);
            //rb.AddForce(new Vector2(0.0f, y), ForceMode2D.Impulse);
            Debug.Log(jumpCount);
            jumpCount--;
        }
    }

    private void Attack()
    {
        if (InputManager.IsButtonPressed(InputManager.GetAttackInput()))
        {
            player.Attack();
        }
    }

    private void Dodge()
    {
        if (InputManager.IsButtonPressed(InputManager.GetDodgeInput()))
        {
            if (player.Dodge())
            {
                transform.Translate(player.GetIFrameDistance() * Time.deltaTime, 0f, 0f);
            }
        }
    }

    //private void DashLeft()
    //{
    //    if(Input.GetKeyDown("a"))
    //    {
    //        if(player.Dodge() && (dashLeftWindow > 0))
    //        {
    //            transform.Translate(player.GetIFrameDistance() * Time.deltaTime, 0f, 0f);
    //        }
    //        dashRightWindow = 0.0f;
    //        dashLeftWindow = 1.0f;
    //    }
    //}

    //private void DashRight()
    //{
    //    if(Input.GetKeyDown("d"))
    //    {
    //        if (player.Dodge() && (dashRightWindow > 0))
    //        {
    //            transform.Translate(player.GetIFrameDistance() * Time.deltaTime, 0f, 0f);
    //        }
    //        dashLeftWindow = 0.0f;
    //        dashRightWindow = 1.0f;
    //    }
    //}

    private void DropWeapon()
    {
        if (InputManager.IsButtonDown(InputManager.GetDropInput()))
        {
            player.DropWeapon();
        }
    }

    private void LaserAttack()
    {
        if (InputManager.IsButtonDown(InputManager.GetLaserAttackInput()))
        {
            player.LaserAttack();
        }
    }

    private void SwitchWeapon()
    {
        if (InputManager.IsButtonDown(InputManager.GetSwitchInput()))
        {
            player.SwitchWeapon();
        }
    }

    private void Flip()
    {
        joystickX = Mathf.Round(Input.GetAxisRaw(InputManager.GetJoystickHorizontal()));
        if ((Input.GetAxisRaw(InputManager.GetMoveInput()) > 0 && !facingRight) || (Input.GetAxisRaw(InputManager.GetMoveInput()) < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        else if ((joystickX > 0 && !facingRight) || (joystickX < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            player.isGrounded = true; //|--- [Mingzhuo Zhang] Edit: I need to know is player is on ground or not for my weapon combo system

            if (player.GetDoubleJumpEnable())
            {
                jumpCount = 2;
            }
            else
            {
                jumpCount = 1;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            player.isGrounded = false;  //|--- [Mingzhuo Zhang] Edit: I need to know is player is on ground or not for my weapon combo system
        }
    }

    void Awake()
    {
        player = gameObject.GetComponent<Player>();
        if (player == null)
        {
            Debug.Log("No [player]");
        }

        facingRight = true;

        attackDelay = player.GetAttackSpeed();
        if (player.GetDoubleJumpEnable())
        {
            jumpCount = 2;
        }
        else
        {
            jumpCount = 1;
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.Log("[rb is null]");
        }
        dashLeftWindow = 0.0f;
        dashRightWindow = 0.0f;
        isPlayerMoving = false;
        ratio = 0.0f;
        accelPercentagePerFrame = 1.6f;
        totalAccelTime = 1.0f;
        airMoveRatio = 0.5f;
        playerSpeed = player.GetMoveSpeed();
    }

    void FixedUpdate()
    {
        Jump();
        Move();
        Attack();
        Dodge();
        DropWeapon();
        SwitchWeapon();
        LaserAttack();
        //DashLeft();
        //DashRight();
        //dashLeftWindow -= Time.deltaTime;
        //dashRightWindow -= Time.deltaTime;
        attackDelay -= Time.deltaTime;
    }

}