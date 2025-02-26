using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Plant
{
<<<<<<< Updated upstream
    const float TIME_TO_BURN = 0.25f;
=======
    const float TIME_TO_BURN = 0.3f;
>>>>>>> Stashed changes

    [SerializeField] private PoisonCloud cloudPrefab;
    [SerializeField] private float cloudHeightOffset;

    private SphereAreaCheck hazardCheck;
    private Coroutine burnRoutine;
    private Coroutine poisonRoutine;
    private bool burning = false;
    private float burnTime = 0;
    private int cloudCount = 0;
    private readonly List<Vector3> cloudPositions = new();

    protected override void Awake()
    {
        base.Awake();

        hazardCheck = GetComponent<SphereAreaCheck>();
    }

    private void Start()
    {
        hazardCheck.CheckArea();
        Collider[] hits = hazardCheck.GetHits();

        if (hits == null)
        {
            Debug.LogWarning("no poison cloud positions found!");
            return;
        }

        for (int i = 0; i < hits.Length; i++)
        {
            cloudPositions.Add(hits[i].transform.position);
        }
    }

    public override void Interact()
    {
        if (GrowthStage == 3 && PlayerToolManager.CurrentTool is Flashlight)
        {
            burnTime += Time.deltaTime;

            if (!burning)
                burnRoutine = StartCoroutine(IEBurn());
        }
    }

    protected override void OnGrowthStageChanged()
    {
        if (GrowthStage == 3)
            poisonRoutine = StartCoroutine(IEPoisonClouds());
        else
        {
            if (poisonRoutine != null)
            {
                StopCoroutine(poisonRoutine);
                poisonRoutine = null;
            }

            if (burnRoutine != null)
            {
                StopCoroutine(burnRoutine);
                burnRoutine = null;
            }
        }
    }

    private IEnumerator IEBurn()
    {
        burning = true;

        while (burnTime < TIME_TO_BURN && burnTime > 0)
        {
            yield return null;
        }

        if (burnTime >= TIME_TO_BURN)
            GrowthStage = 1;

        burnTime = 0;
        burning = false;
    }

    private IEnumerator IEPoisonClouds()
    {
        WaitForSeconds cloudDelay = new(6f);

        while(true)
        {
            if (cloudCount < 2)
            {
                SpawnCloud();

                yield return cloudDelay;
            }
            else
                yield return null;
        }
    }

    private void SpawnCloud()
    {
        PoisonCloud cloud = Instantiate(cloudPrefab, transform);

        Vector3 position = cloudPositions[Random.Range(0, cloudPositions.Count)];
        position += Random.insideUnitSphere * 0.25f;
        position.y += cloudHeightOffset;

        cloud.transform.position = position;

        cloudCount++;

        Invoke(nameof(DecrementCloudCount), PoisonCloud.Lifespan);
    }

    private void DecrementCloudCount()
    {
        cloudCount = Mathf.Max(0, cloudCount - 1);
    }
}
