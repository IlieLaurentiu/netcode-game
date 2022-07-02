using Unity.Netcode;
using UnityEngine;

public class PlayerStateHandler : NetworkBehaviour
{
    #region Fields
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

    [Header("Camera & Offset")]
    [SerializeField] private GameObject _cameraClone;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    [SerializeField] private float _offsetZ;

    PlayerBaseState currentState;
    public PlayerGroundedState GroundedState = new PlayerGroundedState();
    #endregion

    #region Netcode
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _cameraClone = Instantiate(_cameraClone, new Vector3(_offsetX, _offsetY, _offsetZ), Quaternion.identity, transform);
            _cameraClone.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 90f, transform.rotation.z);
            _cameraClone.tag = "Untagged";
        }
        GameObject.Find("GameManager").GetComponent<GameManager>().playerCount++;
        transform.name = $"Player No. {GameObject.Find("GameManager").GetComponent<GameManager>().playerCount}";
        GameObject.Find("GameManager").GetComponent<GameManager>().playerList.Add(gameObject);
    }
    #endregion

    #region State Logic
    void Start()
    {
        // Set initial gravity to the value of _gravityForce (given through the inspector in Unity)
        _initialGravityForce = _gravityForce;

        // starting state for the state machine
        currentState = GroundedState;
        // 'this' is a reference to the context (this EXACT MonoBehaviour script)
        currentState.EnterState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        // exits the previous state
        currentState.ExitState(this);
        // transition to the new state passed in as argument
        currentState = state;
        // calls EnterState logic from the new state one time
        state.EnterState(this);
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

    void Update()
    {
        currentState.UpdateState(this);
    }
    #endregion
}