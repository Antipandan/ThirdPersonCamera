using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CustomEvents : MonoBehaviour
    {
        private static CustomEvents _instance;
        private Action<bool> onFollowChange;
        private Action<string> onMouseSpeedChanged;

        
        // getters och setters ifall vi vill kontrollera synligheten senare!
        public Action<bool> OnFollowChange
        {
            get => onFollowChange;
            set => onFollowChange = value;
        }

        public Action<string> OnMouseSpeedChanged
        {
            get => onMouseSpeedChanged;
            set => onMouseSpeedChanged = value;
        }

        private void Awake()
        {
            if (_instance == null) Destroy(this);
        }

        public void PublishOnFollowChange(bool value)
        {
            onFollowChange?.Invoke(value);
        }

        public void PublishOnMouseSpeedChanged(string newMouseSpeed)
        {
            onMouseSpeedChanged?.Invoke(newMouseSpeed);
        }
    }
}