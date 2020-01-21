using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    enum PlayerAnimationState
    {
        PLAYER_IDLE = 0,
        PLAYER_RUN = 1
    }

    PlayerController mPlayerController;
    Animator mAnimator;
    Rigidbody2D rb;
    float mLastYVel = 0.0f;
    float mLastAccelration = 0.0f;

    PlayerAnimationState mCurrentState = PlayerAnimationState.PLAYER_IDLE;
    void Awake()
    {
        mPlayerController = GetComponent<PlayerController>();
        mAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        mCurrentState = mPlayerController.IsMoving() ? PlayerAnimationState.PLAYER_RUN : PlayerAnimationState.PLAYER_IDLE;

        mAnimator.SetInteger("playerState", (int)mCurrentState);

       
    }

    private void FixedUpdate()
    {
        float currentVelY = rb.velocity.y;
        float currentAccelation = (currentVelY - mLastYVel);
       
        if (currentAccelation > 0.0f && currentVelY > 0.0f)
        {
            Core.Debug.Log(" Jump !!!!!!!!!!!!!!!!!");
            mAnimator.SetBool("IsDoubleJump", true);
        }
        mLastYVel = currentVelY;
        mLastAccelration = currentAccelation;
    }

    void LateUpdate()
    {
        if (mAnimator.gameObject.activeSelf)
        {
            mAnimator.SetInteger("playerState", (int)PlayerAnimationState.PLAYER_IDLE);
            mAnimator.SetBool("IsOnGround", mPlayerController.IsGrounded());
            mAnimator.SetBool("IsDoubleJump", false);
        }
    }
}
