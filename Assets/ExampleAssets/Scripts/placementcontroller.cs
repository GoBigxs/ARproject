using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    private ARRaycastManager arRaycastManager;

    private float lastPlacementTime; // Time when the last object was placed.
    public float placementCooldown = 1.0f; // Cooldown time in seconds.

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        // Check if enough time has passed since the last placement.
        if (Time.time - lastPlacementTime >= placementCooldown)
        {
            if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                lastPlacementTime = Time.time; // Update the last placement time.
            }
        }
    }

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}
