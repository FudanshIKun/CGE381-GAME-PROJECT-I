using DG.Tweening;
using UnityEngine;

public sealed class StumblingState : GroundedState{
	public StumblingState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// PRIVATE MEMBERS
	private bool    noBouncing;
	private int     bouncedTime;
	private bool    finishedX;
	private bool    finishedZ;
	private bool    finished;
	private Vector3 targetPosition;

	// State INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		if (stateMachine.previousState == stateMachine.JumpingState)
			noBouncing = true;
		rb.useGravity = true;
		rb.velocity = Vector3.zero;
		finishedX = false;
		finishedZ = false;
		finished = false;
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

		if (finishedX && finishedZ){
			if (finished)
				return;
			
			Debug.Log("finished");
			stateMachine.player.Speed = 0f;
			var middle = LevelHandler.Instance.Curve.Interpolate(LevelHandler.Instance.Curve.GetNearestPointTF(stateMachine.player.transform.position));
			var playerPos = new Vector3(stateMachine.player.transform.position.x, middle.y, stateMachine.player.transform.position.z);
			var directionToMiddle= (playerPos - middle).normalized;
			var offsetDirection = Vector3.Dot(directionToMiddle, stateMachine.player.transform.right);
			var offset = Vector3.Distance(playerPos, middle) * offsetDirection;
			stateMachine.player.offset = offset;
			stateMachine.ChangeState(stateMachine.RunningState);
			finished = true;
		}
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
		targetPosition = player.transform.position;

		if (player.AnimationController.stumbleSide){
			Debug.Log("[Player 0] Stumbling to right");
			targetPosition += player.transform.right * setting.stumblingDistance;
		}
		else{
			Debug.Log("[Player 0] Stumbling to left");
			targetPosition -= player.transform.right * setting.stumblingDistance;
		}
		
		//rb.AddForce(Vector3.up * Mathf.Sqrt(2 * setting.stumblingJumpPower), ForceMode.VelocityChange);
		rb.AddForce(player.transform.up * setting.stumblingJumpPower, ForceMode.VelocityChange);
		
		player.transform.DOMoveX(targetPosition.x, setting.stumblingDuration)
			.OnComplete(() => {
				finishedX = true;
			});
		
		player.transform.DOMoveZ(targetPosition.z, setting.stumblingDuration)
			.OnComplete(() => {
				finishedZ = true;
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