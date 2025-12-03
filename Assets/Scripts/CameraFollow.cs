using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Transform mytransform;
    private Vector3 cameraOffset;
    private Vector3 followPosition;
    [SerializeField] private float rayDistance;
    [SerializeField] private float speedOffset;
    private float y;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mytransform = GetComponent<Transform>();
        cameraOffset = mytransform.position;
    }


    void LateUpdate()
    {
        followPosition = target.position + cameraOffset;
        mytransform.position = followPosition;
        UpdateCameraOffset();
    }

    private void UpdateCameraOffset()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(target.position, Vector3.down, out raycastHit, rayDistance))
        {
            y = Mathf.Lerp(y, raycastHit.point.y, Time.deltaTime * speedOffset);
        }
        followPosition.y = cameraOffset.y + y;
        mytransform.position = followPosition;
    }
}
