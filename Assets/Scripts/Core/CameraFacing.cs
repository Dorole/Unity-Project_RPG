using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        Transform _mainCamera;

        void Awake()
        {
            _mainCamera = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.forward = _mainCamera.forward;
        }
    }
}
