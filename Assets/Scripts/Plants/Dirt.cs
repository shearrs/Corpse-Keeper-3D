using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    private Plant plant;

    public Plant Plant { get => plant; set => SetPlant(value); }

    private void SetPlant(Plant plant)
    {
        this.plant = plant;
    }

}
