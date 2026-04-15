using UnityEngine;

/// <summary>
/// Déplacement simple clavier/souris pour test rapide dans la serre (mode non-VR).
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class SimpleGreenhouseMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float lookSpeed = 90f;

    private float _verticalVelocity;
    private float _pitch;

    private void Reset()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        UpdateLook();
        UpdateMove();
    }

    private void UpdateLook()
    {
        var mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        if (cameraPivot == null)
        {
            return;
        }

        _pitch = Mathf.Clamp(_pitch - mouseY, -70f, 70f);
        cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void UpdateMove()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputZ = Input.GetAxis("Vertical");

        var moveDirection = (transform.right * inputX + transform.forward * inputZ) * moveSpeed;

        if (characterController.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }

        _verticalVelocity += gravity * Time.deltaTime;
        moveDirection.y = _verticalVelocity;

        characterController.Move(moveDirection * Time.deltaTime);
    }
}
