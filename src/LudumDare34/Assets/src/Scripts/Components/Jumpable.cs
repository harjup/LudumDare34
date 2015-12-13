using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

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

    private Animator _animator;

    private JumpPhysics _jumpPhysics;

    private Shadow _shadow;

    private bool _dead;
    private bool _walking = true;
  
    private PlayerStats _playerStats;

	void Start()
	{
	    var floor = FindObjectOfType<Floor>().gameObject;
	    var floorTop = floor.transform.position.y + (floor.transform.lossyScale.y / 2);
	    var halfCharacterHeight = GetComponentInChildren<BoxCollider>().transform.lossyScale.y/2;
	    FloorPosition = halfCharacterHeight + floorTop;

        _jumpPhysics = new JumpPhysics(FloorPosition, InitialJumpDistance, Gravity, InitialVelocity);

        _animator = GetComponentInChildren<Animator>();

        var prefab = Resources.Load<GameObject>("Prefabs/Shadow");
        _shadow = Instantiate(prefab).GetComponent<Shadow>();

        // TODO: Abstract out or something I dunno
        _shadow.SetBaseScale(new Vector3(12f, 6f, 1f)); 
        _shadow.SetOffset(new Vector3(0f, 0f, -0.15f));
        _shadow.SetFloorPosition(floorTop + .05f);

        _playerStats = gameObject.GetComponentInParent<PlayerStats>();
	}

    void Update()
    {
        _animator.SetBool("NoHitPoints", _dead);

        _shadow.SetDistance(_jumpPhysics.YPos);
        _shadow.UpdatePosition(transform.position);

        if (_dead || _walking)
        {
            return;
        }

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

        _animator.SetBool("GoingUp", _jumpPhysics.Velocity > 0);
        _animator.SetBool("GoingDown", _jumpPhysics.Velocity < 0);
        _animator.SetBool("Landed", _jumpPhysics.CurrentState == JumpPhysics.State.Ground);
	}

    public Tween TweenFromStartToFinish(Vector3 start, Vector3 finish, float time)
    {
        transform.position = start;

        return transform
            .DOMove(finish, time)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => { 
                _walking = false; 
                _animator.SetTrigger("Stopped"); 
            });
    }


    public Sequence TweenWalkRight(float time)
    {
        _walking = true;

        var yPos = FloorPosition;
        transform.position = transform.position.SetY(FloorPosition);


        return DOTween
            .Sequence()
            // TODO: I'm sorry
            // Hop twice...
            .AppendCallback(() => _animator.SetBool("GoingUp", true))
            .AppendCallback(() => _animator.SetBool("GoingDown", false))
            .Append(transform.DOLocalMoveY(yPos + 1, .15f).SetEase(Ease.OutCirc))
            .AppendCallback(() => _animator.SetBool("GoingUp", false))
            .AppendCallback(() => _animator.SetBool("GoingDown", true))
            .Append(transform.DOLocalMoveY(yPos, .15f).SetEase(Ease.InCirc))
            .AppendCallback(() => _animator.SetBool("GoingDown", false))
            .AppendCallback(() => _animator.SetBool("GoingUp", true))
            .Append(transform.DOLocalMoveY(yPos + 1, .15f).SetEase(Ease.OutCirc))
            .AppendCallback(() => _animator.SetBool("GoingUp", false))
            .AppendCallback(() => _animator.SetBool("GoingDown", true))
            .Append(transform.DOLocalMoveY(yPos, .15f).SetEase(Ease.InCirc))
            .AppendCallback(() => _animator.SetBool("GoingDown", false))
            .AppendCallback(() => _animator.SetTrigger("Walking"))
            .Append(transform.DOMoveX(30, time).SetEase(Ease.InSine));
    }

    public void GotHit()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("CicleGuyJump-Down")
            || _animator.GetCurrentAnimatorStateInfo(0).IsName("BoxGuyJump-Down"))
        {
            _jumpPhysics.Bounce();
            return;
        }
            
        _playerStats.TakeHit(1);
        _animator.SetTrigger("GotHit");
    }

    public void Dead()
    {
        _dead = true;  
    }
}