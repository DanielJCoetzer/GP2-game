using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_FirstPersonController : MonoBehaviour {
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;


    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    [Header("Dash Parameters")]
    [SerializeField] private float dashDistance = 10.0f;
    [SerializeField] private float dashSpeed = 50.0f;
    [SerializeField] private float dashCooldown = 2.0f;

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SCR_PlayerInputHandler playerInputHandler;


    Vector3 _currentMovement;
    float _verticalRotation;
    bool _isDashing = false;
    bool _dashOnCooldown = false;
    private float CurrentSpeed => walkSpeed * (playerInputHandler.SprintTriggered ? sprintMultiplier : 1);


    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update() {
        if (!_isDashing) {
            Movement();
            Rotation();

            if (playerInputHandler.DashTriggered && !_dashOnCooldown)
            {
                StartCoroutine(PerformDash());
            }
        }

    }


    private Vector3 CalculateWorldDirection() {
        Vector3 _inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 _worldDirection = transform.TransformDirection(_inputDirection);
        return _worldDirection.normalized;
    }


    private void HandleJumping() {
        if (characterController.isGrounded)
        {
            _currentMovement.y = -0.5f;


            if (playerInputHandler.JumpTriggered)
            {
                _currentMovement.y = jumpForce;
            }
        }
        else
        {
            _currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }


    private void Movement() {
        Vector3 _worldDirection = CalculateWorldDirection();
        _currentMovement.x = _worldDirection.x * CurrentSpeed;
        _currentMovement.z = _worldDirection.z * CurrentSpeed;


        HandleJumping();
        characterController.Move(_currentMovement * Time.deltaTime);
    }


    private void HorizontalRotate(float rotationAmount) {
        transform.Rotate(0, rotationAmount, 0);
    }


    private void VerticalRotate(float rotationAmount) {
        _verticalRotation = Mathf.Clamp(_verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }


    private void Rotation() {
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;


        HorizontalRotate(mouseXRotation);
        VerticalRotate(mouseYRotation);
    }

    private IEnumerator PerformDash() {
        _isDashing = true;
        _dashOnCooldown = true;
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0, playerInputHandler.MovementInput.y);
        Vector3 dashDirection = inputDirection != Vector3.zero
            ? transform.TransformDirection(inputDirection.normalized) 
            : transform.forward; // Default to forward if there isn't a direciton pressed
        //Vector3 dashDirection = transform.forward;
        float dashTravelled = 0f;
        while (dashTravelled < dashDistance)
        {
            float dashStep = dashSpeed * Time.deltaTime;
            dashTravelled += dashStep;
            characterController.Move(dashDirection * dashStep);
            if (characterController.collisionFlags == CollisionFlags.CollidedSides)
            {
                break;
            }

            yield return null;
        }

        _isDashing = false;


        yield return new WaitForSeconds(dashCooldown);
        _dashOnCooldown = false;
    }


}

