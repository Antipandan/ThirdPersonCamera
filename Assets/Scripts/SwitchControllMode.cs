using System;
using JetBrains.Annotations;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

namespace DefaultNamespace
{
    
    public class SwitchControllMode : MonoBehaviour, IPauseable
    {
        [SerializeField] private bool lockCamereMovement;
        [SerializeField] [CanBeNull] private MouseLookController mouseLookController;
        [SerializeField] private bool loop = false;
        [SerializeField] private SplineAnimate splineAnimationController;
        private bool isPlaying = false;

        public bool IsPaused
        {
            get => isPlaying;
            set => isPlaying = value;
        }

        private void Start()
        {
            // vill vara säker att spline Animate är sätt till false
            splineAnimationController.PlayOnAwake = false;
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
            if (splineAnimationController == null) return;
            if (isPlaying)
            {
                splineAnimationController.Play();
            }
            else
            {
                splineAnimationController.Pause();
            }
        }
    }
}