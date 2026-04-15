using System;
using DefaultNamespace;
using UnityEngine;

public class onCameraCollide : MonoBehaviour
{
    [SerializeField] private Camera objectCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private CustomEvents customEvents;
    private bool canBeTriggered = true;
    private Rigidbody objectRidigbody; 
    private bool isFollowing;
    
    private void Awake()
    {
        customEvents.OnFollowChange += ChangeIsFollowing;
        objectRidigbody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isFollowing) Debug.Log($"not following anymore!");
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.transform.SetParent(null);
        if (!other.gameObject.GetComponentInParent<MouseLookController>() && canBeTriggered)
        {
            Debug.Log($"trigger entered!");
            canBeTriggered = false;
            customEvents.PublishOnFollowChange(false);
        }
    }

    private void ChangeIsFollowing(bool value)
    {
        isFollowing = value;
    }

    private void OnTriggerExit(Collider other)
    {
        canBeTriggered = true;
    }
}
