using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class WaxPlacer : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject waxObject;
    [SerializeField] float threshold;
    [SerializeField] Transform _parentWax;
    [SerializeField] WaxPool _waxPool;
    Vector3 lastMousePos;
    bool waxFinished;
    bool waxStarted;
    private bool isTouching;

    private void Awake()
    {

        waxFinished = false;
        waxStarted = false;

    }

    private void Update()
    {
        if (waxFinished)
            return;
        if (isTouching && checkMouseThreshold())
        {
            ShootRaycast();
            lastMousePos = Input.mousePosition;
            waxStarted = true;
        }

        if (!isTouching && waxStarted)
        {
            EventSystem.waxDryStarted?.Invoke();
            waxFinished = true;
        }
    }

    private bool checkMouseThreshold() => Vector3.SqrMagnitude(Input.mousePosition - lastMousePos) > threshold;

    private void ShootRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            InstWax(hit.transform.parent.TransformPoint(hit.point));
        }
    }

    private void InstWax(Vector3 pos)
    {
        var transformWax = _waxPool.GetWax();
        transformWax.position = pos;
        // Instantiate(waxObject, pos, Quaternion.Euler(13.665f, 7.552f, 106.184f), _parentWax);
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += SetIsTouching;
        LeanTouch.OnFingerUp += SetIsTouching;
    }
    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= SetIsTouching;
        LeanTouch.OnFingerUp -= SetIsTouching;
    }

    private void SetIsTouching(LeanFinger finger)
    {

        if (finger.Up)
            isTouching = false;
        else if (finger.Down)
        {
            isTouching = true;
        }
    }
}
