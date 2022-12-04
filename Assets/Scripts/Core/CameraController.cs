using UnityEngine;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform _cameraTransform;
        [SerializeField] Transform _followTarget;
        [SerializeField] float _normalSpeed = 1f;
        [SerializeField] float _fastSpeed = 3f;
        [SerializeField] float _movementTime = 5f;
        [SerializeField] float _rotationAmount = 1f;
        [SerializeField] Vector3 _zoomAmount;

        Vector3 _newPosition;
        float _movementSpeed;
        Quaternion _newRotation;
        Vector3 _newZoom;
        bool _shouldFollowTarget = true;

        void Start()
        {
            _newPosition = transform.position;
            _newRotation = transform.rotation;
            _newZoom = _cameraTransform.localPosition;
        }

        void Update() 
        {
            CheckForFollowTargetInput();

            if (_shouldFollowTarget)
                transform.position = _followTarget.position;
            else
            {
                HandleIndependentCameraInput(); //put this in lateUpdate?
            }

            HandleCameraZoomInput();
        }

        void HandleIndependentCameraInput() //also clamp, see comment on video
        {
            HandleCameraSpeed();
            HandleCameraMovement();
            HandleCameraRotation();
        }
        
        void HandleCameraZoomInput() //clamp
        {
            KeyboardCameraZoom();
            MouseCameraZoom();
        }
       
        void CheckForFollowTargetInput()
        {
            if (Input.GetKey(KeyCode.F1))  
                _shouldFollowTarget = true;

            if (Input.GetKey(KeyCode.F2))
            {
                _newPosition = transform.position;
                _shouldFollowTarget = false;
            }
        }

        void HandleCameraMovement()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                _newPosition += (transform.forward * _movementSpeed);

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                _newPosition += (transform.forward * -_movementSpeed);

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                _newPosition += (transform.right * _movementSpeed);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                _newPosition += (transform.right * -_movementSpeed);

            transform.position = Vector3.Lerp(transform.position, _newPosition, _movementTime * Time.deltaTime);
        }

        void HandleCameraSpeed()
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _movementSpeed = _fastSpeed;
            else
                _movementSpeed = _normalSpeed;
        }

        void HandleCameraRotation()
        {
            if (Input.GetKey(KeyCode.Q))
                _newRotation *= Quaternion.Euler(Vector3.up * _rotationAmount);

            if (Input.GetKey(KeyCode.E))
                _newRotation *= Quaternion.Euler(Vector3.up * -_rotationAmount);

            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, _movementTime * Time.deltaTime);
        }

        void KeyboardCameraZoom()
        {
            if (Input.GetKey(KeyCode.R))
                _newZoom += _zoomAmount;

            if (Input.GetKey(KeyCode.F))
                _newZoom -= _zoomAmount;

            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _newZoom, _movementTime * Time.deltaTime);
        }

        void MouseCameraZoom()
        {
            if (Input.mouseScrollDelta.y != 0)
                _newZoom += Input.mouseScrollDelta.y * _zoomAmount;      
        }

    }
}
