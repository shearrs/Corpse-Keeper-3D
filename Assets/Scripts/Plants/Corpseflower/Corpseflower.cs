using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpseflower : Singleton<Corpseflower>
{
    [SerializeField] private GameObject stage1;
    [SerializeField] private GameObject stage2;
    [SerializeField] private GameObject stage3;

    private GameObject currentStage;
    private int growthStage;
    private float growthAmount;

    public static int GrowthStage { get => Instance.growthStage; private set => Instance.UpdateGrowthStage(value); }
    public static float GrowthAmount { get => Instance.growthAmount; set => Instance.SetGrowthAmount(value); }

    protected override void Awake()
    {
        base.Awake();

        GrowthStage = 1;
    }

    private void SetGrowthAmount(float value)
    {
        if (GrowthStage == 3)
            return;

        growthAmount = value;

        if (growthStage == 1 && growthAmount >= 25)
        {
            GrowthStage = 2;
            growthAmount -= 25;
        }
        else if (growthStage == 2 && growthAmount >= 50)
        {
            GrowthStage = 3;
            growthAmount -= 50;
        }
    }

    private void UpdateGrowthStage(int value)
    {
        growthStage = value;

        if (currentStage != null)
            currentStage.SetActive(false);

        if (growthStage == 1)
            currentStage = stage1;
        else if (growthStage == 2)
            currentStage = stage2;
        else if (growthStage == 3)
            currentStage = stage3;

        currentStage.SetActive(true);
    }
}
