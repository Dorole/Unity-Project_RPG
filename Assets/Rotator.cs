using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] Transform _target;
        [SerializeField] float _speed;
        [SerializeField] Vector3 _axis;

        void Update()
        {
            transform.localRotation *= Quaternion.AngleAxis(_speed * Time.deltaTime, _axis);
            //transform.RotateAround(_target.position, _axis, _speed * Time.deltaTime);
        }
    }
}
