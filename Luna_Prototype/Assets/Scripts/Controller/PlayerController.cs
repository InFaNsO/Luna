using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputController controls;
    Player player;
    BoxCollider2D playerCollider;
    Rigidbody2D rb;
    Stamina stamina;
    ParryAttackable parry;

    [SerializeField]
    bool isPlayerFacingRight;

    [SerializeField]
    bool isGrounded;

    public float controllerSensitivity = 0.5f;

    [SerializeField]
    Vector3 moveVec;
    [SerializeField]
    Vector2 jumpVec;

    int jumpCount = 2;

    [SerializeField]
    float walkAcc = 75;
    [SerializeField]
    float airAcc = 30;
    [SerializeField]
    float groundDec = 70;

    [SerializeField]
    LayerMask groundLayerMask = ~0;

    [SerializeField]
    float dashDuration = 0.14f;
    float dashCounter;
    bool isDashing;

    Inventory inventory;

    private void Awake()
    {
        player = GetComponent<Player>();
        controls = new InputController();
        playerCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        stamina = GetComponent<Stamina>();
        parry = GetComponent<ParryAttackable>();

        controls.PlayerControl.Jump.performed += _jump => Jump();
        controls.PlayerControl.Attack.performed += _attack => Attack();
        controls.PlayerControl.SwitchWeapon.performed += _switch => SwitchWeapon();
        controls.PlayerControl.DropWeapon.performed += _drop => DropWeapon();
        controls.PlayerControl.Dash.performed += _dash => Dash();
        controls.PlayerControl.UseItem.performed += _use => UseItem();
        controls.PlayerControl.SelectPrevItem.performed += _selectprev => SelectPrevItem();
        controls.PlayerControl.SelectNextItem.performed += _selectnext => SelectNextItem();
        controls.PlayerControl.Parry.performed += _parry => Parry();

        controls.PlayerControl.PickUpWeapon.performed += _pickupitem => PickUpWeapon();


        moveVec = new Vector3(0f, 0f, 0f);
        jumpVec = new Vector3(0f, 0f);
        isPlayerFacingRight = true;
        isGrounded = true;

        dashCounter = 1.0f;
        inventory = GetComponent<Inventory>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        JumpUpdate();
        DashMovement();
        Resets();
    }

    public void Parry()
    {
        parry.Parry();
    }

    void Resets()
    {
        jumpVec.y = 0.0f;
        Vector3 setZ = transform.position;
        setZ.z = 0.0f;
        transform.position = setZ;
    }
    public void PickUpWeapon()
    {
        player.PickUpNearbyWeapon();
    }
    public void UseItem()
    {
        inventory.UseItem();
    }

    public void SelectPrevItem()
    {
        inventory.SelectPrevItem();
    }

    public void SelectNextItem()
    {
        inventory.SelectNextItem();
    }

    public void Move()
    {
        moveVec.x = controls.PlayerControl.Move.ReadValue<float>();
        if (Mathf.Abs(moveVec.x) < controllerSensitivity)
            moveVec.x = 0.0f;
        Flip();
        float acc = isGrounded ? walkAcc : airAcc;
        float dec = isGrounded ? groundDec : 0;

        moveVec.x = Mathf.MoveTowards(moveVec.x, player.GetMoveSpeed() * moveVec.x, acc * Time.deltaTime);
        if (!isPlayerFacingRight && moveVec.x < 0.0f || !isPlayerFacingRight && moveVec.x > 0.0f)
        {
            moveVec.x = -moveVec.x;
        }
        transform.Translate(moveVec * Time.deltaTime);
    }

    void Flip()
    {
        if ((moveVec.x > 0 && !isPlayerFacingRight) || (moveVec.x < 0 && isPlayerFacingRight))
        {

            if (player.mCurrentWeapon == null || !player.mCurrentWeapon.mIsAttacking)
            {
                isPlayerFacingRight = !isPlayerFacingRight;
                transform.Rotate(0f, 180f, 0f);
            }
        }
    }

    void JumpUpdate()
    {
        if ((jumpVec.y > 0) && (jumpCount > 0))
        {
            jumpVec.y *= Time.deltaTime * player.GetJumpStrength();
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            rb.AddForce(jumpVec, ForceMode2D.Impulse);
        }
    }

    public void Jump()
    {
        jumpVec.y = 1.0f;
        if (jumpCount > 0)
            jumpCount--;
    }

    public void Attack()
    {
        player.Attack();
    }

    public void DropWeapon()
    {
        player.DropWeapon();
    }

    public void SwitchWeapon()
    {
        player.SwitchWeapon();
    }

    public void Dash()
    {
        if (stamina.IsStaminaSufficient() && (dashCounter >= dashDuration))
        {
            //if(isPlayerFacingRight)
            //    rb.AddForce(new Vector2(player.GetIFrameDistance(), 0f));
            //else
            //    rb.AddForce(new Vector2(-player.GetIFrameDistance(), 0f));
            //transform.Translate(player.GetIFrameDistance() * Time.deltaTime, 0f, 0f);
            dashCounter = 0.0f;
            stamina.UseStamina();
        }
    }

    void DashMovement()
    {
        if (dashCounter < dashDuration)
        {
            transform.Translate(player.GetDashSpeed() * Time.deltaTime, 0f, 0f);
            isDashing = true;
        }
        else
        {
            isDashing = false;
        }
        if (dashCounter < 5.0f)
            dashCounter += Time.deltaTime;
    }

    void GroundCheck()
    {
        player.isGrounded = false;
        isGrounded = false;

        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(playerCollider.bounds.center + Vector3.up * 0.02f, playerCollider.bounds.size, 0f, Vector3.down, 0.04f, groundLayerMask);
        foreach (var hit in raycastHits)
        {
            //if (hit.collider != playerCollider && hit.collider.tag != "PickUp" && hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
            //{

            float angle = Vector2.Angle(hit.normal, Vector2.up);
            if (angle < 70)
            {
                player.isGrounded = true;
                isGrounded = true;
                jumpCount = player.GetDoubleJumpEnable() ? 2 : 1;
            }
            //}
        }

        //Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, playerCollider.size, 0);

        //foreach (Collider2D hit in hits)
        //{
        //    // Ignore our own collider.
        //    if (hit == playerCollider)
        //        continue;

        //    ColliderDistance2D colliderDistance = hit.Distance(playerCollider);

        //    // Ensure that we are still overlapping this collider.
        //    // The overlap may no longer exist due to another intersected collider
        //    // pushing us out of this one.
        //    if (colliderDistance.isOverlapped)
        //    {
        //        //transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

        //        // If we intersect an object beneath us, set grounded to true. 
        //        if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && jumpVec.y < 0)
        //        {
        //            isGrounded = true;
        //        }
        //    }
        //}
    }

    private void OnEnable()
    {
        controls.PlayerControl.Enable();
    }

    private void OnDisable()
    {
        controls.PlayerControl.Disable();
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsMoving()
    {
        if (Mathf.Abs(controls.PlayerControl.Move.ReadValue<float>()) > controllerSensitivity)
            return true;
        else
            return false;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
}
