using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public AxisHandler YAxis;
    public AxisHandler XAxis;


    public void GetValues(int featureidone, int featureidtwo)
    {
        float biggestValueX = DataPointSpawner.Instance.BiggestValuePairX[featureidone];
        float biggestValueY = DataPointSpawner.Instance.BiggestValuePairY[featureidtwo];
        int indexer = 0;
        for(float i = 1; i > 0; i -= 0.25f)
        {
            YAxis.DataTextValues[indexer].text = (biggestValueY / i).ToString();
            XAxis.DataTextValues[indexer].text = (biggestValueX / i).ToString();
            indexer++;
        }
    }
}
