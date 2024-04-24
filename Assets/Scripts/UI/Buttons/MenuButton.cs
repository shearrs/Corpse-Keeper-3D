using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MenuButton : Button2D
    {
        [SerializeField] private GameObject playerCamera;

        public override void OnClicked()
        {
            Disable();
            Loader.LoadUnLoad("MainMenu", "Game");
            MenuCamera.Instance.gameObject.SetActive(true);
            playerCamera.SetActive(false);
        }
    }
}