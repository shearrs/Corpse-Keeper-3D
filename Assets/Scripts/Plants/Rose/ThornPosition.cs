using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornPosition : MonoBehaviour
{
    [SerializeField] private Thorns thorns;

    public bool Occupied => thorns.gameObject.activeSelf;

    private void Start()
    {
        thorns = Instantiate(thorns);
        thorns.transform.parent = transform;
        thorns.transform.localPosition = Vector3.zero;
        // randomize rotation a bit maybe

        thorns.gameObject.SetActive(false);
    }

    public void GrowThorns()
    {
        if (Occupied)
            return;

        thorns.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        thorns.gameObject.SetActive(true);
    }

    public void CutThorns()
    {
        if (!Occupied)
            return;

        // particles or something
        thorns.gameObject.SetActive(false);
    }
}
