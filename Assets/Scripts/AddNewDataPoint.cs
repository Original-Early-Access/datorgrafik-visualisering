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
        dataRow.kNNValues[0] = float.Parse(DataPointX.text, CultureInfo.InvariantCulture);
        dataRow.kNNValues[1] = float.Parse(DataPointY.text, CultureInfo.InvariantCulture);
        dataRow.kNNValues[2] = float.Parse(DataPointZ.text, CultureInfo.InvariantCulture);

        //KNNController.Instance.RunPrediction(dataRow);
    }
}
