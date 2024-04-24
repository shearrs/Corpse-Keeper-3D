using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EndScreen winScreen;
    [SerializeField] private EndScreen loseScreen;
    [SerializeField] private Cooldown winTimer;

    private void Start()
    {
        winTimer.StartTimer();

        StartCoroutine(IEGame());
    }

    private IEnumerator IEGame()
    {
        while (!winTimer.Done)
        {
            if (Corpseflower.GrowthAmount >= 100)
            {
                Lose();
                yield break;
            }

            yield return null;
        }

        Win();
    }

    private void Lose()
    {
        loseScreen.Enable();
    }

    private void Win()
    {
        winScreen.Enable();
    }
}
