using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EndScreen winScreen;
    [SerializeField] private EndScreen loseScreen;
    private bool allPlantsCut = false;

    public static bool AllPlantsCut { get => Instance.allPlantsCut; set => Instance.allPlantsCut = value; }

    private void Start()
    {
        StartCoroutine(IEGame());
    }

    private IEnumerator IEGame()
    {
        while (!AllPlantsCut)
        {
<<<<<<< Updated upstream
            if (Corpseflower.GrowthAmount >= 120)
=======
            if (Corpseflower.GrowthStage == 3 && Corpseflower.GrowthAmount >= 100)
>>>>>>> Stashed changes
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
