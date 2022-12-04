using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement
{
    public class Rotator : MonoBehaviour
    {
        //[SerializeField] Transform _target;
        [SerializeField] float _speed;
        [SerializeField] Vector3 _axis;

        void Update()
        {
            transform.localRotation *= Quaternion.AngleAxis(_speed * Time.deltaTime, _axis);
        }
    }
}
