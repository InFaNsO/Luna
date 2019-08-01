using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpStrength;
    [SerializeField]
    private bool facingRight;
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;
    private float attackDelay;
    private float iFrameDistance;
    private float distanceToGround;
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
        if (InputManager.IsButtonPressed(InputManager.GetJumpInput()) && isGrounded())
        {
            y = Input.GetAxisRaw(InputManager.GetJumpInput()) * Time.deltaTime * player.GetJumpStrength();
            transform.Translate(0f, y, 0f);
        }
    }

    private void Attack()
    {
        if (InputManager.IsButtonPressed(InputManager.GetAttackInput()) && attackDelay <= 0.0f)
        {

            player.Attack();
            attackDelay = player.GetAttackSpeed();
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
        if (Input.GetButtonDown(InputManager.GetDropInput()))
        {
            player.DropWeapon();
        }
    }

    private void SwitchWeapon()
    {
        if (Input.GetButtonDown(InputManager.GetSwitchInput()))
        {
            player.SwitchWeapon();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private bool isGrounded()
    {
        return (transform.position.y - distanceToGround <= 0.0f);
    }

    void Start()
    {
        player = gameObject.GetComponent<Player>();
        if (player == null)
        {
            Debug.Log("No [player");
        }

        facingRight = true;

        //object's mid to ground
        distanceToGround = 0.5f;

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
        attackDelay -= Time.deltaTime;
    }
}
