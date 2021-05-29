using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
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

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public string Label;
    public float X;
    public float Y;
    public float Z;

    public float LineWidth;

    public bool ShouldInterpolate;

    public Vector3 startMarker;
    public Vector3 endMarker;

    public bool journeyEnded;

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
        endMarker = new Vector3(X, Y, Z);
        ShouldInterpolate = true;
        DataRow = dataRow;
        startTime = Time.time;

        if (dataRow.LabelID == 0)
            Color = Color.green;

        else if(dataRow.LabelID == 1)
            Color = Color.blue;

        else
            Color = Color.red;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker, endMarker);
        if(MeshRenderer == null)
            MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.material.color = Color;

    }

    public void SetParallelPlot(DataRow dataRow, Vector3 pos, DataPointHandler prevDataPoint, int count)
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        endMarker = new Vector3(pos.x, (float)dataRow.AllValues[count] * 2, pos.z);
        PreviousDataPoint = prevDataPoint;
        DataRow = dataRow;
        
        ShouldInterpolate = true;

        startTime = Time.time;

        if (dataRow.LabelID == 0)
            Color = Color.green;

        else if (dataRow.LabelID == 1)
            Color = Color.blue;

        else
            Color = Color.red;
        if (MeshRenderer == null)
            MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.material.color = Color;

    }

    public void SetMatrixPLot(DataRow dataRow, Vector3 pos)
    {
        FeatureValue0 = (float)dataRow.kNNValues[0];
        FeatureValue1 = (float)dataRow.kNNValues[1];

        DataRow = dataRow;
        startMarker = transform.position;
        endMarker = pos;
        startTime = Time.time;
        ShouldInterpolate = true;
        journeyLength = Vector3.Distance(startMarker, endMarker);

        if (dataRow.LabelID == 0)
            Color = Color.green;
        else if (dataRow.LabelID == 1)
            Color = Color.blue;
        else
            Color = Color.red;

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
                CreateLine();
            }
        }
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
        DataPointSpawner.Instance.DataPointInformationHandler.InstantiatePanels(DataRow);
    }
}
