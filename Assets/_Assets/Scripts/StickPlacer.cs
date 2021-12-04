using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class StickPlacer : MonoBehaviour
{
    [SerializeField] Transform stick;
    [SerializeField] Vector3 offset;
    private Camera cam;
    private Vector3 scale;
    private bool isStickDisable;
    private bool isTouching;

    private void Awake()
    {
        cam = Camera.main;
        scale = transform.localScale;
        isStickDisable = false;

        Hide();
    }
    void Update()
    {
        if (isTouching && !isStickDisable)
        {
            Show();
            var pos = cam.ScreenToWorldPoint(Input.mousePosition);
            PlaceStick(pos);
        }
        else
        {
            Hide();
        }
    }

    private void PlaceStick(Vector3 pos)
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            stick.transform.position = hit.point + offset;
        }
    }

    private void Hide()
    {
        transform.localScale = Vector3.zero;
    }
    private void Show()
    {
        transform.localScale = scale;
    }


    private void OnEnable()
    {
        EventSystem.waxDryStarted += DisableStick;
        LeanTouch.OnFingerDown += SetIsTouching;
        LeanTouch.OnFingerUp += SetIsTouching;
    }
    private void OnDisable()
    {
        EventSystem.waxDryStarted -= DisableStick;
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

    private void DisableStick()
    {
        isStickDisable = true;
    }


}
