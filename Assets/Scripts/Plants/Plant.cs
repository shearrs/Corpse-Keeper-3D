using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : MonoBehaviour, IInteractable
{
    const float GROWTH_THRESHOLD = 10;

    [Header("Stages")]
    [SerializeField] private GameObject stage1;
    [SerializeField] private GameObject stage2;
    [SerializeField] private GameObject stage3;
    private GameObject currentStage;

    [Header("Stats")]
    [SerializeField] private float growthRate;
    private float growthAmount = 0;
    private int growthStage = 1;

    protected int GrowthStage { 
        get
        {
            return growthStage;
        }
        set
        {
            growthStage = value;
            growthStage = Mathf.Clamp(growthStage, 1, 3);

            UpdateGrowthStage();
        } 
    }

    protected virtual void Awake()
    {
        currentStage = stage1;
        currentStage.SetActive(true);
    }

    protected virtual void Update()
    {
        AddGrowthAmount();
    }

    protected virtual void AddGrowthAmount()
    {
        growthAmount += growthRate * Time.deltaTime;
        
        if (growthAmount > GROWTH_THRESHOLD)
        {
            GrowthStage++;

            growthAmount -= GROWTH_THRESHOLD;
        }
    }

    private void UpdateGrowthStage()
    {
        GameObject newStage = null;

        switch(GrowthStage)
        {
            case 1:
                newStage = stage1;
                break;
            case 2:
                newStage = stage2;
                break;
            case 3:
                newStage = stage3;
                break;
        }

        if (newStage == currentStage)
            return;

        currentStage.SetActive(false);
        currentStage = newStage;
        currentStage.SetActive(true);

        OnGrowthStageChanged();
    }
    protected abstract void OnGrowthStageChanged();

    public abstract void Interact();
}