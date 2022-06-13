using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform _target;

        void LateUpdate()
        {
            transform.position = _target.position;
        }
    }
}
