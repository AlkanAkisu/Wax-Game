using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem
{
    public static Action applyingWaxStarted = delegate { };
    public static Action waxDryStarted = delegate { };
    public static Action applyingWaxFinished = delegate { };
    public static Action pullingWaxStarted = delegate { };
    public static Action<Vector3, float> pullingWaxFinished = delegate { };
    public static Action retryGame = delegate { };


}
