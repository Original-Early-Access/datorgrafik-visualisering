using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPointHandler : MonoBehaviour
{
    public float FeatureValue0;
    public float FeatureValue1; 
    public DataRow DataRow;
    public DataPointHandler PreviousDataPoint;
    public LineRenderer lineRenderer;
    public MeshRenderer MeshRenderer;
    public Color Color;
    public float speed = 1.0F;
    public float SecondsToBlink;
    public AudioSource SpawnSound;
    public AudioSource DatapointOnClickSound;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public string Label;
    public int LabelID;
    public double RegressionValue;
    public float X;
    public float Y;
    public float Z;

    public float LineWidth;

    public bool ShouldInterpolate;

    public Vector3 startMarker;
    public Vector3 endMarker;

    public bool journeyEnded;
    public bool ShouldBlink;
    public float BlinkCooldown;
    private float currentCooldown;
    public List<Color> BlinkColors;
    public int BlinkIndex;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        MeshRenderer = GetComponent<MeshRenderer>();
    }
    public void SetScatterPlotPrediction(DataRow dataRow)
    {
        startMarker = transform.position;
        X = (float)dataRow.kNNValues[0];
        Y = (float)dataRow.kNNValues[1];
        Z = (float)dataRow.kNNValues[2];
        LabelID = dataRow.LabelID;
        RegressionValue = dataRow.RegressionValue;
        endMarker = new Vector3(X, Y, Z);
        ShouldInterpolate = true;
        DataRow = dataRow;
        startTime = Time.time;
        if (DataPointSpawner.Instance.ShouldUseRegressor)
        {
            float diff = (float)(dataRow.LabelID - dataRow.RegressionValue);
            float eval = DataPointSpawner.Instance.IncremenetGradientValue * dataRow.LabelID;
            eval += diff;
            if (eval > 1)
                eval = 1;
            if (eval < 0)
                eval = 0;

            Debug.Log(dataRow.RegressionValue);
            Color = DataPointSpawner.Instance.gradient.Evaluate(dataRow.LabelID);  
        }
        else
        {
            Color = DataPointSpawner.Instance.Colors[DataRow.LabelID];
        }
        
       
        journeyLength = Vector3.Distance(startMarker, endMarker);
        if(MeshRenderer == null)
            MeshRenderer = GetComponent<MeshRenderer>();

        MeshRenderer.material.color = Color;
    }

    public void SetParallelPlot(DataRow dataRow, Vector3 pos, DataPointHandler prevDataPoint, int count)
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        endMarker = new Vector3(pos.x, (float)dataRow.kNNValues[count] * 2, pos.z);
        PreviousDataPoint = prevDataPoint;
        DataRow = dataRow;
        RegressionValue = dataRow.RegressionValue;

        ShouldInterpolate = true;

        startTime = Time.time;

        if (DataPointSpawner.Instance.ShouldUseRegressor)
        {
            float diff = (float)(dataRow.LabelID - dataRow.RegressionValue);
            float eval = DataPointSpawner.Instance.IncremenetGradientValue * dataRow.LabelID;
            eval += diff;
            if (eval > 1)
                eval = 1;
            if (eval < 0)
                eval = 0;
            Color = DataPointSpawner.Instance.gradient.Evaluate(eval);
        }
        else
        {
            Color = DataPointSpawner.Instance.Colors[DataRow.LabelID];
        }

        if (MeshRenderer == null)
            MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.material.color = Color;
    }

    public void SetMatrixPLot(DataRow dataRow, Vector3 pos)
    {
        FeatureValue0 = (float)dataRow.kNNValues[0];
        FeatureValue1 = (float)dataRow.kNNValues[1];
        RegressionValue = dataRow.RegressionValue;

        DataRow = dataRow;
        startMarker = transform.position;


        endMarker = pos;
        startTime = Time.time;
        ShouldInterpolate = true;
        journeyLength = Vector3.Distance(startMarker, endMarker);

        if (DataPointSpawner.Instance.ShouldUseRegressor)
        {
            Color = DataPointSpawner.Instance.gradient.Evaluate((float)(
                (dataRow.kNNValues[0] + dataRow.kNNValues[1]) / 2) 
                /
                (DataPointSpawner.Instance.BiggestValuePairX[dataRow.FeatureIDs[0]] + DataPointSpawner.Instance.BiggestValuePairY[dataRow.FeatureIDs[1]]) / 2 );
        }
        else
        {
            Debug.Log(dataRow.LabelID);
            Color = DataPointSpawner.Instance.Colors[DataRow.LabelID];
        }

        if (MeshRenderer == null)
            MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.material.color = Color;
    }
    void Update()
    {
        if (ShouldInterpolate && !journeyEnded) { 
        // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
            
            if(Vector3.Distance(transform.position, endMarker) <= 0) { 
                journeyEnded = true;
                HighlightNeighbours();
                //SpawnSound.Play();
                CreateLine();
            }
        }

        if (ShouldBlink)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                MeshRenderer.material.color = BlinkColors[BlinkIndex];
                if (BlinkIndex == 1)
                    BlinkIndex = 0;
                else BlinkIndex = 1;
                currentCooldown = BlinkCooldown;
            }
        }
    }

    public void HighlightNeighbours()
    {
        if (DataRow.ShouldUseNeighbour)
            foreach (var neighbour in DataPointSpawner.Instance.SpawnedDatapoints.Where(x => DataRow.Neighbours.Contains(x.GetComponent<DataPointHandler>().DataRow)))
                neighbour.GetComponent<DataPointHandler>().Blink();
    }

    public void Blink()
    {
        StartCoroutine(BlinkTimer(SecondsToBlink)); 
    }

    public IEnumerator BlinkTimer(float secondsToBlink)
    {
        ShouldBlink = true;
        yield return new WaitForSeconds(secondsToBlink);
        ShouldBlink = false;
        MeshRenderer.material.color = Color;

    }
    public void CreateLine()
    {
        if (!(PreviousDataPoint is null))
        {
            lineRenderer.startWidth = LineWidth;
            lineRenderer.endWidth = LineWidth;
            lineRenderer.material.color = Color;
            lineRenderer.SetPositions(new Vector3[] { transform.position, PreviousDataPoint.transform.position });
        }
    }
    private void OnMouseDown()
    {
        DatapointOnClickSound.Play();
        DataPointSpawner.Instance.DataPointInformationHandler.InstantiatePanels(DataRow);
        HighlightNeighbours();
    }
}
