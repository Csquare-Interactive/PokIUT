using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine, PlayerManager player)
        : base(stateMachine, player) { }

    public override void Enter()
    {
        player.animator.SetBool("isWalking", false);
        player.animator.SetBool("isRunning", false);
    }

    public override void Update()
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

        // Handle Animation
        if (horizontal > 0 && !player.facingRight)
        {
            player.facingRight = true;
            player.animator.SetBool("facingRight", true);
        }
        else if (horizontal < 0 && player.facingRight)
        {
            player.facingRight = false;
            player.animator.SetBool("facingRight", false);
        }
    }
}
