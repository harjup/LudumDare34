﻿using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;


public enum Pattern
{
    Undefined,
    StraightLine,
    MakeDecision
}

public class Enemy : MonoBehaviour
{
    public Pattern Pattern;
    public GameContext.Difficulty Difficulty;

    public float HopTime = .15f;
    public float RunTime = 2f;
    public Ease HopEaseType;
    public Ease RunEaseType;

    public Target Target;

    public Vector3 InitialPosition;
    public Sequence Sequence;
    public KeyCode DebugKeyCode;

    private Shadow _shadow;

    private Vector3 _choiceSpot;

    private Animator _animator;

    private bool _done;

    public Transform GetTargetTransform(Target target)
    {
        if (Target == Target.Box)
        {
            return GameObject.Find("RightChar").transform;
        }
        if (Target == Target.Circle)
        {
            return GameObject.Find("LeftChar").transform;
        }

        return null;
    }

    void Start()
    {
        _choiceSpot = FindObjectOfType<ChoiceSpot>().transform.position;

        _animator = GetComponentInChildren<Animator>();

        var prefab = Resources.Load<GameObject>("Prefabs/Shadow");
        _shadow = Instantiate(prefab).GetComponent<Shadow>();

        var floor = FindObjectOfType<Floor>().gameObject;
        var floorTop = floor.transform.position.y + (floor.transform.lossyScale.y / 2);

        // TODO: Abstract out or something I dunno
        _shadow.SetBaseScale(new Vector3(12f, 6f, 1f));
        _shadow.SetOffset(new Vector3(0f, 0f, -0.15f));
        _shadow.SetFloorPosition(floorTop + .05f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(DebugKeyCode))
        {
            Sequence.Kill();
            DoRun(Pattern, GetTargetTransform(Target).position);
        }

        _shadow.SetDistance((transform.position.y - InitialPosition.y) + 1f);
        _shadow.UpdatePosition(transform.position);
    }

    public void DoRun(Pattern pattern, Vector3 target)
    {
        switch (pattern)
        {
            case Pattern.StraightLine:
                StraightLineRun(target);
                break;
            case Pattern.MakeDecision:
                MakeDecisionRun(target);
                break;
        }
    }


    public void StraightLineRun(Vector3 targetPosition)
    {
        var current = InitialPosition;
        var target = targetPosition.SetY(current.y);
        var direction = (target - current).normalized;

        // Let's try overshooting the player by 10 units
        var final = target + (direction * 10f);

        Sequence = DOTween
            .Sequence()
            // 2 hops
            .AppendCallback(() => _animator.SetTrigger("Hop"))
            .Append(transform.DOMoveY(current.y + 1, HopTime).SetEase(HopEaseType))
            .Append(transform.DOMoveY(current.y, HopTime).SetEase(HopEaseType))
            .Append(transform.DOMoveY(current.y + 1, HopTime).SetEase(HopEaseType))
            .Append(transform.DOMoveY(current.y, HopTime).SetEase(HopEaseType))
            .AppendCallback(() => _animator.SetTrigger("Walk"))
            // Walk towards target
            //But continue walking after passing the player
            .Append(transform.DOMove(final, RunTime).SetEase(RunEaseType))
            .AppendCallback(() => _animator.SetTrigger("Idle"))
            .AppendCallback(() => _done = true)
            .Play();
            // That's it.
    }

    public void MakeDecisionRun(Vector3 targetPosition)
    {
        var current = InitialPosition;
        var yPos = current.y;

        var target = targetPosition.SetY(yPos);
        var direction = (target - _choiceSpot.SetY(yPos)).normalized;

        // Let's try overshooting the player by 10 units
        var final = target + (direction * 10f);

        float targetRotation = 0f;
        if (Target == Target.Box)
        {
            targetRotation = -15;
        }
        if (Target == Target.Circle)
        {
            targetRotation = 15;
        }

        Sequence = DOTween
            .Sequence()
            // 2 hops
            .AppendCallback(() => _animator.SetTrigger("Walk"))
            .Append(transform.DOMove(_choiceSpot.SetY(yPos), 1f).SetEase(RunEaseType))
            .AppendCallback(() => _animator.SetTrigger("Hop"))
            .Append(transform.DOMoveY(yPos + 1, HopTime).SetEase(HopEaseType))
            .Join(transform.DOLocalRotate(new Vector3(0, 0, targetRotation), HopTime).SetEase(RunEaseType))
            .Append(transform.DOMoveY(yPos, HopTime).SetEase(HopEaseType))
            .Join(transform.DOLocalRotate(new Vector3(0, 0, 0), HopTime).SetEase(RunEaseType))
            .AppendCallback(() => _animator.SetTrigger("Walk"))
            // Walk towards target
            //But continue walking after passing the player
            .Append(transform.DOMove(final, RunTime / 2f).SetEase(RunEaseType))
            .AppendCallback(() => _animator.SetTrigger("Idle"))
            .AppendCallback(() => _done = true)
            .Play();
    }

    public Tween WalkTo(Vector3 start, Vector3 finish, float time, int index)
    {
        transform.position = start;

        _animator.SetTrigger("Walk");

        var finishActual = new Vector3(finish.x, finish.y, (finish.z - 4) + (index * 2f));
        InitialPosition = finishActual;

        return transform
            .DOMove(finishActual, time + (index / 10f))
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _animator.SetTrigger("Idle");
            });
    }

    public IEnumerator DoAttack()
    {
        _done = false;

        switch (Pattern)
        {
            case Pattern.StraightLine:
                StraightLineRun(GetTargetTransform(Target).position);
                break;
            case Pattern.MakeDecision:
                MakeDecisionRun(GetTargetTransform(Target).position);
                break;
            default:
                yield break;
        }

        // Exit once our attack is done
        while (_done == false)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void CleanUp()
    {
        Sequence.Kill();
        Destroy(_shadow.gameObject);
        Destroy(this.gameObject);
    }
}
