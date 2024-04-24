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

            Flytrap flytrap = Instantiate(flytrapPrefab, transform);

            Vector3 targetPosition = position.position;
            targetPosition.y += POSITION_OFFSET;
            flytrap.transform.position = targetPosition;
        }
    }
}
