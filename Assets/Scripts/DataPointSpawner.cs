using Assets.KNNAlgorithm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DataPointSpawner : MonoBehaviour
{
    public bool shouldUseScatterPlot;
    public bool shouldUseParallelPlot;
    public bool ShouldUseWeights => WeightToggle.isOn;
    public GameObject DataPointPrefab;
    public GameObject CategoryPrefab;
    public GameObject CategoryMatrixPrefab;

    public GameObject Camera2D;
    public GameObject Camera3D;
    public GameObject Wall2D;
    public GameObject AxisPrefab;
    public Toggle WeightToggle;

    public List<DataPointHandler> SpawnedDatapoints = new List<DataPointHandler>();
    public List<CategoryHandler> Categories = new List<CategoryHandler>();
    public List<MatrixCategory> MatrixCategories = new List<MatrixCategory>();

    public List<Color> Colors = new List<Color>();
    public Dictionary<string, MatrixCategory> MatrixCategoryMap = new Dictionary<string, MatrixCategory>();

    public DataPointInformationHandler DataPointInformationHandler;

    public Queue<DataRow> DataPoints { get; set; } = new Queue<DataRow>();
    public bool ShouldResetDataPoints { get; set; }
    public static DataPointSpawner Instance;

    public InitializeData DataPlotterHandler;

    public bool CategoryExists;
    public int OffsetValueForCategories = 5;

    public bool ShouldUseRegressor;

    public Gradient gradient = new Gradient();
    public float IncremenetGradientValue;   
    public void Start() => Instance = this;
    public Dictionary<int, float> BiggestValuePairX = new Dictionary<int, float>();
    public Dictionary<int, float> BiggestValuePairY = new Dictionary<int, float>();
    public Dictionary<int, float> BiggestValuePairZ = new Dictionary<int, float>();


    private void Update()
    {
        if (ShouldResetDataPoints)
        {
            ResetDatapoints();
            Colors.Clear();
            ShouldResetDataPoints = false;
        }
        while (DataPoints.Count > 0)
        {
            var dp = DataPoints.Dequeue();
            
           
            if (Colors.Count <= 0)
            {
                if(ShouldUseRegressor){
                    Colors.Add(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
                    Colors.Add(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
                }
                else
                {
                    foreach (var feature in KNNController.Instance.Labels)
                        Colors.Add(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

                }

                GradientColorKey[] colorKey = new GradientColorKey[Colors.Count];
                GradientAlphaKey[] alphaKey = new GradientAlphaKey[Colors.Count];

                float incremenetTime = 0f;
                IncremenetGradientValue = Colors.Count /2;
                for(int i = 0; i < Colors.Count; i++)
                {
                    colorKey[i] = new GradientColorKey(Colors[i], incremenetTime);
                    alphaKey[i] = new GradientAlphaKey(1f, incremenetTime);
                    incremenetTime += IncremenetGradientValue;
                }
                gradient.SetKeys(colorKey, alphaKey);
            }


            if (shouldUseScatterPlot) { 
                var prefab = Instantiate(DataPointPrefab).GetComponent<DataPointHandler>();
                SpawnedDatapoints.Add(prefab);
                prefab.SetScatterPlotPrediction(dp);
            }
            else if(shouldUseParallelPlot)
            {
                if (!CategoryExists) { 
                    InstantiateCategories();
                    CategoryExists = true;
                }
                DataPointHandler PrevDataPoint = null;
                int count = 0;
                foreach(var category in Categories)
                { 
                    var prefab = Instantiate(DataPointPrefab).GetComponent<DataPointHandler>();
                    SpawnedDatapoints.Add(prefab);
                    prefab.SetParallelPlot(dp, category.transform.position, PrevDataPoint, count);
                    PrevDataPoint = prefab;
                    count++;
                }
            }
            else // Matrix plot
            {
                var matrix = MatrixCategoryMap[$"{dp.FeatureIDs[0]}{dp.FeatureIDs[1]}"];
                var partner = MatrixCategoryMap[$"{dp.FeatureIDs[1]}{dp.FeatureIDs[0]}"];
                Vector3 offset = new Vector3(0,0,0);
                DataPointHandler prefab = Instantiate(DataPointPrefab).GetComponent<DataPointHandler>();

                if (dp.FeatureIDs[0] < dp.FeatureIDs[1])    
                {
                    offset = new Vector3(
                        partner.transform.position.x - matrix.PositionOffsetForPlot.x + ((float)dp.kNNValues[1] / BiggestValuePairY[dp.FeatureIDs[1]]) * matrix.transform.lossyScale.y,
                        matrix.transform.position.y - matrix.PositionOffsetForPlot.y + ((float)dp.kNNValues[0] / BiggestValuePairX[dp.FeatureIDs[0]]) * matrix.transform.lossyScale.x,
                        matrix.transform.position.z + matrix.PositionOffsetForPlot.z);
                }
                else
                    offset = new Vector3(
                        partner.transform.position.x - matrix.PositionOffsetForPlot.x + ((float)dp.kNNValues[0] / BiggestValuePairX[dp.FeatureIDs[0]]) * matrix.transform.lossyScale.x,
                        matrix.transform.position.y - matrix.PositionOffsetForPlot.y + ((float)dp.kNNValues[1] / BiggestValuePairY[dp.FeatureIDs[1]]) * matrix.transform.lossyScale.y,
                        matrix.transform.position.z + matrix.PositionOffsetForPlot.z);  

                prefab.SetMatrixPLot(dp, offset);
                
                SpawnedDatapoints.Add(prefab);
            }
        }
    }

    internal void FillBiggestValues()
    {
        foreach (var datapoint in KNNController.Instance.ActiveData.DataRows)
        {
            if (BiggestValuePairX.TryGetValue(datapoint.FeatureIDs[0], out float xValue))
                BiggestValuePairX[datapoint.FeatureIDs[0]] = datapoint.kNNValues[0] > xValue ? (float)datapoint.kNNValues[0] : xValue;
            else
                BiggestValuePairX.Add(datapoint.FeatureIDs[0], (float)datapoint.kNNValues[0]);

            if (BiggestValuePairY.TryGetValue(datapoint.FeatureIDs[1], out float yValue))
                BiggestValuePairY[datapoint.FeatureIDs[1]] = datapoint.kNNValues[1] > yValue ? (float)datapoint.kNNValues[1] : yValue;
            else
                BiggestValuePairY.Add(datapoint.FeatureIDs[1], (float)datapoint.kNNValues[1]);

            if (datapoint.kNNValues.Length > 2)
                if (BiggestValuePairZ.TryGetValue(datapoint.FeatureIDs[2], out float zValue))
                    BiggestValuePairZ[datapoint.FeatureIDs[2]] = datapoint.kNNValues[2] > yValue ? (float)datapoint.kNNValues[2] : zValue;
                else
                    BiggestValuePairZ.Add(datapoint.FeatureIDs[2], (float)datapoint.kNNValues[2]);
        }
        
    }

    public void InstantiateCategories(){
        Vector3 offset = new Vector3(0, 0, 0);
        for(int i = 0; i < KNNController.Instance.FeatureNames.Count; i++)
        {
            var prefab = Instantiate(CategoryPrefab, offset, Quaternion.identity).GetComponent<CategoryHandler>();
            prefab.LabelText.text = KNNController.Instance.FeatureNames[i];
            Categories.Add(prefab);
            offset = new Vector3(offset.x + OffsetValueForCategories, offset.y, offset.z);
        }
    }

    public void InstantiateMatrixCateogires()
    {
        Vector3 offset = new Vector3(0, 0, 0);
        for (int i = 0; i < KNNController.Instance.FeatureNames.Count; i++) 
        {
            var prefab = Instantiate(CategoryMatrixPrefab, offset, Quaternion.identity).GetComponent<MatrixCategory>();
            prefab.FeatureLabel = KNNController.Instance.FeatureNames[i];
            prefab.FeatureUIText.text = prefab.FeatureLabel;
            prefab.FeatureID = i;
            MatrixCategories.Add(prefab);
            offset = new Vector3(offset.x + CategoryMatrixPrefab.transform.lossyScale.x, offset.y - CategoryMatrixPrefab.transform.lossyScale.y, offset.z);
        }
        MatrixCategory[] tempCat = MatrixCategories.ToArray();
        foreach (var category in tempCat)
        {
            category.InitiateIntercourseWithPartners(MatrixCategories);
            MatrixCategories.RemoveAt(0);
        }
        MatrixCategories = tempCat.ToList();
        KNNController.Instance.PlotData();
        InstantiateAxis();
    }
    public void ResetDatapoints() {
        SpawnedDatapoints.ForEach(data => Destroy(data));
        SpawnedDatapoints.Clear();
    }

    public void InstantiateAxis()
    {
        foreach(var item in MatrixCategoryMap.Keys)
            InstnatiateAxis(Convert.ToInt32(item[0].ToString()), Convert.ToInt32(item[1].ToString()));
    }
    //min mic fun// Kabeln ær helt død tror jag kar inte

    private void InstnatiateAxis(int FeatureIDOne, int FeatureIDTwo)
    {
        Vector3 offset = new Vector3(0, 0, 0);
        var matrix = MatrixCategoryMap[$"{FeatureIDOne}{FeatureIDTwo}"];
        var partner = MatrixCategoryMap[$"{FeatureIDTwo}{FeatureIDOne}"];
        if (FeatureIDOne < FeatureIDTwo)
        {
            offset = new Vector3(
                partner.transform.position.x - matrix.PositionOffsetForPlot.x,
                matrix.transform.position.y,
                matrix.transform.position.z + matrix.PositionOffsetForPlot.z);
        }
        else
            offset = new Vector3(
                partner.transform.position.x - matrix.PositionOffsetForPlot.x ,
                matrix.transform.position.y,
                matrix.transform.position.z + matrix.PositionOffsetForPlot.z);
        var thing = Instantiate(AxisPrefab, offset, Quaternion.identity).GetComponent<GridHandler>();
        thing.GetValues(FeatureIDOne, FeatureIDTwo);
    }
}
