using UnityEngine;
using System;
using System.Collections.Generic;

public class MainWax : MonoBehaviour
{
    [SerializeField] private Color dryColor;
    [SerializeField] private float waxDryTotalTime = 1.5f;


    private Color defaultColor;
    private MeshFilter _meshFilter;
    private MegaModifyObject _modifyObject;
    private Renderer _renderer;
    private Vector3 dir;
    private float speed;
    private bool isPullingFinished;

    private float finishTime;
    private bool isWaxDrying;
    private float waxDryStartTime;

    private static MainWax _instance;
    public static MainWax Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _modifyObject = GetComponent<MegaModifyObject>();
        _renderer = GetComponent<Renderer>();
        _modifyObject.enabled = false;
        _modifyObject.Enabled = false;

        isWaxDrying = false;
        defaultColor = _renderer.material.color;

        


    }

    private void Update()
    {
        if (isPullingFinished && finishTime > Time.time)
        {
            transform.position += dir * speed * Time.deltaTime;
        }
        else if (isPullingFinished)
        {
            isPullingFinished = false;
        }
        else
        {
            finishTime = Time.time + 1.5f;
        }

        if (isWaxDrying)
            WaxDry();
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

        if (!_modifyObject.enabled)
        {
            _modifyObject.enabled = true;
            _modifyObject.Enabled = true;
        }
        _modifyObject.MeshHasBeenChanged();

    }

    private void OnEnable()
    {
        EventSystem.pullingWaxFinished += PullingFinished;

        EventSystem.waxDryStarted += startWaxDrying;
    }
    private void OnDisable()
    {
        EventSystem.pullingWaxFinished -= PullingFinished;
        EventSystem.waxDryStarted -= startWaxDrying;
    }

    private void startWaxDrying()
    {
        isWaxDrying = true;
        waxDryStartTime = Time.time;
    }
    private void WaxDry()
    {
        var t = (Time.time - waxDryStartTime) / waxDryTotalTime;
        if (t < 1)
            _renderer.material.color = Color.Lerp(defaultColor, dryColor, t);
        else
        {
            isWaxDrying = false;
            EventSystem.applyingWaxFinished?.Invoke();
        }

    }

    private void PullingFinished(Vector3 dir, float speed)
    {
        this.dir = dir;
        this.speed = speed;
        isPullingFinished = true;
    }

}