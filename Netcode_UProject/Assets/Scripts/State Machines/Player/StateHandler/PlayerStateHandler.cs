using Unity.Netcode;
using UnityEngine;

public class PlayerStateHandler : NetworkBehaviour
{
    #region Fields
    [Header("Directional Movement")]
    public float gravityForce;
    [SerializeField] public Rigidbody playerRigidBody;
    [SerializeField] private float _playerSpeed;

    [Header("Jump Settings & Logic")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkSphereRadius;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public float initialGravityForce;

    [Header("Camera & Offset")]
    [SerializeField] private GameObject _cameraClone;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    [SerializeField] private float _offsetZ;
    public float horizontalInput;
    public float verticalInput;

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

    #region State Transition Logic
    void Start()
    {
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
        // Read player Input for the Horizontal and Vertical axis, multiply it by _playerSpeed and then by Time.fixedDeltaTime to keep it constant with the physics update
        horizontalInput = Input.GetAxis("Horizontal") * _playerSpeed * Time.fixedDeltaTime;
        verticalInput = Input.GetAxis("Vertical") * _playerSpeed * Time.fixedDeltaTime;

        // Check for ground below the player
        isGrounded = Physics.CheckSphere(groundCheck.position, checkSphereRadius, layerMask);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }
    #endregion
}