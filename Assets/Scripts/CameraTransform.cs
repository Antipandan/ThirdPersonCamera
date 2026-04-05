using UnityEngine;
using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    [System.Serializable]
    public class CameraTransform
    {
        [SerializeField] private Transform newRotation;
        [SerializeField] private float transitionDuration;
        
        public Transform Rotation => newRotation;

        public float TransitionDuration
        {
            get => transitionDuration;
            set => transitionDuration = value;
        }
    }
}