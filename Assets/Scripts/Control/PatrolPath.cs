using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float WAYPOINT_GIZMO_RADIUS = 0.2f;

        void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform currentChild = transform.GetChild(i);
                Gizmos.DrawSphere(currentChild.position, WAYPOINT_GIZMO_RADIUS);
            }
        }
    }
}
