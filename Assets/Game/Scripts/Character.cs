using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _cc;
    public float MoveSpeed = 5f;
    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;
    private float _verticalVelocity;
    public float gravity = -9.8f;
    private Animator _animator;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    private void CalcPlayerMovement()
    {
        _movementVelocity.Set(_playerInput.horizontalInput, 0f, _playerInput.verticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;

        _animator.SetFloat("Speed", _movementVelocity.magnitude);

        _movementVelocity *= MoveSpeed * Time.deltaTime;

        if (_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);

        Debug.Log(_cc.isGrounded);
        _animator.SetBool("AirBorne",!_cc.isGrounded);
    }

    private void FixedUpdate()
    {
        CalcPlayerMovement();

        if (_cc.isGrounded == false)
            _verticalVelocity = gravity;
        else
            _verticalVelocity = gravity * 0.3f;

        _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;

        _cc.Move(_movementVelocity);
    }
}
