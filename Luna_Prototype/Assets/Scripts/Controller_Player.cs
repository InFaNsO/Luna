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

    private void Move()
    {
        if ((Input.GetAxisRaw(InputManager.GetMoveInput()) > 0 && !facingRight) || (Input.GetAxisRaw(InputManager.GetMoveInput()) < 0 && facingRight))
        {
            Flip();
        }
        if (InputManager.IsButtonPressed(InputManager.GetMoveInput()))
        {
            x = Mathf.Abs(Input.GetAxisRaw(InputManager.GetMoveInput()) * Time.deltaTime * player.GetMoveSpeed());
            transform.Translate(x, 0f, 0f);
        }
    }

    private void Jump()
    {
        if (InputManager.IsButtonPressed(InputManager.GetJumpInput()) && isGrounded)
        {
            y = Input.GetAxisRaw(InputManager.GetJumpInput()) * Time.deltaTime * player.GetJumpStrength();
            transform.Translate(0f, y, 0f);
        }
    }

    private void Attack()
    {
        if (InputManager.IsButtonPressed(InputManager.GetAttackInput()) /*&& attackDelay <= 0.0f*/) // [Mingzhuo Zhang edit]--- attack delay is in the weapon no need to check here
        {
            player.Attack();
            //attackDelay = player.GetAttackSpeed();                                                // [Mingzhuo Zhang edit]--- attack delay is in the weapon no need to check here
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
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Start()
    {
        player = gameObject.GetComponent<Player>();
        if (player == null)
        {
            Debug.Log("No [player]");
        }

        facingRight = true;

        attackDelay = player.GetAttackSpeed();
    }

    void Update()
    {
        Move();
        Jump();
        Attack();
        Dodge();
        DropWeapon();
        SwitchWeapon();
        LaserAttack();
        attackDelay -= Time.deltaTime;
    }
}
