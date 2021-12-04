using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelperText : MonoBehaviour
{
    [SerializeField, TextArea(2, 5)] string waxingString;
    [SerializeField, TextArea(2, 5)] string dryingString;
    [SerializeField, TextArea(2, 5)] string pullString;
    [SerializeField, TextArea(2, 5)] string doneString;
    TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        ChangeText(waxingString);
    }

    private void ChangeText(string txt)
    {
        tmpText.text = txt;
    }

    private void OnEnable()
    {
        EventSystem.waxDryStarted += dryText;
        EventSystem.applyingWaxFinished += pullText;
        EventSystem.pullingWaxFinished += doneText;
    }
    private void OnDisable()
    {
        EventSystem.waxDryStarted -= dryText;
        EventSystem.applyingWaxFinished -= pullText;
        EventSystem.pullingWaxFinished -= doneText;
    }

    private void dryText()
    {
        ChangeText(dryingString);
    }

    private void pullText()
    {
        ChangeText(pullString);

    }

    private void doneText(Vector3 _a, float _b)
    {
        ChangeText(doneString);

    }



}
