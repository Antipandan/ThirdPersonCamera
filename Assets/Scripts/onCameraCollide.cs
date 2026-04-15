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
    private Transform oldHeadTransform;
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
        oldHeadTransform = headTransform;
        objectRidigbody = gameObject.GetComponent<Rigidbody>();
        float distanceX = player.position.x - gameObject.transform.position.x;
        float distanceY = player.position.y - gameObject.transform.position.y;
        distanceCameraPlayer = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(cameraRay.origin, cameraRay.origin + cameraRay.direction * 2f, Color.red);
        if (Physics.Raycast(cameraRay, out RaycastHit hit))
        {
            Debug.Log(hit.transform.name);
        }
        if (!isFollowing)
        {
            
            // ja det är samma namn men det är inte samma scope.
            float distanceX = player.position.x - gameObject.transform.position.x;
            float distanceY = player.position.y - gameObject.transform.position.y;
            Debug.Log($"currentDistance: {Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY)}");
            // ge lite extra distance
            if (Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY) > distanceCameraPlayer + 1/50f)
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

        oldCameraPosition = gameObject.transform.localPosition;
        oldCameraRotation = gameObject.transform.localRotation;
        Debug.Log($"oldHeadTransform: {oldHeadTransform.localPosition}");
        headTransform.SetParent(null);
        isFollowing = false;
    }

    private void OnTriggerExit(Collider other)
    {
        headTransform = oldHeadTransform;
        // testar lite nya saker!
        SetLocalCameraPosition(oldCameraRotation, gameObject.GetComponent<Camera>());
    }

    private static void SetLocalCameraPosition(Quaternion rotation, Camera camera)
    {
        Debug.Log($"set local camera position!");
        camera.gameObject.transform.localRotation = rotation;
    }
    
}
