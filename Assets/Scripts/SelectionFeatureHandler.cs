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
    // Start is called before the first frame update
    void Start()
    {
        KNNController.Instance.LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeFeatures()
    {
        KNNController.Instance.SelectedFeatures.Clear();
        KNNController.Instance.SelectedFeatures.Add(Feature_1.dropdown.value);
        KNNController.Instance.SelectedFeatures.Add(Feature_2.dropdown.value);
        KNNController.Instance.SelectedFeatures.Add(Feature_3.dropdown.value);
        new Thread(() => KNNController.Instance.StartPrediction()).Start();
    }
}
