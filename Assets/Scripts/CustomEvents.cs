using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CustomEvents : MonoBehaviour
    {
        private static CustomEvents _instance;
        private Action<bool> onFollowChange;

        public Action<bool> OnFollowChange
        {
            get => onFollowChange;
            set => onFollowChange = value;
        }

        private void Awake()
        {
            if (_instance == null) Destroy(this);
        }

        public void PublishOnFollowChange(bool value)
        {
            onFollowChange?.Invoke(value);
        }
    }
}