﻿using UnityEngine;

public abstract class State : IState{
	protected State(StateMachine stateMachine, Setting setting){
		this.setting = setting;
		this.stateMachine = stateMachine;
		tf = stateMachine.player.transform;
		rb = stateMachine.player.RB;
		animation = stateMachine.player.AnimationController;
	}
	
	// PROTECTED MEMBERS
	protected readonly StateMachine    stateMachine;
	protected readonly Setting         setting;
	protected          Transform       tf        { get; }
	protected          Rigidbody       rb        { get; }
	protected          PlayerAnimation animation { get; }
	
	// State INTERFACE
	public virtual void OnEnter(){
		if (stateMachine.debugState)
			Debug.Log($"[Player 0] OnEnter {GetType().Name} state.");
	}

	public virtual void OnFixedUpdate(){
		if (stateMachine.debugState)
			Debug.Log($"[Player 0] OnFixedUpdate {GetType().Name} state.");
	}

	public virtual void OnUpdate(){
		if (stateMachine.debugState)
			Debug.Log($"[Player 0] OnUpdate {GetType().Name} state.");
	}

	public virtual void OnExit(){
		if (stateMachine.debugState)
			Debug.Log($"[Player 0] OnExit {GetType().Name} state.");
	}

	public virtual void OnTriggerEnter(Collider collider){
		if (setting.IsGroundLayer(collider.gameObject.layer))
			OnGroundContact();
	}

	public virtual void OnTriggerExit(Collider collider){
		if (setting.IsGroundLayer(collider.gameObject.layer))
			OnGroundContactLost();
	}

	// PROTECTED METHODS
	protected virtual void OnGroundContact(){
		if (stateMachine.debugState)
			Debug.Log($"[Player] OnGroundContact {GetType().Name} state.");
	}

	protected virtual void OnGroundContactLost(){
		if (stateMachine.debugState)
			Debug.Log($"[Player] OnGroundContactLost {GetType().Name} state.");
	}
}