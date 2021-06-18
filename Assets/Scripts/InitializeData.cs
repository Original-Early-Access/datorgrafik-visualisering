using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool CSVHasBeenLoaded;
    public void OnLoadClick()
    {
        if (KValue.text != "" && CSVHasBeenLoaded) // CSV NAME MÅSTE KOLLAS OXÅ SENNARE
        {
            DataPointSpawner.Instance.ShouldUseRegressor = CSVType.value.Equals(1); // value 1 should be equal to regressor :)
            KNNController.Instance.LoadData(@CSVPath.text); 
            StartPlot();
            MainPanel.SetActive(false);
            
            navbarToggle.ToggleInRuntimeNavbar();
        }
    }

    public void LoadCSV()
    {
        Feature_1.PopulateDropdown();
        Feature_2.PopulateDropdown();
        Feature_3.PopulateDropdown();
        CSVHasBeenLoaded = true;
    }
    public void StartPlot()
    {
        pointSpawner.shouldUseScatterPlot = false;
        pointSpawner.shouldUseParallelPlot = false;

        KNNController.Instance.SelectedFeatures.Clear();
        DataPointSpawner.Instance.ResetDatapoints();

        DataPointSpawner.Instance.MatrixCategories.ForEach(category => Destroy(category.gameObject));
        DataPointSpawner.Instance.MatrixCategories.Clear();
        DataPointSpawner.Instance.MatrixCategoryMap.Clear();

        DataPointSpawner.Instance.Categories.ForEach(category => Destroy(category.gameObject));
        DataPointSpawner.Instance.Categories.Clear();
        DataPointSpawner.Instance.CategoryExists = false;

        DataPointSpawner.Instance.Camera3D.SetActive(true);
        DataPointSpawner.Instance.Camera2D.SetActive(false);
        DataPointSpawner.Instance.Wall2D.SetActive(false);
            
            
        if (PlotMode.value == 0) // Should do Scatterplot
        {
            KNNController.Instance.SelectedFeatures.Add(Feature_1.dropdown.value);
            KNNController.Instance.SelectedFeatures.Add(Feature_2.dropdown.value);
            KNNController.Instance.SelectedFeatures.Add(Feature_3.dropdown.value);

            pointSpawner.shouldUseScatterPlot = true;
            new Thread(() => KNNController.Instance.StartPrediction(new List<int>() { 
                Feature_1.dropdown.value,
                Feature_2.dropdown.value,
                Feature_3.dropdown.value
            }, int.Parse(KValue.text), DataPointSpawner.Instance.ShouldUseRegressor, DataPointSpawner.Instance.ShouldUseWeights)).Start(); 
        }
        else if(PlotMode.value == 1) // should do parallel plot
        {
            pointSpawner.shouldUseParallelPlot = true;
            KNNController.Instance.SelectedFeatures = KNNController.Instance.AllFeatures.ToList();
            new Thread(() => KNNController.Instance.StartPrediction(KNNController.Instance.AllFeatures,
                int.Parse(KValue.text),
                DataPointSpawner.Instance.ShouldUseRegressor, DataPointSpawner.Instance.ShouldUseWeights)).Start();
        }
        else if(PlotMode.value == 2) // should do matrix plot
        {
            KNNController.Instance.SelectedFeatures = KNNController.Instance.AllFeatures.ToList();

            DataPointSpawner.Instance.Camera3D.SetActive(false);
            DataPointSpawner.Instance.Camera2D.SetActive(true);
            DataPointSpawner.Instance.Wall2D.SetActive(true);   
            DataPointSpawner.Instance.InstantiateMatrixCateogires();
        }
        
    }

    public void ChangePlot()
    {
        StartPlot();
    }

}
