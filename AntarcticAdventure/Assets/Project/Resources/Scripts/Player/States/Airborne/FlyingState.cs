using System.Collections;
using UnityEngine;

public sealed class FlyingState : AirborneState{
	public FlyingState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// PRIVATE MEMBERS
	private bool  isFalling;

	// State INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		stateMachine.player.IsFlying = true;
		stateMachine.player.Copter.SetActive(true);
		rb.useGravity = false;
		stateMachine.player.StartCoroutine(Landing(stateMachine.player));
		stateMachine.flyUpAcc = 0f;
		animation.SetFlying(true);
		SoundHandler.Instance.StartCopter();
	}

	public override void OnUpdate(){
		base.OnUpdate();
		Fly(stateMachine.player);
	}

	public override void OnExit(){
		base.OnExit();
		stateMachine.player.IsFlying = false;
		rb.velocity = Vector3.zero;
		animation.SetFlying(false);
		SoundHandler.Instance.StopAmbient();
	}
	
	protected override void OnGroundContact(){
		base.OnGroundContact();
		stateMachine.ChangeState(stateMachine.RunningState);
		animation.PlayImpactAnimation();
	}

	// PRIVATE METHODS
	private void Fly(Player player){
		// Offset Calculation
		if (InputHandler.Instance.IsPressingLeft){
			Debug.Log("[Player 0] Moving left");
			stateMachine.offsetDir = -1;
		}
		else if (InputHandler.Instance.IsPressingRight){
			Debug.Log("[Player 0] Moving right");
			stateMachine.offsetDir = 1;
		}
		
		stateMachine.targetOffset = Mathf.Lerp(stateMachine.targetOffset, stateMachine.offsetDir, setting.offsetAcceleration * Time.deltaTime);
		if (!InputHandler.Instance.IsPressingLeft && !InputHandler.Instance.IsPressingRight)
			stateMachine.targetOffset = 0f;
		player.offset += stateMachine.targetOffset * setting.maxOffset * Time.deltaTime;
		player.offset = Mathf.Clamp(player.offset, -setting.maxOffset, setting.maxOffset);
		
		
		if (player.IsFlying){
			// Height Calculation
			stateMachine.flyUpAcc = setting.flyupSpeed - player.height;
			player.height += stateMachine.flyUpAcc * Time.deltaTime;
			player.height = Mathf.Clamp(player.height, 0, setting.flyHeight);
			// Speed Calculation
			stateMachine.speedAcc = setting.flySpeed - player.Speed;
			player.Speed += stateMachine.speedAcc * Time.deltaTime;
			player.Speed = Mathf.Clamp(player.Speed, setting.baseSpeed, setting.maxSpeed);
			player.travelledDst += Time.deltaTime * player.Speed;
			var point = stateMachine.player.Curve.InterpolateByDistance(player.travelledDst);
			
			player.transform.position = new Vector3(point.x, player.height, point.z) + player.transform.right * player.offset;
		}
		else{
			// Height Calculation
			player.height = rb.position.y;
			
			// Speed Calculation
			if (InputHandler.Instance.IsPressingForward)
				stateMachine.speedAcc = setting.accelerateSpeed - player.Speed;
			else
				stateMachine.speedAcc = setting.baseSpeed - player.Speed;
			
			player.Speed += stateMachine.speedAcc * Time.deltaTime;
			player.Speed = Mathf.Clamp(player.Speed, setting.baseSpeed, setting.maxSpeed);
			player.travelledDst += Time.deltaTime * player.Speed;
			var point = stateMachine.player.Curve.InterpolateByDistance(player.travelledDst);
			
			player.transform.position = new Vector3(point.x, rb.position.y, point.z) + player.transform.right * player.offset;
		}
		
		player.transform.rotation = Quaternion.LookRotation(player.Curve.GetTangentByDistance(player.travelledDst));
	}

	private IEnumerator Landing(Player player){
		yield return new WaitForSeconds(setting.flyDuration);
		yield return new WaitForEndOfFrame();
		player.IsFlying = false;
		rb.useGravity = true;
		player.Copter.SetActive(false);
	}
}