using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DataPointInformationHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FeaturePanelPrefab;
    public Text LabelText;
    public List<FeaturePanelHandler> ExistingPanels = new List<FeaturePanelHandler>();

    public float Offset;
    public float StartingOffset = 10;
    private float currentOffset;
    public void InstantiatePanels(DataRow dataRow = null)
    {
        List<int> FeatureIDS = new List<int>();
        if (dataRow == null)
            FeatureIDS = KNNController.Instance.SelectedFeatures;
        else
            FeatureIDS = dataRow.FeatureIDs;

        ResetPanels();
        int counter = 0;
        foreach(var feature in FeatureIDS)
        {
            Vector3 offset = new Vector3(transform.position.x, transform.position.y + currentOffset);
            var prefab = Instantiate(FeaturePanelPrefab, offset, Quaternion.identity, parent: transform).GetComponent<FeaturePanelHandler>();
            prefab.FeatureText.text = KNNController.Instance.FeatureNames[feature];

            if (dataRow == null)
                prefab.FeatureID = feature;

            else{
                LabelText.text = dataRow.Label;
                prefab.FeatureInputField.text = dataRow.kNNValues[counter].ToString();
            }

            currentOffset += Offset;
            counter++;
            ExistingPanels.Add(prefab);
        }
    }

    public void ResetPanels()
    {
        currentOffset = StartingOffset;
        foreach (var panel in ExistingPanels)
            Destroy(panel.gameObject);
        ExistingPanels.Clear();
    }

    public void SpawnDataPoint()
    {
        if (DataPointSpawner.Instance.DataPlotterHandler.PlotMode.value == 2)
        {

            foreach (var matrix in DataPointSpawner.Instance.MatrixCategoryMap)
            {
                DataRow dataRow = new DataRow();
                dataRow.kNNValues = new double[2];

                dataRow.kNNValues[0] = double.Parse(ExistingPanels[0].FeatureInputField.text);
                dataRow.kNNValues[1] = double.Parse(ExistingPanels[1].FeatureInputField.text);
                dataRow.AllValues = dataRow.kNNValues.ToList();
                dataRow.FeatureIDs = new List<int> { int.Parse(matrix.Key[0].ToString()), int.Parse(matrix.Key[1].ToString())};
                KNNController.Instance.RunPrediction(dataRow, dataRow.FeatureIDs, dataRow.kNNValues, int.Parse(DataPointSpawner.Instance.DataPlotterHandler.KValue.text), DataPointSpawner.Instance.ShouldUseRegressor, DataPointSpawner.Instance.ShouldUseWeights, true);
            }
        }
        else
        {
            DataRow dataRow = new DataRow();
            dataRow.kNNValues = new double[2];
            dataRow.kNNValues = new double[ExistingPanels.Count];
            for (int i = 0; i < ExistingPanels.Count; i++)
            {
                dataRow.kNNValues[i] = double.Parse(ExistingPanels[i].FeatureInputField.text);
                dataRow.FeatureIDs.Add(DataPointSpawner.Instance.SpawnedDatapoints[0].GetComponent<DataPointHandler>().DataRow.FeatureIDs[i]);
            }
            dataRow.AllValues = dataRow.kNNValues.ToList();
            LabelText.text = KNNController.Instance.Labels[KNNController.Instance.RunPrediction(
                dataRow, 
                dataRow.FeatureIDs,
                dataRow.kNNValues,
                int.Parse(DataPointSpawner.Instance.DataPlotterHandler.KValue.text),
                DataPointSpawner.Instance.ShouldUseRegressor,
                DataPointSpawner.Instance.ShouldUseWeights, true)];
        }

    }
}
