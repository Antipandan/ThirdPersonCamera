using System;
using UnityEngine;

public class onCameraCollide : MonoBehaviour
{
    private bool canBeTriggered = true;
    private Rigidbody objectRidigbody;
    
    private void Awake()
    {
        objectRidigbody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponentInParent<MouseLookController>() && canBeTriggered)
        {
            Debug.Log($"trigger entered!");
            canBeTriggered = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canBeTriggered = true;
    }
}
