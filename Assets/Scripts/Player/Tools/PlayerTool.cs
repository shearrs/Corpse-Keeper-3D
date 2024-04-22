using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTool : MonoBehaviour
{
    public bool IsBeingUsed { get; protected set; }

    public abstract void Enable();
    public abstract void Disable();

    public abstract void Use();
}