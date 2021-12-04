using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class HairCombiner : MonoBehaviour
{

    private Hair[] hairs;
    private bool isCombined;
    private bool isWaxingStarted;
    private MeshFilter _meshFilter;
    private bool isTouching;

    private static HairCombiner _instance;
    public static HairCombiner Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
        hairs = FindObjectsOfType<Hair>();
        _meshFilter = GetComponent<MeshFilter>();
        isCombined = false;
        isWaxingStarted = false;
    }

    private void Update()
    {
        if (isTouching)
        {
            isWaxingStarted = true;
        }

        if (!isTouching && !isCombined && isWaxingStarted)
        {
            MainWax.Instance.CombineInto(transform);
            isCombined = true;
            Destroy(gameObject);
        }
    }

    public void CombineInto(Transform meshObject)
    {
        var mFilter1 = GetComponent<MeshFilter>();
        var mFilter2 = meshObject.GetComponent<MeshFilter>();

        MeshFilter[] meshFilters = new MeshFilter[] { mFilter1, mFilter2 };
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }
        var mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.CombineMeshes(combine);
        _meshFilter.sharedMesh = mesh;

        transform.gameObject.SetActive(true);
        // meshObject.gameObject.SetActive(false);

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
