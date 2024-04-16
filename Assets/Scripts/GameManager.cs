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
            Instance.shears = Mathf.Max(0, value);
            OnShearsChanged?.Invoke();
        }
    }
    public static int Water 
    { 
        get => Instance.water; 
        set
        {
            Instance.water = Mathf.Max(0, value);
            OnWaterChanged?.Invoke();
        }
    }
    public static int Fertilizer 
    { 
        get => Instance.fertilizer; 
        set
        {  
            Instance.fertilizer = Mathf.Max(0, value);
            OnFertilizerChanged?.Invoke();
        }
    }

    private int shears;
    private int water;
    private int fertilizer;
}