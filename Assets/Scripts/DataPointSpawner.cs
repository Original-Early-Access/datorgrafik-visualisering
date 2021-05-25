using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DataPointSpawner : MonoBehaviour
{
    public bool shouldUseScatterPlot;
    public GameObject DataPointPrefab;
    public List<GameObject> SpawnedDatapoints = new List<GameObject>();
    public List<GameObject> FeaturesList = new List<GameObject>();
    public Queue<DataPoint> DataPoints { get; set; } = new Queue<DataPoint>();
    public bool Destroy { get; set; }
    public static DataPointSpawner Instance;

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

            if (shouldUseScatterPlot)
            {
                var dp = DataPoints.Dequeue();
                var prefab = Instantiate(DataPointPrefab).GetComponent<DataPointHandler>();
                SpawnedDatapoints.Add(prefab.gameObject);
                prefab.SetScatterPlotPrediction(dp);
            }
            else
            {
                var dp = DataPoints.Dequeue();
                DataPointHandler PrevDataPoint = null;
                int count = 0;
                foreach(var feature in FeaturesList)
                { 
                    
                    var prefab = Instantiate(DataPointPrefab).GetComponent<DataPointHandler>();
                    SpawnedDatapoints.Add(prefab.gameObject);
                    prefab.SetParallelPlot(dp, feature.transform.position, PrevDataPoint, count);
                    PrevDataPoint = prefab;
                    count++;
                }
            }

        }
    }
    public void ResetDatapoints() => SpawnedDatapoints.ForEach(data => Destroy(data));
}
