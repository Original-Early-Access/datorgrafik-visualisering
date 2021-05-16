using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class AddNewDataPoint : MonoBehaviour
{
    public InputField DataPointY;
    public InputField DataPointX;
    public InputField DataPointZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDataPoints()
    {
        DataPoint dataPoint = new DataPoint();

        dataPoint.X = float.Parse(DataPointX.text, CultureInfo.InvariantCulture);
        dataPoint.Y = float.Parse(DataPointY.text, CultureInfo.InvariantCulture);
        dataPoint.Z = float.Parse(DataPointZ.text, CultureInfo.InvariantCulture);

        KNNController.Instance.RunPrediction(dataPoint);
    }
}
