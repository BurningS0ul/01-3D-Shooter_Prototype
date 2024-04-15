using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawLine : MonoBehaviour
{
   private void OnDrawGizmos() {
    Gizmos.DrawLine(transform.position, transform.position + transform.forward * 50);
   }
}
