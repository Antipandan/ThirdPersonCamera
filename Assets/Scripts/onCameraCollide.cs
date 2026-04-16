using System;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class onCameraCollide : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform headTransform;
    private Rigidbody objectRidigbody; 
    private bool isFollowing = true;
    private float distanceCameraPlayer;
    private Vector3 oldHeadTransformLocalPos;
    private Quaternion oldHeadTransformRotation;
    private Vector3 oldCameraPosition;
    private Quaternion oldCameraRotation;
    private Ray cameraRay;
    
    private void Awake()
    {
        Vector3 CameraPosition = gameObject.transform.position;
        Vector3 PlayerPosition = player.position;
        cameraRay = new Ray(gameObject.transform.position, new Vector3(
            CameraPosition.x + PlayerPosition.x,
            CameraPosition.y + PlayerPosition.y,
            CameraPosition.z + PlayerPosition.z).normalized
        );
        oldHeadTransformLocalPos = headTransform.localPosition;
        objectRidigbody = gameObject.GetComponent<Rigidbody>();
        float distanceX = player.position.x - gameObject.transform.position.x;
        float distanceY = player.position.y - gameObject.transform.position.y;
        float distanceZ = player.position.z - gameObject.transform.position.z;
        distanceCameraPlayer = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ);
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(cameraRay.origin, cameraRay.origin + cameraRay.direction * 2f, Color.red);
        if (Physics.Raycast(cameraRay, out RaycastHit hit))
        {
            // Debug.Log(hit.transform.name);
        }
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
                headTransform.SetParent(player);
                isFollowing = true;
            }
        }
    }

    private void Update()
    {
        Vector3 CameraPosition = gameObject.transform.position;
        Vector3 PlayerPosition = player.position;
        cameraRay.origin = gameObject.transform.position;
        cameraRay.direction = new Vector3(
                CameraPosition.x + PlayerPosition.x,
                CameraPosition.y + PlayerPosition.y,
                CameraPosition.z + PlayerPosition.z).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        oldHeadTransformLocalPos = headTransform.localPosition;
        oldHeadTransformRotation = headTransform.localRotation;
        oldCameraPosition = gameObject.transform.localPosition;
        oldCameraRotation = gameObject.transform.localRotation;
        Debug.Log($"oldHeadTransform: {oldHeadTransformLocalPos}");
        headTransform.SetParent(null);
        isFollowing = false;
    }

    private void OnTriggerExit(Collider other)
    {
        headTransform.SetParent(player.transform, false);
        // testar lite nya saker!
        //SetLocalCameraPosition(oldCameraRotation, oldCameraPosition, gameObject.GetComponent<Camera>());
        SetLocalHeadRotation(oldHeadTransformRotation, oldHeadTransformLocalPos, headTransform);
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
