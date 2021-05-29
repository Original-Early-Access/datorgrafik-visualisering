using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointInformationHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FeaturePanelPrefab;
    public List<GameObject> ExistingPanels = new List<GameObject>();
    public float Offset;
    public float StartingOffset = 10;
    private float currentOffset;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiatePanels(DataRow dataRow)
    {
        ResetPanels();
        int counter = 0;
        foreach(var feature in dataRow.FeatureIDs)
        {
            Vector3 offset = new Vector3(transform.position.x, transform.position.y + currentOffset);
            var prefab = Instantiate(FeaturePanelPrefab, offset, Quaternion.identity, parent: transform).GetComponent<FeaturePanelHandler>();
            prefab.FeatureID = feature;
            prefab.FeatureText.text = KNNController.Instance.FeatureNames[feature];
            prefab.FeatureInputField.text = dataRow.kNNValues[counter].ToString();
            currentOffset += Offset;
            counter++;
            ExistingPanels.Add(prefab.gameObject);
        }
    }

    public void ResetPanels()
    {
        currentOffset = StartingOffset;
        foreach (var panel in ExistingPanels)
            Destroy(panel);
        ExistingPanels.Clear();
    }
}
