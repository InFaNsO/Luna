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
    PlayerAnimationState mCurrentState = PlayerAnimationState.PLAYER_IDLE;
    void Awake()
    {
        mPlayerController = GetComponent<PlayerController>();
        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        mCurrentState = mPlayerController.IsMoving() ? PlayerAnimationState.PLAYER_RUN : PlayerAnimationState.PLAYER_IDLE;

        mAnimator.SetInteger("playerState", (int)mCurrentState);
    }

    void LateUpdate()
    {
        if (mAnimator.gameObject.activeSelf)
        {
            mAnimator.SetInteger("playerState", (int)PlayerAnimationState.PLAYER_IDLE);
            mAnimator.SetBool("isGrounded", mPlayerController.IsGrounded());
        }
    }
}
