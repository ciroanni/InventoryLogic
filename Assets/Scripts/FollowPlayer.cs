using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class FollowPlayer : MonoBehaviour
{
    // semplice script per far seguire la camera al player, con rotazione e zoom basati sull'input del mouse
    // riscritto perchè quello dello starter assets di unity era scomparso...
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private float targetHeight = 1.6f;

    [Header("Camera Distance")]
    [SerializeField] private float distance = 4f;
    [SerializeField] private float minDistance = 2.5f;
    [SerializeField] private float maxDistance = 6f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 180f;
    [SerializeField] private float minPitch = -25f;
    [SerializeField] private float maxPitch = 70f;

    [Header("Smoothing")]
    [SerializeField] private float positionSmooth = 14f;
    [SerializeField] private float rotationSmooth = 16f;

    [Header("Player Orientation")]
    [SerializeField] private bool rotateTargetWithMoveInput = true;
    [SerializeField] private bool rotateTargetWithLookInput = true;
    [SerializeField] private float targetRotationSmooth = 14f;

    private float _yaw;
    private float _pitch = 15f;
    private float _targetRotationVelocity;

    private void Start()
    {
        if (target == null)
        {
            enabled = false;
            return;
        }

        Vector3 euler = transform.eulerAngles;
        _yaw = euler.y;
        _pitch = euler.x;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector2 lookInput = ReadLookInput();
        _yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        _pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

        if (rotateTargetWithLookInput && lookInput.sqrMagnitude > 0.0001f)
        {
            RotateTargetToYaw(_yaw);
        }

        Vector2 moveInput = ReadMoveInput();
        if (rotateTargetWithMoveInput && moveInput.sqrMagnitude > 0.0001f)
        {
            RotateTargetFromInput(moveInput);
        }

        Quaternion desiredRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        float clampedDistance = Mathf.Clamp(distance, minDistance, maxDistance);
        Vector3 focusPoint = target.position + Vector3.up * targetHeight;
        Vector3 desiredPosition = focusPoint - desiredRotation * Vector3.forward * clampedDistance;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmooth * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmooth * Time.deltaTime);
    }

    private void RotateTargetFromInput(Vector2 moveInput)
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        moveDirection = Quaternion.Euler(0f, _yaw, 0f) * moveDirection;

        float desiredTargetYaw = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        RotateTargetToYaw(desiredTargetYaw);
    }

    private void RotateTargetToYaw(float desiredTargetYaw)
    {
        float smoothedYaw = Mathf.SmoothDampAngle(
            target.eulerAngles.y,
            desiredTargetYaw,
            ref _targetRotationVelocity,
            1f / Mathf.Max(0.01f, targetRotationSmooth));

        target.rotation = Quaternion.Euler(0f, smoothedYaw, 0f);
    }

    private Vector2 ReadLookInput()
    {
#if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null)
        {
            return Mouse.current.delta.ReadValue();
        }
#endif
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private Vector2 ReadMoveInput()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            float x = 0f;
            float y = 0f;

            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
            if (Keyboard.current.sKey.isPressed) y -= 1f;
            if (Keyboard.current.wKey.isPressed) y += 1f;

            return new Vector2(x, y).normalized;
        }
#endif
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
}
