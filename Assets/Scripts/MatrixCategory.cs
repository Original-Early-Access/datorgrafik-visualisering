using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MatrixCategory : MonoBehaviour
{
    // Start is called before the first frame update
    public int FeatureID;
    public string FeatureLabel;
    public Text FeatureUIText;
    public GameObject UnderBox;
    public Vector3 PositionOffsetForPlot;

    public void InitiateIntercourseWithPartners(List<MatrixCategory> partners) 
    {
        foreach (var partner in partners.Where(x => x != this))
            IntercourseWithAnotherCategory(partner);
    }

    private void IntercourseWithAnotherCategory(MatrixCategory partner) { 
        DataPointSpawner.Instance.MatrixCategoryMap.Add($"{FeatureID}{partner.FeatureID}", this);
        KNNController.Instance.StartPrediction(new List<int> { FeatureID, partner.FeatureID }, int.Parse(DataPointSpawner.Instance.DataPlotterHandler.KValue.text), DataPointSpawner.Instance.ShouldUseRegressor, DataPointSpawner.Instance.ShouldUseWeights);
        IntercourseWithAnotherCategoryButReverseCow(partner);
    }

    private void IntercourseWithAnotherCategoryButReverseCow(MatrixCategory partner)
    {
        DataPointSpawner.Instance.MatrixCategoryMap.Add($"{partner.FeatureID}{FeatureID}", partner);
        KNNController.Instance.StartPrediction(new List<int> {partner.FeatureID, FeatureID }, int.Parse(DataPointSpawner.Instance.DataPlotterHandler.KValue.text), DataPointSpawner.Instance.ShouldUseRegressor, DataPointSpawner.Instance.ShouldUseWeights);
    }
}
