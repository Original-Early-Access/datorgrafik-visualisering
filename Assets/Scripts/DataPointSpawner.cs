using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DataPointSpawner : MonoBehaviour
{
    public bool shouldUseScatterPlot;
    public GameObject DataPointPrefab;
    public GameObject CategoryPrefab;
    public GameObject CategoryMatrixPrefab;

    public GameObject Camera2D;
    public GameObject Camera3D;
    public GameObject Wall2D;

    public List<GameObject> SpawnedDatapoints = new List<GameObject>();
    public List<GameObject> Categories = new List<GameObject>();
    public List<MatrixCategory> MatrixCategories = new List<MatrixCategory>();
    public Queue<DataRow> DataPoints { get; set; } = new Queue<DataRow>();
    public bool ShouldResetDataPoints { get; set; }
    public static DataPointSpawner Instance;
    public bool CategoryExists;

    public int OffsetValueForCategories = 5;
    public float OffsetValueForMatrixCategories = 0.05f;

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
            else
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

        }
    }

    public void InstantiateCategories(){
        Vector3 offset = new Vector3(0, 0, 0);
        for(int i = 0; i < KNNController.Instance.FeatureNames.Count - 2; i++) // -2 BECAUSE STOOPID ID UNT OTHER THING :)
        {
            var prefab = Instantiate(CategoryPrefab, offset, Quaternion.identity);
            Categories.Add(prefab);
            offset = new Vector3(offset.x + OffsetValueForCategories, offset.y, offset.z);
        }
    }

    public void InstantiateMatrixCateogires()
    {
        Vector3 offset = new Vector3(0, 0, 0);
        for (int i = 1; i < KNNController.Instance.FeatureNames.Count - 1; i++) // -2 BECAUSE STOOPID ID UNT OTHER THING :)
        {
            var prefab = Instantiate(CategoryMatrixPrefab, offset, Quaternion.identity).GetComponent<MatrixCategory>();
            prefab.FeatureLabel = KNNController.Instance.FeatureNames[i];
            prefab.FeatureID = i;
            MatrixCategories.Add(prefab);
            offset = new Vector3(offset.x + OffsetValueForMatrixCategories, offset.y + OffsetValueForMatrixCategories, offset.z);
        }
    }
    public void ResetDatapoints() {
        SpawnedDatapoints.ForEach(data => Destroy(data));
        Categories.ForEach(category => Destroy(category));
    }
}
