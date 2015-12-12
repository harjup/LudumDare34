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


    private State _state;
    public float _floorPosition;
    private float _ypos;

    public float _initialVelocty = 20f;
    public float _gravity = 30f;
    public float _initialJumpDistance = 2.5f;
    private float _velocity = 0f;

    public float _heldButtonAmount = 3f;


    public float YPos {get { return _ypos; }}

    private bool _jumpButtonPressed = false;


    public JumpPhysics(float floorPosition, float initialJumpDistance, float initialJumpStrength, float gravity)
    {
        _state = State.Ground;

        _floorPosition = floorPosition;
        _ypos = _floorPosition;
    }

    public void JumpButtonPressed(bool value)
    {
        _jumpButtonPressed = value;
    }

    public void Tick(float deltatime)
    {
        switch (_state)
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
                    _velocity += _heldButtonAmount * deltatime;
                }

                _velocity -= _gravity*deltatime;
                _ypos += (deltatime*_velocity);
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
        _state = State.Ground;
        _velocity = 0f;
        _ypos = _floorPosition;
    }


    private void Jump()
    {
        _state = State.Air;
        _velocity = _initialVelocty;
        _ypos += _initialJumpDistance;
        // Set initial velocity
        // Set jump button timer (how long it can be held)
    }

}
