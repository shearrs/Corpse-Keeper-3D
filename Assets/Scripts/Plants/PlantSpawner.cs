using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    private const float POSITION_OFFSET = 0.2f;

    [Header("Plants")]
    [SerializeField] private Rose rosePrefab;
    [SerializeField] private Mushroom mushroomPrefab;
    [SerializeField] private Flytrap flytrapPrefab;

    [Header("Chances")]
    [SerializeField] private Vector2Int roseRange;
    [SerializeField] private Vector2Int mushroomRange;
    [SerializeField] private Vector2Int flytrapRange;

    [Header("Positions")]
    [SerializeField] private Transform[] positions;

    private void Start()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            Transform position = positions[i];
            SpawnPlant(position.position);
        }
    }

    // 0-5  - rose
    // 6-8 - mushroom
    // 9-11 - flytrap
    private void SpawnPlant(Vector3 position)
    {
        int random = Random.Range(0, flytrapRange.y);

        Plant plant;

        if (random < roseRange.y)
            plant = rosePrefab;
        else if (random < mushroomRange.y)
            plant = mushroomPrefab;
        else
            plant = flytrapPrefab;

        plant = Instantiate(plant, transform);

        Vector3 targetPosition = position;
        targetPosition.y += POSITION_OFFSET;
        plant.transform.position = targetPosition;
    }
}
