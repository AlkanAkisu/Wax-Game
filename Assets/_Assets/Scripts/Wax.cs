using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wax : MonoBehaviour
{
    private HairCombiner _hairCombiner;


    public void WaxInit(HairCombiner hairCombiner)
    {
        _hairCombiner = hairCombiner;
    }

    public void Combine()
    {
        MainWax.Instance.CombineInto(transform);

    }

    private void OnTriggerEnter(Collider other)
    {
        var hair = other.GetComponent<Hair>();
        if (hair == null) return;

        _hairCombiner.CombineInto(hair.transform);
        Destroy(hair.gameObject);
    }
}
