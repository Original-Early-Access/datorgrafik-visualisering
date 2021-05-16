using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownHandler : MonoBehaviour
{
    public Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        PopulateDropdown();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void PopulateDropdown()
    {
        dropdown.ClearOptions();
        if (KNNController.Instance.FeatureNames.Count == 0)
            KNNController.Instance.LoadData();
        dropdown.AddOptions(KNNController.Instance.FeatureNames);
    }
}
