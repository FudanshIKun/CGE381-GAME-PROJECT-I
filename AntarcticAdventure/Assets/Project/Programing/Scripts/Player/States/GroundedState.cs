using UnityEngine;

public abstract class GroundedState : State{
	protected GroundedState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// State INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		stateMachine.player.IsGrounded = true;
	}

	public override void OnExit(){
		base.OnExit();
		stateMachine.player.IsGrounded = false;
	}

	// PROTECTED METHODS
	protected float FloatOnGround(){
		var center = stateMachine.player.BodyCollider.center;
		var downward = new Ray(stateMachine.player.transform.position + center, Vector3.down);
		var groundLayer = LayerMask.GetMask("Ground");

		if (Physics.Raycast(downward, out var hit, setting.floatRay, groundLayer, QueryTriggerInteraction.Ignore)){
			// Adjust the player's position to float above the ground.
			return hit.point.y + setting.floatHeight;
		}
		
		return 0;
	}
}