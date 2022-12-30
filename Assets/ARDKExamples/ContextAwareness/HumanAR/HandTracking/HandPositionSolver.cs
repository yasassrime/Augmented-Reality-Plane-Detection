using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARDK.Extensions;
using Niantic.ARDK.AR.Awareness;

public class HandPositionSolver : MonoBehaviour
{

    [SerializeField] private ARHandTrackingManager handTrackingManager;
    [SerializeField] private Camera ARCamera;
    [SerializeField] private float accuracyLevel = 0.85f;

    private Vector3 handPosition;
    public Vector3 HandPosition { get => handPosition; }



    // Start is called before the first frame update
    void Start()
    {
        handTrackingManager.HandTrackingUpdated += UpdateTrackingData;
    }

    private void OnDestroy()
    {
        handTrackingManager.HandTrackingUpdated -= UpdateTrackingData;
    }

    private void UpdateTrackingData(HumanTrackingArgs updatedData)
    {
        var trackingInfo = updatedData.TrackingData?.AlignedDetections;
        if(trackingInfo == null)
        {
            return;
        }
        foreach(var latestDatestDataSet in trackingInfo)
        {
            if(latestDatestDataSet.Confidence < accuracyLevel)
            {
                return;
            }

            Vector3 trackingFrameSize = new Vector3(latestDatestDataSet.Rect.width, latestDatestDataSet.Rect.height, 0);
            float depthEstimation = 0.2f + Mathf.Abs(1 - trackingFrameSize.magnitude);

            handPosition = ARCamera.ViewportToWorldPoint(new Vector3(latestDatestDataSet.Rect.center.x, 1 - latestDatestDataSet.Rect.y, depthEstimation));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
