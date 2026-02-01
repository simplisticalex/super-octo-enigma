using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 10f;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector3 _currentVelocity;
    private Animator _animator;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        MovePlayer();
        float horizontalSpeed = new Vector3(_moveInput.x, 0, _moveInput.y).magnitude;
        _animator.SetFloat("Speed", horizontalSpeed);
    }

    private void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);

            _controller.Move(moveDirection * _speed * Time.deltaTime);
        }

        if (!_controller.isGrounded)
        {
            _currentVelocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _currentVelocity.y = -0.5f;
        }

        _controller.Move(_currentVelocity * Time.deltaTime);
    }
}