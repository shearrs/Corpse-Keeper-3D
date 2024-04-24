using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose : Plant
{
    [SerializeField] private Cooldown thornsCooldown;
    private SphereAreaCheck hazardCheck;
    private readonly List<ThornPosition> thornPositions = new();

    protected override void Awake()
    {
        base.Awake();

        hazardCheck = GetComponent<SphereAreaCheck>();
    }

    private void Start()
    {
        hazardCheck.CheckArea();
        Collider[] hits = hazardCheck.GetHits();

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
        StopAllCoroutines();

        if (GrowthStage == 3)
            StartCoroutine(IEThorns());
    }

    private IEnumerator IEThorns()
    {
        while(true)
        {
            SpawnThorns();
            thornsCooldown.StartTimer();

            while (!thornsCooldown.Done)
                yield return null;

            yield return null;
        }
    }

    private void SpawnThorns()
    {
        for (int i = 0; i < thornPositions.Count; i++)
        {
            ThornPosition position = thornPositions[i];

            if (position.Occupied)
                continue;
            else
            {
                position.GrowThorns();
                break;
            }
        }
    }
}
