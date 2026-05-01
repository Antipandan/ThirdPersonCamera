using System;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using Utility;

[RequireComponent(typeof(Rigidbody))]
public class onCameraCollide : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform headTransform;
    private bool isFollowing = true;
    private bool canBeTriggered = true;
    private float distanceCameraPlayer;
    private Vector3 oldHeadTransformLocalPos;
    private Quaternion oldHeadTransformRotation;
    
    private void Awake()
    {
        oldHeadTransformLocalPos = headTransform.localPosition;
        float distanceX = player.position.x - gameObject.transform.position.x;
        float distanceY = player.position.y - gameObject.transform.position.y;
        float distanceZ = player.position.z - gameObject.transform.position.z;
        distanceCameraPlayer = UtilityFunctions.GetMagnitudeOfVector(new Vector3(distanceX, distanceY, distanceZ));
    }

    private void FixedUpdate()
    {
        if (!isFollowing)
        {
            
            // ja det är samma namn men det är inte samma scope.
            float distanceX = player.position.x - gameObject.transform.position.x;
            float distanceY = player.position.y - gameObject.transform.position.y;
            float distanceZ = player.position.z - gameObject.transform.position.z;
            if (UtilityFunctions.GetMagnitudeOfVector(new Vector3(distanceX, distanceY, distanceZ)) > distanceCameraPlayer + 1/50f)
            {
                headTransform.SetParent(player);
                isFollowing = true;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canBeTriggered)
        {
            oldHeadTransformLocalPos = headTransform.localPosition;
            oldHeadTransformRotation = headTransform.localRotation;
            headTransform.SetParent(null);
            isFollowing = false;
            canBeTriggered = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        headTransform.SetParent(player.transform, false);
        SetLocalHeadRotation(oldHeadTransformRotation, oldHeadTransformLocalPos, headTransform);
        canBeTriggered = true;
    }

    private static void SetLocalHeadRotation(Quaternion rotation, Vector3 localPosition, Transform head)
    {
        head.localRotation = rotation;
        head.localPosition = localPosition;
    }
    
}
