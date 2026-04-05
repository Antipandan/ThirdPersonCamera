using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class FixedCameraController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private CameraTransform newCameraTransform;
        [SerializeField] private UnityEvent onTriggerEntered;
        private static Transform currentLookingPoint;
        public static bool isPlaying = false;
        
        private void Update()
        {
            if (isPlaying) RotateBodies(currentLookingPoint);
        }

        public void OnPlayModeSwitch()
        {
            // xor
            isPlaying = isPlaying ^ true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            currentLookingPoint = newCameraTransform.Rotation != null? newCameraTransform.Rotation : other.transform;
            SwitchControllMode controlMode = playerCamera.gameObject.GetComponent<SwitchControllMode>();
            if (controlMode == null) return;
            if (!controlMode.IsPaused) return;
            //RotateBodies(newCameraTransform.Rotation);
            onTriggerEntered?.Invoke();
        }
        
        private void RotateBodies(Transform target, float deltaTime)
        {
            if (1 >= newCameraTransform.TransitionDuration)
            {
                // Player rotation
                Vector3 oldPlayerRotation = playerTransform.rotation.eulerAngles;
            
                playerTransform.eulerAngles = new Vector3(
                    oldPlayerRotation.x,
                    //Vector3.Lerp(playerTransform.transform.localEulerAngles, target.transform.localEulerAngles, elapsedTime / newCameraTransform.TransitionDuration).y, 
                    oldPlayerRotation.z);
            
                //Kamera rotation
                Vector3 oldCameraRotation = playerCamera.transform.localEulerAngles;
                playerCamera.transform.LookAt(target);
            
                playerCamera.transform.localEulerAngles = new Vector3(
                    //Vector3.Lerp(playerCamera.transform.localEulerAngles, target.transform.localEulerAngles, elapsedTime / newCameraTransform.TransitionDuration).x, 
                    oldCameraRotation.y, 
                    oldCameraRotation.z);
                
                //elapsedTime += deltaTime;
            }
            
        }

        private void RotateBodies(Transform target)
        {
            // Player rotation
            Vector3 oldPlayerRotation = playerTransform.rotation.eulerAngles;
            playerTransform.LookAt(target);
            
            playerTransform.eulerAngles = new Vector3(
                oldPlayerRotation.x,
                playerTransform.eulerAngles.y,
                oldPlayerRotation.z);
            
            //Kamra rotation
            Vector3 oldCameraRotation = playerCamera.transform.localEulerAngles;
            playerCamera.transform.LookAt(target);
            
            playerCamera.transform.localEulerAngles = new Vector3(
                playerCamera.transform.localEulerAngles.x, 
                oldCameraRotation.y, 
                oldCameraRotation.z);
        }
    }
}