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
            // vill vara säker att spline Animate är sätt till false
            return;
        }

        public void SwitchControll(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SwitchControll();
            }
        }

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