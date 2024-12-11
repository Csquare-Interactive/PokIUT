using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public PlayerData playerData;
    public GameObject playerBody;

    [HideInInspector] public Rigidbody rb;

    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        rb = playerBody.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        stateMachine = new PlayerStateMachine();
        stateMachine.Initialize(new PlayerIdleState(stateMachine, this));
    }

    private void Update()
    {
        stateMachine.Update();

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
    }
}
