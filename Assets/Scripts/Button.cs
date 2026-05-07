using System;
using UnityEngine;
using UnityEditor;

namespace DefaultNamespace
{
    [CustomEditor(typeof(MouseLookController))]
    public class Button : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        
            GUILayoutOption[] glo = {GUILayout.Width(100), GUILayout.Height(34)};
            if (GUILayout.Button("Update Position", glo))
            {
                MouseLookController mouse = target as MouseLookController;
                mouse?.ChangeCameraPosition();
            }
        }
    }
}