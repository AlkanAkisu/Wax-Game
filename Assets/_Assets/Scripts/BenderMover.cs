using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class BenderMover : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float yOffset = -0.01f;
    [SerializeField] float threshold;
    [SerializeField] private float putDistance;
    [SerializeField] Transform midPoint;
    [SerializeField] bool canBenderMove = true;
    [SerializeField] private float mouseDistanceToReach;
    [SerializeField] private float lastThrowSpeed;
    [SerializeField] private float benderDistanceReach;

    Vector2 firstTouchPos;
    private Vector3 pullDir;
    private Vector3 benderStartPos;
    private State currentState;
    private Camera _cam;
    private float finishTime;

    private bool isTouching;
    private Vector2 touchPosition;
    private LeanFinger finger;

    enum State
    {
        Waxing,
        WaxingFinished,
        PullingInit,
        PullingStarted,
        PullingFinished,
        END
    }

    private void Awake()
    {
        canBenderMove = false;
        currentState = State.Waxing;
        _cam = Camera.main;
        isTouching = false;
    }
    private void Update()
    {

        if (isTouching && canBenderMove && isState(State.Waxing))
        {
            currentState = State.PullingInit;
        }

        if (isState(State.PullingInit) && checkMouseThreshold())
        {
            var mouseChange = getMouseDifference().normalized;
            pullDir.z = mouseChange.x;
            pullDir.y = mouseChange.y;
            pullDir = pullDir.normalized;

            EventSystem.pullingWaxStarted?.Invoke();
            currentState = State.PullingStarted;
            PutBenderInPos(pullDir);
        }

        if (isState(State.PullingStarted))
        {
            if (isTouching)
                MoveBender();
            else
            {
                EventSystem.pullingWaxFinished?.Invoke(pullDir, lastThrowSpeed);
                currentState = State.PullingFinished;
            }
        }
        if (isState(State.PullingFinished) && finishTime > Time.time)
        {
            transform.position += pullDir * lastThrowSpeed * Time.deltaTime;
        }
        else if (isState(State.PullingFinished))
        {

            currentState = State.END;
        }
        else
        {
            finishTime = Time.time + 1.5f;
        }

    }

    private void PutBenderInPos(Vector3 pullDir)
    {
        var pos = midPoint.position - pullDir * putDistance;
        pos.x = transform.position.x;
        transform.position = pos;

        benderStartPos = transform.position;

        transform.up = pullDir;
    }

    private Vector2 getMouseDifference() => finger.LastScreenPosition - finger.StartScreenPosition;
    private float getMouseDistance() => Vector2.SqrMagnitude(getMouseDifference());
    private bool checkMouseThreshold() => getMouseDistance() > threshold;


    private void MoveBender()
    {
        var percentage = _cam.ScreenToViewportPoint(getMouseDifference()).sqrMagnitude / Mathf.Pow(mouseDistanceToReach, 2);
        if (percentage < 1 && isTouching)
            transform.position = benderStartPos + (percentage * benderDistanceReach) * pullDir;
        else
        {
            EventSystem.pullingWaxFinished?.Invoke(pullDir, lastThrowSpeed);
            currentState = State.PullingFinished;
        }

    }

    private void OnEnable()
    {
        EventSystem.applyingWaxFinished += waxFinished;
        LeanTouch.OnFingerDown += SetIsTouching;
        LeanTouch.OnFingerUp += SetIsTouching;
    }
    private void OnDisable()
    {
        EventSystem.applyingWaxFinished -= waxFinished;
        LeanTouch.OnFingerDown -= SetIsTouching;
        LeanTouch.OnFingerUp -= SetIsTouching;
    }

    private void SetIsTouching(LeanFinger finger)
    {

        if (finger.Up)
            isTouching = false;
        else if (finger.Down)
            isTouching = true;

        this.finger = finger;

    }

    private void waxFinished()
    {
        canBenderMove = true;
    }

    private bool isState(State state) => currentState == state;
}
