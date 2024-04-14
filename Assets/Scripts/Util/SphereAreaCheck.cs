using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAreaCheck : MonoBehaviour
{
    [Header("Gizmos")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private Color gizmosColor = Color.red;

    [Header("Detection")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float radius = .4f;
    [SerializeField] private Vector3 originOffset = Vector3.zero;

    public Vector3 OriginOffset { get => originOffset; set => originOffset = value; }

    private const int MAX_COLLISIONS = 10;
    private int hits;
    private readonly Collider[] collisions = new Collider[MAX_COLLISIONS];

    /// <summary>
    /// Checks the area for overlapping colliders and stores them. Retrieve hits with GetHit() or GetHits().
    /// </summary>
    public int CheckArea()
    {
        hits = Physics.OverlapSphereNonAlloc(transform.TransformPoint(originOffset), radius, collisions, layerMask);

        return hits;
    }

    /// <summary>
    /// Returns the first hit found.
    /// </summary>
    public Collider GetHit()
    {
        return collisions[0];
    }

    /// <summary>
    /// Returns all hits found.
    /// </summary>
    public Collider[] GetHits()
    {
        if (hits == 0)
            return null;

        Collider[] subArr = new Collider[hits];

        for(int i = 0; i < hits; i++)
            subArr[i] = collisions[i];

        return subArr;
    }

    public void OnDrawGizmosSelected()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = gizmosColor;
        Gizmos.DrawWireSphere(transform.TransformPoint(originOffset), radius);
    }
}