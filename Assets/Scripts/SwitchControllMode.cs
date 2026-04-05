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
        [SerializeField] private bool loop = false;
        private bool isPlaying = false;

        public bool IsPaused
        {
            get => isPlaying;
            set => isPlaying = value;
        }

        private void Start()
        {
            
        }

        // den är här ifall man vill aktivera kamera åkning via knapp tryckning
        public void SwitchControll(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SwitchControll();
            }
        }

        // holy spaghetti
        public void SwitchControll()
        {
            // xor bitwise operation
            isPlaying ^= true;
            if (lockCamereMovement)
            {
                if (mouseLookController != null) mouseLookController.Pause();
            }
        }
    }
}