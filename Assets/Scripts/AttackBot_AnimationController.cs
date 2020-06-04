using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBot_AnimationController : MonoBehaviour
{
    Enemy mEnemy;
    Animator mAnimator;
    void Awake()
    {
        mEnemy = GetComponentInParent<Enemy>();
        mAnimator = GetComponent<Animator>();
    }


    void Update()
    {

    }

    private void LateUpdate()
    {

    }

    public void GoAttackAnimation(float animationSpeedMutiplyer)
    {
        //if (animationSpeedMutiplyer < 1.0f) animationSpeedMutiplyer = 1.0f;
        mAnimator.SetTrigger("MeleeAttack");
        mAnimator.SetFloat("attackAnimationSpeedMultiplyer", animationSpeedMutiplyer);
    }

    public void GoDeathAnimation()
    {
        mAnimator.SetTrigger("Death");
    }

    public void GoStunAnimation()
    {
        mAnimator.SetTrigger("Stun");
    }

    public void Attack()
    {
        mEnemy.RealAttack();
    }

    public void Die()
    {
        mEnemy.RealDie();
    }

    public void RestFromStun()
    {
        mEnemy.RestFromStun();
    }
}
