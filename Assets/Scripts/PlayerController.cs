using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Simple character controller for movement
/// </summary>
public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    #region Movement

    [SerializeField] private float acceleration = 80;
    [SerializeField] private float maxVelocity = 10;
    private Vector3 playerInput;
    private Rigidbody rigidBody;

    private void HandleMovement()
    {
        rigidBody.velocity += playerInput.normalized * (acceleration * Time.deltaTime);
        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxVelocity);
    }

    #endregion

    #region Rotation

    [SerializeField] private float rotationSpeed = 450;
    private Plane groundPlane = new(Vector3.up, Vector3.zero);
    private Camera mainCamera;

    private void HandleRotation()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out var enter))
        {
            var hitPoint = ray.GetPoint(enter);

            var dir = hitPoint - transform.position;
            var rot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }
    }

    #endregion
}