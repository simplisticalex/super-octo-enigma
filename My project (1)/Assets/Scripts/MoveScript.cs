using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 15f;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector3 _currentVelocity;
    private Animator _animator;
    private Transform _cameraTransform;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

     
        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        MovePlayer();

        if (_animator != null)
        {
            float horizontalSpeed = _moveInput.magnitude;
            _animator.SetFloat("Speed", horizontalSpeed);
        }
    }

    private void MovePlayer()
    {
        // Берем направление камеры
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * _moveInput.y + right * _moveInput.x).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);

            _controller.Move(moveDirection * _speed * Time.deltaTime);
        }

        if (!_controller.isGrounded)
            _currentVelocity.y += Physics.gravity.y * Time.deltaTime;
        else
            _currentVelocity.y = -0.5f;

        _controller.Move(_currentVelocity * Time.deltaTime);
    }
}