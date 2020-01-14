using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [SerializeField]
    private float mMaxStamina = 100f;
    [SerializeField]
    private float mCurrentStamina;
    [SerializeField]
    private float mStaminaCostPerDash= 20f;
    private bool isStaminaSufficient;

    public Slider staminaBar;

    private void Start()
    {
        mCurrentStamina = mMaxStamina;
        isStaminaSufficient = true;
        UpdateStaminaBar();
    }

    private void FixedUpdate()
    {
        if (mCurrentStamina < mMaxStamina)
            mCurrentStamina += Time.deltaTime * 10.0f;
    }

    private void Update()
    {
        if (mCurrentStamina < mStaminaCostPerDash)
            isStaminaSufficient = false;
        else
            isStaminaSufficient = true;
        UpdateStaminaBar();
    }

    public void UseStamina()
    {
        if (isStaminaSufficient)
            mCurrentStamina -= mStaminaCostPerDash;
    }

    private void UpdateStaminaBar()
    {
        staminaBar.value = mCurrentStamina / mMaxStamina;
    }
    public bool IsStaminaSufficient()
    {
        return isStaminaSufficient;
    }
}
