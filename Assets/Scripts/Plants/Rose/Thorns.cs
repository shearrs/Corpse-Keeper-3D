using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        AudioManager.PlaySound(AudioManager.PlantHurtSound, 0.85f, 1f, 0.8f);
        Destroy(gameObject);
    }
}
