public sealed class IdlingState : State{
	public IdlingState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}

	// State INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		
	}

	public override void OnUpdate(){
		base.OnUpdate();
		Idling();
	}
	
	// PRIVATE METHODS
	private void Idling(){
		
	}
}