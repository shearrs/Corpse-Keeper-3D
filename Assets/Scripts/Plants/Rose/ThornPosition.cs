using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornPosition : MonoBehaviour
{
    [SerializeField] private GameObject thorns;

    public bool Occupied => thorns.activeSelf;

    private void Awake()
    {
        thorns = Instantiate(thorns, transform);
        // randomize rotation a bit maybe

        thorns.SetActive(false);
    }

    public void GrowThorns()
    {
        if (Occupied)
            return;

        thorns.SetActive(true);
    }

    public void CutThorns()
    {
        if (!Occupied)
            return;

        // particles or something
        thorns.SetActive(false);
    }
}