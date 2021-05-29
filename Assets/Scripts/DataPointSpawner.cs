using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DataPointSpawner : MonoBehaviour
{
    public bool shouldUseScatterPlot;
    public bool shouldUseParallelPlot;

    public GameObject DataPointPrefab;
    public GameObject CategoryPrefab;
    public GameObject CategoryMatrixPrefab;

    public GameObject Camera2D;
    public GameObject Camera3D;
    public GameObject Wall2D;

    public List<GameObject> SpawnedDatapoints = new List<GameObject>();
    public List<GameObject> Categories = new List<GameObject>();
    public List<MatrixCategory> MatrixCategories = new List<MatrixCategory>();
    public Dictionary<string, MatrixCategory> MatrixCategoryMap = new Dictionary<string, MatrixCategory>();

    public DataPointInformationHandler DataPointInformationHandler;

    public Queue<DataRow> DataPoints { get; set; } = new Queue<DataRow>();
    public bool ShouldResetDataPoints { get; set; }
    public static DataPointSpawner Instance;

    public bool CategoryExists;
    public int OffsetValueForCategories = 5;

    public void Start() => Instance = this;

  
    private void Update()
    {
        if (ShouldResetDataPoints)
        {
            ResetDatapoints();
            ShouldResetDataPoints = false;
        }
        while (DataPoints.Count > 0)
        {
            var dp = DataPoints.Dequeue();

            if (shouldUseScatterPlot) { 
                var prefab = Instantiate(DataPointPrefab).GetComponent<DataPointHandler>();
                SpawnedDatapoints.Add(prefab.gameObject);
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
                    SpawnedDatapoints.Add(prefab.gameObject);
                    prefab.SetParallelPlot(dp, category.transform.position, PrevDataPoint, count);
                    PrevDataPoint = prefab;
                    count++;
                }
            }
            else // Matrix plot
            {
                var matrix = MatrixCategoryMap[$"{dp.FeatureIDs[0]}{dp.FeatureIDs[1]}"];
                var partner = MatrixCategoryMap[$"{dp.FeatureIDs[1]}{dp.FeatureIDs[0]}"];
                Vector3 offset;
                DataPointHandler prefab = Instantiate(DataPointPrefab).GetComponent<DataPointHandler>();

                if (dp.FeatureIDs[0] < dp.FeatureIDs[1]) // then we go up positive
                    offset = new Vector3(
                        partner.transform.position.x - matrix.PositionOffsetForPlot.x + (float)dp.kNNValues[1],
                        matrix.transform.position.y - matrix.PositionOffsetForPlot.y + (float)dp.kNNValues[0],
                        matrix.transform.position.z + matrix.PositionOffsetForPlot.z);
                else
                    offset = new Vector3(
                        partner.transform.position.x - matrix.PositionOffsetForPlot.x + (float)dp.kNNValues[1],
                        matrix.transform.position.y - matrix.PositionOffsetForPlot.y + (float)dp.kNNValues[0],
                        matrix.transform.position.z + matrix.PositionOffsetForPlot.z);  

                prefab.SetMatrixPLot(dp, offset);
                SpawnedDatapoints.Add(prefab.gameObject);
            }
        }
    }

    public void InstantiateCategories(){
        Vector3 offset = new Vector3(0, 0, 0);
        for(int i = 0; i < KNNController.Instance.FeatureNames.Count; i++)
        {
            var prefab = Instantiate(CategoryPrefab, offset, Quaternion.identity);
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
        foreach(var category in MatrixCategories.ToArray())
        {
            category.InitiateIntercourseWithPartners(MatrixCategories);
            MatrixCategories.RemoveAt(0);
        }

    }
    public void ResetDatapoints() {
        SpawnedDatapoints.ForEach(data => Destroy(data));
        Categories.ForEach(category => Destroy(category));
    }
}
