using DG.Tweening;
using UnityEngine;

public sealed class StumblingState : GroundedState{
	public StumblingState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// PRIVATE MEMBERS
	private bool noBouncing;
	private int  bouncedTime;

	// State INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		if (stateMachine.previousState == stateMachine.JumpingState)
			noBouncing = true;
		rb.useGravity = true;
		rb.velocity = Vector3.zero;
		Stumbling(stateMachine.player);
		animation.SetRunning(true);
		animation.StartStumble(animation.stumbleSide);
		SoundHandler.Instance.PlayCollision();
	}

	public override void OnUpdate(){
		var ground = FloatOnGround();
		rb.transform.position = ground != 0
			? new Vector3(stateMachine.player.transform.position.x, ground, stateMachine.player.transform.position.z) 
			: stateMachine.player.transform.position;
	}

	public override void OnExit(){
		base.OnExit();
		bouncedTime = 0;
		noBouncing = false;
		stateMachine.player.IsStumbling = false;
		rb.useGravity = false;
		animation.StopStumble();
	}

	protected override void OnGroundContact(){
		base.OnGroundContact();
		Bouncing();
	}

	// PRIVATE METHODS
	private void Stumbling(Player player){
		player.IsStumbling = true;
		var point = LevelHandler.Instance.Curve.InterpolateByDistance(player.travelledDst);
		
		var targetX = player.AnimationController.stumbleSide
			? Mathf.Clamp(point.x + setting.stumblingDistance, -setting.maxOffset, setting.maxOffset)
			: Mathf.Clamp(point.x - setting.stumblingDistance, -setting.maxOffset, setting.maxOffset);
		
		//rb.AddForce(Vector3.up * Mathf.Sqrt(2 * setting.stumblingJumpPower), ForceMode.VelocityChange);
		rb.AddForce(Vector3.up * setting.stumblingJumpPower, ForceMode.VelocityChange);
		
		player.transform.DOMoveX(targetX, setting.stumblingDuration)
			.OnComplete(() => {
				player.Speed = 0f;
				player.offset = player.transform.position.x;
				stateMachine.ChangeState(stateMachine.RunningState);
			});
	}

	private void Bouncing(){
		if (noBouncing){
			rb.useGravity = false;
			rb.velocity = Vector3.zero;
			return;
		}
		
		switch (bouncedTime){
			case 1:
				bouncedTime ++;
				return;
			case > 1:
				bouncedTime ++;
				Debug.Log("[Player 0] hasBouncedOnce");
				rb.useGravity = false;
				rb.velocity = Vector3.zero;
				return;
		}

		bouncedTime ++;
		rb.velocity = new Vector3(rb.velocity.x, setting.stumblingJumpPower / 1.5f, rb.velocity.z);
		Debug.Log("[Player 0] Bouncing");
	}
}