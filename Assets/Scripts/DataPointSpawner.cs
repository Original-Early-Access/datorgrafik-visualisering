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

    public List<GameObject> SpawnedDatapoints = new List<GameObject>();
    public List<GameObject> Categories = new List<GameObject>();

    public Queue<DataRow> DataPoints { get; set; } = new Queue<DataRow>();
    public bool Destroy { get; set; }
    public static DataPointSpawner Instance;
    public bool CategoryExists;

    public int OffsetValueForCategories = 5;

    public void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Destroy)
        {
            ResetDatapoints();
            Destroy = false;
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
    public void ResetDatapoints() => SpawnedDatapoints.ForEach(data => Destroy(data));
}
