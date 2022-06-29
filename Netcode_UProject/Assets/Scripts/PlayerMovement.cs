using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Directional Movement")]
    [SerializeField] private Rigidbody _playerRigidBody;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _gravityForce;

    [Header("Jump Settings & Logic")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkSphereRadius;
    private bool isGrounded;
    private float _initialGravityForce;

    [Header("Camera Offset")]
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    [SerializeField] private float _offsetZ;

    #region NetCode
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);

        //find the main camera gameobject
        GameObject camera = Camera.main.gameObject;
        camera.transform.SetParent(transform);
        //Maybe set the right position as well
        camera.transform.position = transform.position + new Vector3(_offsetX, _offsetY, _offsetZ);
    }
    #endregion

    #region Movement
    private void Start()
    {
        // Set initial gravity to the value of _gravityForce (given through the inspector in Unity)
        _initialGravityForce = _gravityForce;
    }

    private void FixedUpdate()
    {
        // Check for ground below the player
        isGrounded = Physics.CheckSphere(groundCheck.position, checkSphereRadius, layerMask);

        // Read player Input for the Horizontal and Vertical axis, multiply it by _playerSpeed and then by Time.fixedDeltaTime to keep it constant with the physics update
        float _horizontalInput = Input.GetAxis("Horizontal") * _playerSpeed * Time.fixedDeltaTime;
        float _verticalInput = Input.GetAxis("Vertical") * _playerSpeed * Time.fixedDeltaTime;

        // Multiplying by Time.fixedDeltaTime here is unnecessary, because velocity is distance/time. The Rigidbody uses Time.deltaTime internally already
        _playerRigidBody.velocity = -transform.forward * _horizontalInput + transform.right * _verticalInput + transform.up * _gravityForce;

        // if the player is grounded, set our custom gravity to 0. Doing so keeps the rigidbody's default gravity and let's the player move smoother
        if (isGrounded) 
        { 
            _gravityForce = 0;
        }
        else 
        {
            _gravityForce = _initialGravityForce; 
        }
    }
    #endregion
}
