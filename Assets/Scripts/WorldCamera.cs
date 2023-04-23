using UnityEngine;


[RequireComponent(typeof(Camera))]
public class WorldCamera : MonoBehaviour, IDragHandler
{
    [Header("General")]

    [SerializeField]
    private GameObject _cameraRoot = null;
    [SerializeField]
    private Camera _camera;

    [Header("Rotation Parameters")]

    [SerializeField]
    private GameObject _rotatePivot;
    [SerializeField]
    private float _initialVerticalAngle = 355.0f;

    [Header("Camera Movement Parameters")]
    
    [SerializeField]
    private float _rotationPrecision = 0.015f;
    [SerializeField]
    private float _panPrecision = 0.015f;

    [Header("Other Parameters")]

    [SerializeField]
    private float _inertiaTime = 0.5f;
    [SerializeField]
    private float _resetHorizontalRotationSpeed = 5.0f;
    [SerializeField]
    private float _resetHorizontalPanSpeed = 2.0f;

    [SerializeField]
    private GameObject _pivotSphere;

    private bool _isRotating = false;
    private bool _isPanning = false;

    private Vector3 _initialPosition;
    private Vector3 _nextPivotPosition;

    private Vector2 _targetAngles;
    private Vector2 _targetPos;

    private Quaternion _azimuthQuaternion;

    private bool _underInertia;
    private Vector3 _inertiaStartVelocity;
    private float _inertiaCurrentTime = 0.0f;

    private bool _isRotatePivotInitialized = false;

    public bool EnableInertia { get; set; } = false;

    public Camera Camera { get => _camera; }

    public void LookAtTarget(Transform target)
    {
        TrySetPivot(target.position);

        _targetPos = new Vector3(target.position.x,
            transform.position.y);
     
        _isPanning = true;
    }

    private void TrySetPivot(Vector3 target)
    {
        _nextPivotPosition = new Vector3(target.x,
            target.y,
            target.z);
    }

    public void Dragging(Vector2 currentPosition, Vector2 deltaPosition, int mouseButton)
    {
        _isPanning = false;
        _isRotating = false;

        if (mouseButton == 0 /*kMouseRightButton*/)
        {
            float azimuth = -180.0f * deltaPosition.x / Screen.width;
            RotateHorizontal(azimuth);
        }
    }

    public void ResetCameraView()
    {
        RotateTo(new Vector2(0, _initialVerticalAngle));
        MoveTo(_initialPosition);
    }

    public void ResetVerticalRotation()
    {
        RotateTo(new Vector2(_cameraRoot.transform.localRotation.eulerAngles.z, _initialVerticalAngle));
    }

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        _azimuthQuaternion = new Quaternion(0, 0, transform.rotation.z, transform.rotation.w);

        HandlePanInertia();
        HandleCameraViewChange();
    }

    private void HandlePanInertia()
    {
        if (!EnableInertia)
        {
            return;
        }

        if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            _underInertia = true;
        }

        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            _underInertia = false;
        }

        if (_underInertia && _inertiaCurrentTime <= _inertiaTime)
        {
            Camera.transform.position += _inertiaStartVelocity;
            _inertiaStartVelocity = Vector3.Lerp(_inertiaStartVelocity, Vector3.zero, _inertiaCurrentTime);
            _inertiaCurrentTime += Time.smoothDeltaTime;
        }
        else
        {
            _underInertia = false;
            _inertiaCurrentTime = 0.0f;
        }
    }

    private void HandleCameraViewChange()
    {
        if (_isRotating)
        {
            bool done = true;

            // Rotation
            float distanceH = _targetAngles.y - _cameraRoot.transform.localRotation.eulerAngles.y;
            if (Mathf.Abs(distanceH) >= 180)
            {
                distanceH += -Mathf.Sign(distanceH) * 360.0f;
            }

            if (Mathf.Abs(distanceH) > _rotationPrecision)
            {
                RotateHorizontal(distanceH * _resetHorizontalRotationSpeed * Time.deltaTime);
                done = false;
            }

            if (done)
            {
                _isRotating = false;
            }
        }

        if (_isPanning)
        {
            bool done = true;

            // TODO: XY Position
            float distancePosX = _targetPos.x - transform.position.x;
            if (Mathf.Abs(distancePosX) > _panPrecision)
            {
                transform.position = new Vector3(
                    transform.position.x + distancePosX * _resetHorizontalPanSpeed * Time.deltaTime,
                    transform.position.y, 
                    transform.position.z);

                done = false;
            }

            if (done)
            {
                _isPanning = false;
                _rotatePivot.transform.position = _nextPivotPosition;
            }
        }
    }

    private void MoveTo(Vector2 targetPos)
    {
        _targetPos = targetPos;
        _isPanning = true;
    }

    private void RotateTo(Vector2 targetAngles)
    {
        _targetAngles = targetAngles;
        _isRotating = true;
    }

    private void RotateHorizontal(float deltaSize)
    {
        Vector3 currentAngle = Camera.transform.rotation.eulerAngles;
        _cameraRoot.transform.RotateAround(_rotatePivot.transform.position, Vector3.up, deltaSize);
    }
}
