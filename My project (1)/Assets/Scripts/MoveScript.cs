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
    private Animator _animator;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        Debug.Log("Сигнал от кнопок дошел! Ввод: " + _moveInput);
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
            _controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }
}