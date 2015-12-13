using System;
using UnityEngine;
using System.Collections;

public class JumpPhysics
{
    public enum State
    {
        Undefined,
        Ground,
        Air
    }


    public State CurrentState;
    public float _floorPosition;
    private float _ypos;

    public float _initialVelocty = 20f;
    public float _gravity = 30f;
    public float _initialJumpDistance = 2.5f;
    public float Velocity = 0f;

    public float _heldButtonAmount = 3f;


    public float YPos {get { return _ypos; }}

    private bool _jumpButtonPressed = false;


    public JumpPhysics(float floorPosition, float initialJumpDistance, float initialJumpStrength, float gravity)
    {
        CurrentState = State.Ground;

        _floorPosition = floorPosition;
        _ypos = _floorPosition;
    }

    public void JumpButtonPressed(bool value)
    {
        _jumpButtonPressed = value;
    }

    public void Tick(float deltatime)
    {
        switch (CurrentState)
        {
            case State.Ground:
                // TODO: Cooldown so you can't just hold the button
                if (_jumpButtonPressed)
                {
                    Jump();
                }
                break;
            case State.Air:

                //  Add velocity from button being pressed for x available seconds
                //  Decrease velocity by gravity
                //  If going to be under the floor...
                //  Set state to ground
                //  Set velocity to 0

                if (_jumpButtonPressed)
                {
                    Velocity += _heldButtonAmount * deltatime;
                }

                Velocity -= _gravity*deltatime;
                _ypos += (deltatime*Velocity);
                if (_ypos < _floorPosition)
                {
                    Land();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Land()
    {
        CurrentState = State.Ground;
        Velocity = 0f;
        _ypos = _floorPosition;
    }


    private void Jump()
    {
        CurrentState = State.Air;
        Velocity = _initialVelocty;
        _ypos += _initialJumpDistance;
        // Set initial velocity
        // Set jump button timer (how long it can be held)
    }

}
