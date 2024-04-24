using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : MonoBehaviour, IInteractable
{
    const float GROWTH_THRESHOLD = 10;
    const float CORPSE_GROWTH_TIME = 5f;

    [Header("Stages")]
    [SerializeField] private GameObject stage1;
    [SerializeField] private GameObject stage2;
    [SerializeField] private GameObject stage3;
    private GameObject currentStage;
    private Coroutine growthRoutine;
    private Coroutine corpseRoutine;

    [Header("Stats")]
    [SerializeField] private int growthRate;
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
        growthRoutine = StartCoroutine(IEGrowth());
    }

    protected virtual void AddGrowthAmount()
    {
        if (GrowthStage == 3)
            return;

        int random = Random.Range(0, 5);

        if (random > 0)
            return;

        growthAmount += growthRate;
        
        if (growthAmount >= GROWTH_THRESHOLD)
        {
            growthAmount -= GROWTH_THRESHOLD;

            GrowthStage++;
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

        if (GrowthStage == 3)
        {
            if (growthRoutine != null)
                StopCoroutine(growthRoutine);

            corpseRoutine ??= StartCoroutine(IECorpseGrowth());
        }
        else
        {
            if (corpseRoutine != null)
                StopCoroutine(corpseRoutine);

            growthRoutine ??= StartCoroutine(IEGrowth());
        }

        OnGrowthStageChanged();
    }
    
    private IEnumerator IEGrowth()
    {
        while(true)
        {
            float elapsedTime = 0;

            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            AddGrowthAmount();

            yield return null;
        }
    }

    private IEnumerator IECorpseGrowth()
    {
        while(true)
        {
            float elapsedTime = 0;

            while(elapsedTime < CORPSE_GROWTH_TIME)
            {
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            Corpseflower.GrowthAmount += 1;

            yield return null;
        }
    }

    protected abstract void OnGrowthStageChanged();

    public abstract void Interact();
}