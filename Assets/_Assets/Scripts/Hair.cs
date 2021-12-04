using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hair : MonoBehaviour
{
    private bool willCombine;

    public bool WillCombine { get => willCombine; set => willCombine = value; }

    private void Start()
    {
        willCombine = false;
    }

    public void Combine()
    {
       
        MainWax.Instance.CombineInto(transform);
    }



}
