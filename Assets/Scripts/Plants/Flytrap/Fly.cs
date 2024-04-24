using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour, IInteractable
{
    [SerializeField] private float timeToFly;
    private AudioSource audioSource;

    public int CurrentPosition { get; set; }
    public Flytrap Flytrap { get; set; }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

                float pitch = audioSource.pitch;
                pitch += Random.Range(-0.01f, 0.01f);
                pitch = Mathf.Clamp(pitch, 0.9f, 1.25f);
                audioSource.pitch = pitch;

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
