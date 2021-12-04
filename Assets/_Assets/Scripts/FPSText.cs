using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSText : MonoBehaviour
{
    TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        ChangeText(30);
    }

    private void ChangeText(float fps)
    {
        float fc = (float)Mathf.Round(fps * 100f) / 100f;
        tmpText.text = "FPS: " + fc;
    }

    private void Update()
    {
        ChangeText(1.0f / Time.deltaTime);
    }
}
