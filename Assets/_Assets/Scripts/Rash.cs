using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rash : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem.pullingWaxFinished += EnableRash;
    }
    private void OnDisable()
    {
        EventSystem.pullingWaxFinished -= EnableRash;
    }


    private void EnableRash(Vector3 _a, float _b)
    {
        GetComponent<Animator>().enabled = true;
    }
}
