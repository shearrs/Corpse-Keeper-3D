using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour, IInteractable
{
    [SerializeField] private float flySpeed;

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

            while (Vector3.Distance(transform.position, target) > float.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);

                yield return null;
            }
        }
    }

    private void Die()
    {
        Flytrap.FlyCount--;
        Destroy(gameObject);
    }
}
