using System;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;

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
    private Vector3 oldCameraPosition;
    private Quaternion oldCameraRotation;
    
    private void Awake()
    {
        Vector3 CameraPosition = gameObject.transform.position;
        Vector3 PlayerPosition = player.position;
        oldHeadTransformLocalPos = headTransform.localPosition;
        float distanceX = player.position.x - gameObject.transform.position.x;
        float distanceY = player.position.y - gameObject.transform.position.y;
        float distanceZ = player.position.z - gameObject.transform.position.z;
        distanceCameraPlayer = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ);
    }

    private void FixedUpdate()
    {
        if (!isFollowing)
        {
            
            // ja det är samma namn men det är inte samma scope.
            float distanceX = player.position.x - gameObject.transform.position.x;
            float distanceY = player.position.y - gameObject.transform.position.y;
            float distanceZ = player.position.z - gameObject.transform.position.z;
            // Debug.Log($"currentDistance: {Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY)}");
            // ge lite extra distance
            if (Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ) > distanceCameraPlayer + 1/50f)
            {
                // TODO detta är problematiskt när spelare hoppar upp och ned framför en väg
                headTransform.SetParent(player);
                isFollowing = true;
            }
            
        }
    }

    private void Update()
    {
        Vector3 CameraPosition = gameObject.transform.position;
        Vector3 PlayerPosition = player.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canBeTriggered)
        {
            Debug.Log($"trigger entered!");
            oldHeadTransformLocalPos = headTransform.localPosition;
            oldHeadTransformRotation = headTransform.localRotation;
            oldCameraPosition = gameObject.transform.localPosition;
            oldCameraRotation = gameObject.transform.localRotation;
            headTransform.SetParent(null);
            isFollowing = false;
            canBeTriggered = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"distance between player and camera: {distanceCameraPlayer}");
        headTransform.SetParent(player.transform, false);
        // testar lite nya saker!
        //SetLocalCameraPosition(oldCameraRotation, oldCameraPosition, gameObject.GetComponent<Camera>());
        SetLocalHeadRotation(oldHeadTransformRotation, oldHeadTransformLocalPos, headTransform);
        canBeTriggered = true;
    }

    private static void SetLocalCameraPosition(Quaternion rotation,Vector3 oldLocalPosition, Camera camera)
    {
        camera.gameObject.transform.localRotation = rotation;
        camera.gameObject.transform.localPosition = oldLocalPosition;
    }

    private static void SetLocalHeadRotation(Quaternion rotation, Vector3 localPosition, Transform head)
    {
        Debug.Log($"localRotation: {rotation}, localPosition: {localPosition}");
        head.localRotation = rotation;
        head.localPosition = localPosition;
        Debug.Log($"new values LocalRotation: {head.localRotation}, localPosition: {head.localPosition}");
    }
    
}
