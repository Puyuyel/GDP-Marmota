using System;
using UnityEngine;

public class AlternateCamera : MonoBehaviour
{
    public float offsetY = 5f;
    public float smoothTime = 0.3f;
    public static event Action<bool> OnCameraToggle;

    private Vector3 _originalPosition;
    private Vector3 _targetPosition;
    private Vector3 _velocity = Vector3.zero;
    private bool _isDown = false;
    private bool _isMoving = false;

    private void Start()
    {
        _originalPosition = transform.position;
        _targetPosition = _originalPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isMoving)
        {
            ToggleCamera();
        }

        if(_isMoving)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, smoothTime);

            if(Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            {
                transform.position = _targetPosition;
                _isMoving = false;
            }
        }
    }

    private void ToggleCamera()
    {
        if (_isDown)
        {
            _targetPosition = _originalPosition;
        }
        else
        {
            _targetPosition = new Vector3(transform.position.x, transform.position.y - offsetY, transform.position.z);
        }

        _isDown = !_isDown;
        _isMoving = true;
        OnCameraToggle?.Invoke(_isDown);
    }
}
