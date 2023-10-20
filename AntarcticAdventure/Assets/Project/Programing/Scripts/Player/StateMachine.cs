using System;
using UnityEngine;

[Serializable]
public sealed class StateMachine{
	// PUBLIC MEMBERS
	public Player         player         { get; }
	public RunningState   RunningState   { get; }
	public JumpingState   JumpingState   { get; }
	public FlyingState    FlyingState    { get; }
	public StumblingState StumblingState { get; }
	public FallenState    FallenState    { get; }
	public FinishedState  FinishedState  { get; }
	
	public float speedAcc;
	public float flyUpAcc;
	public float targetOffset;
	public int   offsetDir;
	
	// PRIVATE MEMBERS
	private IState previousState { get; set; }
	private IState currentState  { get; set; }
	
	// PROTECTED METHODS
	public StateMachine(Player player, Setting setting){
		this.player = player;

		RunningState = new RunningState(this, setting);
		JumpingState = new JumpingState(this, setting);
		FlyingState = new FlyingState(this, setting);
		StumblingState = new StumblingState(this, setting);
		FallenState = new FallenState(this, setting);
		FinishedState = new FinishedState(this, setting);
	}

	// PUBLIC METHODS
	public void OnStart(){
		Debug.Log(GetType() + ".OnStart()");
		ChangeState(RunningState);
	}
	
	public void OnFixedUpdate(){
		currentState?.OnFixedUpdate();
	}

	public void OnUpdate(){
		currentState?.OnUpdate();
	}

	public void ChangeState(IState state){
		if (state == currentState)
			return;
		
		currentState?.OnExit();
		previousState = currentState;
		currentState = state;
		currentState.OnEnter();
	}

	public void OnTriggerEnter(Collider collider){
		currentState?.OnTriggerEnter(collider);
	}

	public void OnTriggerExit(Collider collider){
		currentState?.OnTriggerExit(collider);
	}
}