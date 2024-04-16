using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose : Plant
{
    [SerializeField] private Thorns thorns;

    public override void Interact()
    {
        GameManager.Shears--;
        GrowthStage--;
    }

    protected override void OnGrowthStageChanged()
    {
        
    }
}
