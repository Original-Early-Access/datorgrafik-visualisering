using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointHandler : MonoBehaviour
{
    public DataPoint DataPoint;
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
        DataPoint = dataRow.DataPoint;
        Label = dataRow.Label;
        X = DataPoint.X;
        Y = DataPoint.Y;
        Z = DataPoint.Z;
        endMarker = new Vector3(DataPoint.X, DataPoint.Y, DataPoint.Z);
        ShouldInterpolate = true;

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
        endMarker = new Vector3(pos.x, (float)dataRow.Values[count] * 2, pos.z);
        PreviousDataPoint = prevDataPoint;
        DataRow = dataRow;
        // add some logic for drawing big line to prevdatapoint;
        // be careful with adding lines if thingy hasn't moved yet :)

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
}
