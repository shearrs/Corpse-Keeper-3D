using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPositions : Singleton<FlyPositions>
{
    [SerializeField] private Fly flyPrefab;
    [SerializeField] private Transform[] positions;

    private static Transform[] Positions => Instance.positions;

    public static Fly SpawnFly()
    {
        int index = Random.Range(0, Positions.Length);
        Vector3 position = Positions[index].position;

        Fly fly = Instantiate(Instance.flyPrefab, Instance.transform);
        fly.transform.position = position;

        fly.CurrentPosition = index;

        return fly;
    }

    public static Vector3 GetNextPosition(int current)
    {
        int index;

        do
        {
            index = Random.Range(0, Positions.Length);
        } while (index == current);

        return Positions[index].position;
    }
}