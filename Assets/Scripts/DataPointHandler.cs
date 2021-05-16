using Assets.KNNAlgorithm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointHandler : MonoBehaviour
{
    public DataPoint DataPoint;
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public string Label;

    public bool ShouldInterpolate;

    public Vector3 startMarker;
    public Vector3 endMarker;
    public void SetPrediction(DataPoint dp)
    {
        startMarker = transform.position;
        DataPoint = dp;
        Label = dp.Label;
        endMarker = new Vector3(dp.X, dp.Y, dp.Z);
        ShouldInterpolate = true;

        startTime = Time.time;

        if (dp.LabelID == 0)
            GetComponent<MeshRenderer>().material.color = Color.green;

        else if(dp.LabelID == 1)
            GetComponent<MeshRenderer>().material.color = Color.blue;

        else
            GetComponent<MeshRenderer>().material.color = Color.red;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker, endMarker);
    }

    void Update()
    {
        if (ShouldInterpolate) { 
        // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
        }
    }
}
