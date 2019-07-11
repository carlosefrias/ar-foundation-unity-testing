using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using TrackableType = UnityEngine.XR.ARSubsystems.TrackableType;

public class ArTapToPlaceObject : MonoBehaviour
{
    #region Public fields

    public GameObject PlacementIndicator, ObjecToPlace;
    #endregion
    #region Private fields
   
    private ARSessionOrigin _arOrigin;
    private Pose _placementPose;
    private bool _placementPoseIsValid;
    
    #endregion

    #region Unity methods
    
    void Start()
    {
        _arOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (_placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }
    #endregion

    #region Public methods

    #endregion

    #region Private Methods
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var screenPoint = new Vector2(screenCenter.x, screenCenter.y);
        var hits = new List<ARRaycastHit>();
        _arOrigin.GetComponent<ARRaycastManager>().Raycast(screenPoint, hits, TrackableType.Planes);
        _placementPoseIsValid = hits.Count != 0;
        if (_placementPoseIsValid)
        {
            _placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0.0f, cameraForward.z).normalized;
            _placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
    
    private void UpdatePlacementIndicator()
    {
        if (_placementPoseIsValid)
        {
            PlacementIndicator.SetActive(true);
            PlacementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        else
        {
            PlacementIndicator.SetActive(false);
        }
    }
    
    private void PlaceObject()
    {
        Instantiate(ObjecToPlace, _placementPose.position, _placementPose.rotation);
    } 
    #endregion
}
