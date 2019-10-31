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

    [SerializeField]
    bool isPlayerFacingRight;

    [SerializeField]
    bool isGrounded;

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


    private void Awake()
    {
        player = GetComponent<Player>();
        controls = new InputController();
        playerCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        controls.PlayerControl.Jump.performed += _jump => Jump();
        controls.PlayerControl.Attack.performed += _attack => Attack();
        controls.PlayerControl.SwitchWeapon.performed += _switch => SwitchWeapon();
        controls.PlayerControl.DropWeapon.performed += _drop => DropWeapon();

        moveVec = new Vector3(0f, 0f, 0f);
        jumpVec = new Vector3(0f, 0f);
        isPlayerFacingRight = true;
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        JumpUpdate();
        Resets();
    }

    void Resets()
    {
        jumpVec.y = 0.0f;
        Vector3 setZ = transform.position;
        setZ.z = 0.0f;
        transform.position = setZ;
    }

    public void Move()
    {
        moveVec.x = controls.PlayerControl.Move.ReadValue<float>();
        Flip();

        float acc = isGrounded ? walkAcc : airAcc;
        float dec = isGrounded ? groundDec : 0;

        moveVec.x = Mathf.MoveTowards(Mathf.Abs(moveVec.x), player.GetMoveSpeed() * Mathf.Abs(moveVec.x), acc * Time.deltaTime);
        transform.Translate(moveVec * Time.deltaTime);
    }

    void Flip()
    {
        if ((moveVec.x > 0 && !isPlayerFacingRight) || (moveVec.x < 0 && isPlayerFacingRight))
        {
            isPlayerFacingRight = !isPlayerFacingRight;
            transform.Rotate(0f, 180f, 0f);
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
        if(jumpCount > 0)
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

    void GroundCheck()
    {
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(playerCollider.bounds.center, playerCollider.bounds.size/2, 0f, Vector3.down, playerCollider.bounds.extents.y);
        foreach (var hit in raycastHits)
        {
            if(hit.collider != null)
            {
                if(hit.collider != playerCollider)
                {
                    if (hit.collider.tag != "PickUp")
                    {
                        isGrounded = true;
                        jumpCount = player.GetDoubleJumpEnable() ? 2 : 1;
                        Debug.Log(hit.collider.tag);
                    }
                    else
                    {
                        isGrounded = false;
                    }
                }
            }
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
}
