using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour, IInteractable
{
    [SerializeField] private float timeToFly;

    public int CurrentPosition { get; set; }
    public Flytrap Flytrap { get; set; }

    private void Start()
    {
        StartCoroutine(IEFly());
    }

    private void Update()
    {
        if (Flytrap.GrowthStage != 3)
            Die();
    }

    public void Interact()
    {
        if (PlayerToolManager.CurrentTool is Hands && !PlayerToolManager.IsHolding)
        {
            PlayerToolManager.IsHolding = true;
            Die();
        }
    }

    private IEnumerator IEFly()
    {
        while(true)
        {
            Vector3 target = FlyPositions.GetNextPosition(CurrentPosition);

            float elapsedTime = 0;
            Vector3 start = transform.position;

            while(elapsedTime < timeToFly)
            {
                transform.position = Vector3.Lerp(start, target, elapsedTime / timeToFly);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = target;
        }
    }

    private void Die()
    {
        Flytrap.FlyCount--;
        Destroy(gameObject);
    }
}
