using System;
using JetBrains.Annotations;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    
    public class SwitchControllMode : MonoBehaviour, IPauseable
    {
        [SerializeField] private bool lockCamereMovement;
        [SerializeField] [CanBeNull] private MouseLookController mouseLookController;
        private bool isPlaying = false;

        public bool IsPaused
        {
            get => isPlaying;
            set => isPlaying = value;
        }
        
        public void SwitchControll()
        {
            // xor bitwise operation
            isPlaying ^= true;
            if (mouseLookController != null && lockCamereMovement) mouseLookController.Pause();
            
        }
    }
}