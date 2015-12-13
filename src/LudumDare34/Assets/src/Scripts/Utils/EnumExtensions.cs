using System;
using UnityEngine;
using System.Collections;

public static class EnumExtensions
{
    public static KeyCode ToKeyCode(this JumpButton jumpButton)
    {
        switch (jumpButton)
        {
            case JumpButton.Undefined:
                return KeyCode.None;
            case JumpButton.LeftArrow:
                return KeyCode.LeftArrow;
            case JumpButton.RightArrow:
                return KeyCode.RightArrow;
            default:
                throw new ArgumentOutOfRangeException("jumpButton");
        }
    }
}
