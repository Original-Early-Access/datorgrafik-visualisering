using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SelectionFeatureHandler : MonoBehaviour
{
    public DropDownHandler Feature_1;
    public DropDownHandler Feature_2;
    public DropDownHandler Feature_3;

    void Start()
    {
        KNNController.Instance.LoadData();
    }

    public void ChangeFeatures()
    {
        if (DataPointSpawner.Instance.shouldUseScatterPlot) { 
            KNNController.Instance.SelectedFeatures.Clear();
            KNNController.Instance.SelectedFeatures.Add(Feature_1.dropdown.value);
            KNNController.Instance.SelectedFeatures.Add(Feature_2.dropdown.value);
            KNNController.Instance.SelectedFeatures.Add(Feature_3.dropdown.value);
        }
        //new Thread(() => KNNController.Instance.StartPrediction(DataPointSpawner.Instance.shouldUseScatterPlot)).Start();
    }
}
