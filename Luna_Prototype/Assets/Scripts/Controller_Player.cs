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

    private float ratio;
    [SerializeField]
    private float totalAccelTime;
    [SerializeField]
    private float accelPercentagePerFrame;

    [SerializeField]
    private float airMoveRatio;
    private float playerSpeed;

    private void Move()
    {
        if ((Input.GetAxisRaw(InputManager.GetMoveInput()) > 0 && !facingRight) || (Input.GetAxisRaw(InputManager.GetMoveInput()) < 0 && facingRight))
        {
            Flip();
        }
        if (InputManager.IsButtonPressed(InputManager.GetMoveInput()))
        {
            if (ratio < totalAccelTime)
            {
                ratio += accelPercentagePerFrame * Time.deltaTime;
            }
            if(!isGrounded)
            {
                playerSpeed = player.GetMoveSpeed() * airMoveRatio;
            }
            else
            {
                playerSpeed = player.GetMoveSpeed();
            }
            x = Mathf.Abs(Input.GetAxisRaw(InputManager.GetMoveInput()) * Time.deltaTime * playerSpeed * ratio);
            transform.Translate(x, 0f, 0f);
        }
        else
        {
            ratio = 0.0f;
        }
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

    private void DashLeft()
    {
        if(Input.GetKeyDown("a"))
        {
            if(player.Dodge() && (dashLeftWindow > 0))
            {
                transform.Translate(player.GetIFrameDistance() * Time.deltaTime, 0f, 0f);
            }
            dashRightWindow = 0.0f;
            dashLeftWindow = 1.0f;
        }
    }

    private void DashRight()
    {
        if(Input.GetKeyDown("d"))
        {
            if (player.Dodge() && (dashRightWindow > 0))
            {
                transform.Translate(player.GetIFrameDistance() * Time.deltaTime, 0f, 0f);
            }
            dashLeftWindow = 0.0f;
            dashRightWindow = 1.0f;
        }
    }

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
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if(player.GetDoubleJumpEnable())
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
        DashLeft();
        DashRight();
        dashLeftWindow -= Time.deltaTime;
        dashRightWindow -= Time.deltaTime;
        attackDelay -= Time.deltaTime;
    }

}