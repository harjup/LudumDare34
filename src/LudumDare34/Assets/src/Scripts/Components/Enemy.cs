﻿using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.UI;


public enum Pattern
{
    Undefined,
    StraightLine,
    MakeDecision
}

public class Enemy : MonoBehaviour
{
    public Pattern Pattern;

    public float HopTime = .15f;
    public float RunTime = 2f;
    public Ease HopEaseType;
    public Ease RunEaseType;

    public Transform TargetTransform;

    public Vector3 InitialPosition;
    public Sequence Sequence;
    public KeyCode DebugKeyCode;

    private Shadow _shadow;

    private Vector3 _choiceSpot;

    void Start()
    {
        InitialPosition = transform.position;

        _choiceSpot = FindObjectOfType<ChoiceSpot>().transform.position;

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
            transform.position = InitialPosition;
            Sequence.Kill();
            DoRun(Pattern, TargetTransform.position);
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
            .Append(transform.DOMoveY(current.y + 1, HopTime).SetEase(HopEaseType))
            .Append(transform.DOMoveY(current.y, HopTime).SetEase(HopEaseType))
            .Append(transform.DOMoveY(current.y + 1, HopTime).SetEase(HopEaseType))
            .Append(transform.DOMoveY(current.y, HopTime).SetEase(HopEaseType))
            // Walk towards target
            //But continue walking after passing the player
            .Append(transform.DOMove(final, RunTime).SetEase(RunEaseType))
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

        Sequence = DOTween
            .Sequence()
            // 2 hops
            .Append(transform.DOMove(_choiceSpot.SetY(yPos), 1f).SetEase(RunEaseType))
            .Append(transform.DOMoveY(yPos + 1, HopTime).SetEase(HopEaseType))
            .Append(transform.DOMoveY(yPos, HopTime).SetEase(HopEaseType))
            // Walk towards target
            //But continue walking after passing the player
            .Append(transform.DOMove(final, RunTime / 2f).SetEase(RunEaseType))
            .Play();
    }
}
