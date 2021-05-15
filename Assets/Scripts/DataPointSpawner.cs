using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DataPointSpawner : MonoBehaviour
{
    public GameObject DataPointPrefab;
    public Queue<DataPoint> DataPoints { get; set; } = new Queue<DataPoint>();

    public static DataPointSpawner Instance;

    public void Start()
    {
        Instance = this;
        new Thread(() => KNNController.Instance.StartPrediction()).Start();
    }

    private void Update()
    {
        while (DataPoints.Count > 0)
        {
            var dp = DataPoints.Dequeue();
            var prefab = Instantiate(DataPointPrefab).GetComponent<DataPointHandler>();
            prefab.SetPrediction(dp);
        }
    }
}
