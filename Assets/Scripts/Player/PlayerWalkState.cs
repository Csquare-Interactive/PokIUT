using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(PlayerStateMachine stateMachine, PlayerManager player) 
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

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            MovePlayer(moveDirection, player.playerData.walkSpeed);
        }
        else
        {
            Vector3 stoppedVelocity = player.velocity;
            stoppedVelocity.y = -9.81f * Time.deltaTime;
            stoppedVelocity.z = 0f;
            player.controller.Move(stoppedVelocity * Time.deltaTime);
            stateMachine.ChangeState(new PlayerIdleState(stateMachine, player));
        }

        if (player.playerData.canRun && Input.GetKey(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(new PlayerRunState(stateMachine, player));
        }
    }

    private void MovePlayer(Vector3 direction, float speed)
    {
        player.controller.Move(direction * speed * Time.deltaTime);
    }
}
