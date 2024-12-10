public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerManager player;

    protected PlayerState(PlayerStateMachine stateMachine, PlayerManager player)
    {
        this.stateMachine = stateMachine;
        this.player = player;
    }

    public virtual void Enter() {}
    public virtual void Update() {}
    public virtual void Exit() {}
}
