using UnityEngine;
using System.Collections.Generic;

public class Jumpable : MonoBehaviour
{
    public float FloorPosition = .5f;
    public float Gravity = 30f;
    public float InitialJumpDistance = .25f;
    public float InitialVelocity = 20f;
    public float HeldButtonAmount = 5f;

    public enum JumpButton
    {
        Undefined,
        LeftArrow,
        RightArrow
    }

    public JumpButton Button { get; set; }

    private JumpPhysics _jumpPhysics;

	void Start ()
	{
        _jumpPhysics = new JumpPhysics(FloorPosition, InitialJumpDistance, Gravity, InitialVelocity);
	}

    void Update()
    {
        _jumpPhysics._heldButtonAmount = HeldButtonAmount;
        _jumpPhysics._floorPosition = FloorPosition;
        _jumpPhysics._gravity = Gravity;
        _jumpPhysics._initialVelocty = InitialVelocity;
        _jumpPhysics._initialJumpDistance = InitialJumpDistance;

        var jumpButtonPressed = Input.GetKey(KeyCode.LeftArrow);

        _jumpPhysics.JumpButtonPressed(jumpButtonPressed);
	    _jumpPhysics.Tick(Time.deltaTime);

        transform.position = transform.position.SetY(_jumpPhysics.YPos); 
	}
}

public static class Vector3Extensions
{
    public static Vector3 SetY(this Vector3 position, float val)
    {
        return new Vector3(
            position.x,
            val,
            position.z);
    }
}
