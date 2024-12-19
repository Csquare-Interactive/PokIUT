using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public PlayerData playerData;
    public GameObject playerBody;
    public GameObject playerCamera;

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;

    private PlayerStateMachine stateMachine;
    private CameraTransparencyHandler transparencyHandler;

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
        rb = playerBody.GetComponent<Rigidbody>();
        animator = playerBody.GetComponent<Animator>();

        stateMachine = new PlayerStateMachine();
        stateMachine.Initialize(new PlayerIdleState(stateMachine, this));

        transparencyHandler = new CameraTransparencyHandler(playerCamera.transform, playerBody.transform, LayerMask.GetMask("TransparentObjects"), LayerMask.GetMask("InvisibleObjects"));
    }

    private void Update()
    {
        stateMachine.Update();

        transparencyHandler.Update();
    }
}
