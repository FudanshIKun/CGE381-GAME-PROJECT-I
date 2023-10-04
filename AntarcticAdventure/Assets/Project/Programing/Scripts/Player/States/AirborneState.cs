public abstract class AirborneState : State{
	protected AirborneState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// IState INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		stateMachine.player.IsAirborne = true;
	}

	public override void OnExit(){
		base.OnExit();
		stateMachine.player.IsAirborne = false;
	}
}