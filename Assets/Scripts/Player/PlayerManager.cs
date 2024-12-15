using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public PlayerData playerData;
    public GameObject playerBody;
    public GameObject playerCamera;

    [HideInInspector] public CharacterController controller;
    public Vector3 velocity;

    private PlayerStateMachine stateMachine;
    private CameraTransparencyHandler transparencyHandler;
    public RaycastHit slopeHit;
    private bool isGrounded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        stateMachine = new PlayerStateMachine();
        stateMachine.Initialize(new PlayerIdleState(stateMachine, this));

        transparencyHandler = new CameraTransparencyHandler(playerCamera.transform, playerBody.transform, LayerMask.GetMask("TransparentObjects"), LayerMask.GetMask("InvisibleObjects"));
        controller = playerBody.GetComponent<CharacterController>();
    }

    private void Update()
    {
        stateMachine.Update();
        transparencyHandler.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();

        if (stateMachine.CurrentState is PlayerIdleState)
        {
            playerData.state = "Idle";
        }
        else if (stateMachine.CurrentState is PlayerWalkState)
        {
            playerData.state = "Walk";
        }
        else if (stateMachine.CurrentState is PlayerRunState)
        {
            playerData.state = "Run";
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += -9.81f * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
