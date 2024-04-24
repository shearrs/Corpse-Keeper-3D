using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (PlayerToolManager.CurrentTool is not Shears)
            return;

        AudioManager.PlaySound(AudioManager.PlantHurtSound, 0.85f, 1f, 0.8f);
        gameObject.SetActive(false);
    }
}
