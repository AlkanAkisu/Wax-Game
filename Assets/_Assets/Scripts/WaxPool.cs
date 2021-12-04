using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaxPool : MonoBehaviour
{
    [SerializeField] List<Transform> waxPool;
    [SerializeField] HairCombiner _hairCombiner;

    public Transform GetWax()
    {
        var waxTr = waxPool[0];
        Debug.Log($"waxTr null? {waxTr == null}");
        waxPool.RemoveAt(0);
        waxTr.gameObject.SetActive(true);

        var wax = waxTr.GetComponent<Wax>();
        wax.WaxInit(_hairCombiner);
        StartCoroutine(Combine(wax));
        return waxTr;
    }

    public void AddWax(Transform wax)
    {
        waxPool.Add(wax);
        wax.gameObject.SetActive(false);
    }

    IEnumerator Combine(Wax wax)
    {
        yield return new WaitForSeconds(0.1f);
        wax.Combine();
        AddWax(wax.transform);

    }

    

}
