using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    private static string moveInput = "Move";
    private static string jumpInput = "Jump";
    private static string attackInput = "Attack";
    private static string dodgeInput = "Dodge";
    private static string switchWeaponInput = "Switch";
    private static string dropWeaponInput = "Drop";

    public static string GetMoveInput()
    {
        return moveInput;
    }

    public static string GetJumpInput()
    {
        return jumpInput;
    }

    public static string GetAttackInput()
    {
        return attackInput;
    }

    public static string GetDodgeInput()
    {
        return dodgeInput;
    }

    public static string GetSwitchInput()
    {
        return switchWeaponInput;
    }

    public static string GetDropInput()
    {
        return dropWeaponInput;
    }

    public static bool IsButtonPressed(string input)
    {
        return Input.GetButton(input);
    }

}
