using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    private static string moveInput = "Move";
    private static string jumpInput = "Jump";
    private static string attackInput = "Attack";
    private static string dodgeInput = "Dodge";

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

    public static bool IsButtonPressed(string input)
    {
        return Input.GetButton(input);
    }

}
