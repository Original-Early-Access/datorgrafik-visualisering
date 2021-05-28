using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class InitializeData : MonoBehaviour
{
    public Text CSVPath;
    public Text KValue;
    public Dropdown PlotMode;
    public Dropdown CSVType;
    public GameObject MainPanel;
    public DataPointSpawner pointSpawner;
    public NavbarHandler navbarToggle;

    public DropDownHandler Feature_1;
    public DropDownHandler Feature_2;
    public DropDownHandler Feature_3;

    public void OnLoadClick()
    {
        if (KValue.text != "") // CSV NAME MÅSTE KOLLAS OXÅ SENNARE
        {
            KNNController.Instance.LoadData(); //, CSVType.value
            StartPlot();
            MainPanel.SetActive(false);
            navbarToggle.ToggleInRuntimeNavbar();
        }
    }
    public void StartPlot()
    {
        if(KValue.text != "")
        {
            DataPointSpawner.Instance.ResetDatapoints();
            DataPointSpawner.Instance.Camera3D.SetActive(true);
            DataPointSpawner.Instance.Camera2D.SetActive(false);
            DataPointSpawner.Instance.Wall2D.SetActive(false);
            if (PlotMode.value == 0)
            {
                pointSpawner.shouldUseScatterPlot = true;
                KNNController.Instance.SelectedFeatures.Clear();
                KNNController.Instance.SelectedFeatures.Add(Feature_1.dropdown.value);
                KNNController.Instance.SelectedFeatures.Add(Feature_2.dropdown.value);
                KNNController.Instance.SelectedFeatures.Add(Feature_3.dropdown.value);
                new Thread(() => KNNController.Instance.StartPrediction(true, int.Parse(KValue.text), PlotMode.value)).Start(); 
            }
            else if(PlotMode.value == 1)
            {
                new Thread(() => KNNController.Instance.StartPrediction(false, int.Parse(KValue.text), PlotMode.value)).Start();
                pointSpawner.shouldUseScatterPlot = false;
            }else if(PlotMode.value == 2)
            {
                DataPointSpawner.Instance.Camera3D.SetActive(false);
                DataPointSpawner.Instance.Camera2D.SetActive(true);
                DataPointSpawner.Instance.Wall2D.SetActive(true);
            }
        }
    }

    public void ChangePlot()
    {
        StartPlot();
        navbarToggle.ToggleChangePlotMode();
    }

}
