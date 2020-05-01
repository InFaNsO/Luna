using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MZEnemy_AnimationController : MonoBehaviour
{
    MZEnemyBat mEnemyBat;
    Animator mAnimator;
    void Awake()
    {
        mEnemyBat = GetComponentInParent<MZEnemyBat>();
        mAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
            
    }

    private void LateUpdate()
    {
        
    }

    public void GoMeleeAttackAnimation()
    {
        mAnimator.SetTrigger("MeleeAttack");
    }

    public void GoRangeAttackAnimation(float animationSpeedMutiplyer)
    {
        if (animationSpeedMutiplyer < 1.0f) animationSpeedMutiplyer = 1.0f;
        mAnimator.SetTrigger("RangeAttack");
        mAnimator.SetFloat("attackAnimationSpeedMultiplyer", animationSpeedMutiplyer);
    }

    public void GoDeathAnimation()
    {
        mAnimator.SetTrigger("Death");
    }

    public void MeleeAttack()
    {
        mEnemyBat.MeleeAttack();
    }

    public void RangeAttack()
    {
        mEnemyBat.RangeAttack();
    }

    public void Die()
    {
        mEnemyBat.RealDie();
    }
}
