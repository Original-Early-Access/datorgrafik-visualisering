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

    public void SetDataPoints()
    {
        DataRow dataRow = new DataRow();
        dataRow.DataPoint = new DataPoint();
        dataRow.DataPoint.X = float.Parse(DataPointX.text, CultureInfo.InvariantCulture);
        dataRow.DataPoint.Y = float.Parse(DataPointY.text, CultureInfo.InvariantCulture);
        dataRow.DataPoint.Z = float.Parse(DataPointZ.text, CultureInfo.InvariantCulture);

        //KNNController.Instance.RunPrediction(dataRow);
    }
}
