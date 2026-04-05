using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DrawCircleGizmo : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(gameObject.transform.position, 1/10f);
        }
    }
}