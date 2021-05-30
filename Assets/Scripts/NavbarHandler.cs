using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavbarHandler : MonoBehaviour
{
    public GameObject InRuntimeNavbarPanel;
    public GameObject ChangePlotModePanel;
    public GameObject AddDatapointPanel;
    public DataPointInformationHandler AddDatapointInformationHandler;
    public void ToggleInRuntimeNavbar()
    {
        if (InRuntimeNavbarPanel != null)
            InRuntimeNavbarPanel.SetActive(!InRuntimeNavbarPanel.activeSelf);
    }

    public void ToggleChangePlotMode()
    {
        if (ChangePlotModePanel != null)
            ChangePlotModePanel.SetActive(!ChangePlotModePanel.activeSelf);
    }

    public void ToggleAddDatapoint()
    {
        if (AddDatapointPanel != null) { 
            AddDatapointPanel.SetActive(!AddDatapointPanel.activeSelf);
            AddDatapointInformationHandler.InstantiatePanels();
        }
    }
}
