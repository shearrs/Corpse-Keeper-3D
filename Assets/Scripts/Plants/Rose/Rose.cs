using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose : Plant
{
    [SerializeField] private float thornDetectionRadius = 5.5f;
    private LayerMask thornMask;
    private readonly List<ThornPosition> thornPositions = new();

    private void Start()
    {
        thornMask = LayerMask.NameToLayer("ThornPosition");
        Collider[] hits = Physics.OverlapSphere(transform.position, thornDetectionRadius, thornMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].gameObject.TryGetComponent(out ThornPosition position))
            {
                thornPositions.Add(position);
            }
        }
    }

    public override void Interact()
    {
        if (PlayerToolManager.CurrentTool is not Shears || GrowthStage != 3)
            return;

        GrowthStage = 1;
    }

    protected override void OnGrowthStageChanged()
    {
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, thornDetectionRadius);
    }
}
