using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : Button3D
{
    public override void OnClicked()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}