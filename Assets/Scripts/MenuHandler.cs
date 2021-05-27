using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject navBarHandler;

    public void ToggleMainMenu()
    {
        if (MainMenuPanel != null)
        {
            MainMenuPanel.SetActive(!MainMenuPanel.activeSelf);
            navBarHandler.SetActive(false);
        }  
    }
}
