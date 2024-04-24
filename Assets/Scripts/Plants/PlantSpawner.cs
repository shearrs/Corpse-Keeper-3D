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
        int random = Random.Range(0, 12);

        Plant plant;

        if (random < 6)
            plant = rosePrefab;
        else if (random < 9)
            plant = mushroomPrefab;
        else
            plant = flytrapPrefab;

        plant = Instantiate(plant, transform);

        Vector3 targetPosition = position;
        targetPosition.y += POSITION_OFFSET;
        plant.transform.position = targetPosition;
    }
}
