using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class SmartCameraLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _player;
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [Header("Mouse Settings")]
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _minVerticalAngle = -40f;
    [SerializeField] private float _maxVerticalAngle = 80f;

    [Header("Auto Follow Settings")]
    [SerializeField] private float _autoFollowStrength = 0.5f;

    private float _rotationX = 0f;
    private float _rotationY = 0f;
    private Vector2 _mouseInput;

    public void OnLook(InputValue value) => _mouseInput = value.Get<Vector2>();

    private void Start()
    {
        if (_player != null)
        {
            _rotationY = _player.eulerAngles.y;
        }
    }

    private void LateUpdate()
    {
        if (_player == null) return;

        // Управление мышкой
        _rotationY += _mouseInput.x * _mouseSensitivity;
        _rotationX -= _mouseInput.y * _mouseSensitivity;
        _rotationX = Mathf.Clamp(_rotationX, _minVerticalAngle, _maxVerticalAngle);

        // Автоследование за персонажем
        float playerYaw = _player.eulerAngles.y;
        _rotationY = Mathf.LerpAngle(_rotationY, playerYaw, _autoFollowStrength * Time.deltaTime);

        // Применяем вращение
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
    }
}