using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine, PlayerManager player)
        : base(stateMachine, player) { }

    public override void Enter()
    {
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (!Mathf.Approximately(horizontal, 0f) || !Mathf.Approximately(vertical, 0f))
        {
            if (player.playerData.canRun && Input.GetKey(KeyCode.LeftShift))
            {
                stateMachine.ChangeState(new PlayerRunState(stateMachine, player));
            }
            else
            {
                stateMachine.ChangeState(new PlayerWalkState(stateMachine, player));
            }
        }
    }

    
}
