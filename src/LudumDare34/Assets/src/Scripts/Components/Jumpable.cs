using System;
using UnityEngine;
using System.Collections.Generic;

public enum JumpButton
{
    Undefined,
    LeftArrow,
    RightArrow
}

public class Jumpable : MonoBehaviour
{
    // TODO: Calculate
    public float FloorPosition;

    public float Gravity = 50f;
    public float InitialJumpDistance = .5f;
    public float InitialVelocity = 15f;
    public float HeldButtonAmount = 15f;

    public JumpButton Button;

    private JumpPhysics _jumpPhysics;

    private Shadow _shadow;
    

	void Start ()
	{
	    var floor = FindObjectOfType<Floor>().gameObject;
	    var floorTop = floor.transform.position.y + (floor.transform.lossyScale.y / 2);
	    var halfCharacterHeight = GetComponentInChildren<BoxCollider>().transform.lossyScale.y/2;
	    FloorPosition = halfCharacterHeight + floorTop;

        _jumpPhysics = new JumpPhysics(FloorPosition, InitialJumpDistance, Gravity, InitialVelocity);

        var prefab = Resources.Load<GameObject>("Prefabs/Shadow");
        _shadow = Instantiate(prefab).GetComponent<Shadow>();

        // TODO: Abstract out or something I dunno
        _shadow.SetBaseScale(new Vector3(12f, 6f, 1f)); 
        _shadow.SetOffset(new Vector3(0f, 0f, -0.15f));
        _shadow.SetFloorPosition(floorTop + .05f);

	}

    void Update()
    {
        // Set parameters we wanna tweak
        _jumpPhysics._heldButtonAmount = HeldButtonAmount;
        _jumpPhysics._floorPosition = FloorPosition;
        _jumpPhysics._gravity = Gravity;
        _jumpPhysics._initialVelocty = InitialVelocity;
        _jumpPhysics._initialJumpDistance = InitialJumpDistance;

        var jumpButtonPressed = Input.GetKey(Button.ToKeyCode());

        _jumpPhysics.JumpButtonPressed(jumpButtonPressed);
	    _jumpPhysics.Tick(Time.deltaTime);

        transform.position = transform.position.SetY(_jumpPhysics.YPos);

        _shadow.SetDistance(_jumpPhysics.YPos);
        _shadow.UpdatePosition(transform.position);
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
