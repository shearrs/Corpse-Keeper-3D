using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flytrap : Plant
{
    private Coroutine flyRoutine;

    public int FlyCount { get; set; }

    public override void Interact()
    {
        Debug.Log("interacted");

        if (GrowthStage == 3 && PlayerToolManager.CurrentTool is Hands && PlayerToolManager.IsHolding)
        {
            StopCoroutine(flyRoutine);
            PlayerToolManager.IsHolding = false;
            GrowthStage = 1;
        }
    }

    protected override void OnGrowthStageChanged()
    {
        if (GrowthStage == 3 && flyRoutine == null)
            flyRoutine = StartCoroutine(IEFlies());
        else if (GrowthStage < 3 && flyRoutine != null)
        {
            StopCoroutine(flyRoutine);
            flyRoutine = null;
        }
    }

    private IEnumerator IEFlies()
    {
        WaitForSeconds flyDelay = new(5f);

        while (true)
        {
            if (FlyCount < 1)
            {
                SpawnFly();

                yield return flyDelay;
            }
            else
                yield return null;
        }
    }

    private void SpawnFly()
    {
        Fly fly = FlyPositions.SpawnFly();
        fly.Flytrap = this;

        FlyCount++;
    }
}