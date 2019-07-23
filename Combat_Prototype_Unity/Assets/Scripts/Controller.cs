using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpStrength;
    [SerializeField]
    private bool facingRight;
    private float x;
    private float y;
    private float attackSpeed;
    private float iFrameDistance;
    private float iFrameDuration;
    private float iFrameCoolDown;
    private float distanceToGround;
    Player player;

    private void Move()
    {
        if((Input.GetAxisRaw(InputManager.GetMoveInput()) > 0 && !facingRight) || (Input.GetAxisRaw(InputManager.GetMoveInput()) < 0 && facingRight))
        {
            Flip();
        }
        if(InputManager.IsButtonPressed(InputManager.GetMoveInput()))
        {
            x = Mathf.Abs(Input.GetAxisRaw(InputManager.GetMoveInput()) * Time.deltaTime * movementSpeed);
            transform.Translate(x, 0f, 0f);
        }
    }

    private void Jump()
    {
        if (InputManager.IsButtonPressed(InputManager.GetJumpInput()) && isGrounded())
        {
            y = Input.GetAxisRaw(InputManager.GetJumpInput()) * Time.deltaTime * jumpStrength;
            transform.Translate(0f, y, 0f);
        }
    }

    private void Attack()
    {
        if (InputManager.IsButtonPressed(InputManager.GetAttackInput()) && attackSpeed <= 0.0f)
        {

            player.Attack();

            //attackSpeed = player.GetEquippedWeapon().GetAttackSpeed()
            //this would set the attack cool down to 1 sec
            attackSpeed = 1.0f;
        }
    }

    private void Dodge()
    {
        if (InputManager.IsButtonPressed(InputManager.GetDodgeInput()) && iFrameCoolDown <= 0.0f)
        {
            transform.Translate(iFrameDistance * Time.deltaTime, 0f, 0f);
            iFrameCoolDown = 2.0f;
            iFrameDuration = 1.0f;
        }
    }

    private void playerIframe()
    {
        if(iFrameDuration >= 0.0f)
        {
            //player.Invicible();
        }
        else
        {
            //player.NotInvincible();
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
        player = new Player();

        facingRight = true;

        //object's mid to ground
        distanceToGround = 0.5f;

        //movementSpeed = player.GetMovementSpeed();
        movementSpeed = 5.0f;

        //jumpStrength = player.GetJumpStrength();
        jumpStrength = 20.0f;

        //iFrameDuration = player.GetIframeDuration();
        iFrameDuration = 1.0f;

        //iFrameCoolDown = player.GetDodgeCoolDown();
        iFrameCoolDown = 2.0f;

        //iFrameDistance = player.GetIframeDistance();
        iFrameDistance = 150.0f;

        //attackSpeed = player.GetEquippedWeapon().GetAttackSpeed()
        attackSpeed = 1.0f;
    }

    void Update()
    {
        Move();
        Jump();
        Attack();
        Dodge();
        attackSpeed -= Time.deltaTime;
        iFrameCoolDown -= Time.deltaTime;
        iFrameDuration -= Time.deltaTime;
    }
}
