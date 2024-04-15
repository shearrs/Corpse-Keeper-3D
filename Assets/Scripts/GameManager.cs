using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Action OnShearsChanged;
    public static Action OnWaterChanged;
    public static Action OnFertilizerChanged;

    public static int Shears 
    { 
        get => Instance.shears; 
        set
        {
            Instance.shears = value;
            OnShearsChanged?.Invoke();
        }
    }
    public static int Water 
    { 
        get => Instance.water; 
        set
        {
            Instance.water = value;
            OnWaterChanged?.Invoke();
        }
    }
    public static int Fertilizer 
    { 
        get => Instance.fertilizer; 
        set
        {  
            Instance.fertilizer = value; 
            OnFertilizerChanged?.Invoke();
        }
    }

    private int shears;
    private int water;
    private int fertilizer;
}