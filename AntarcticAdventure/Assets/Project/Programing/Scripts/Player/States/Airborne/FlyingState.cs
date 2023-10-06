using System.Collections;
using UnityEngine;

public sealed class FlyingState : AirborneState{
	public FlyingState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// PRIVATE MEMBERS
	private bool  isFalling;

	// IState INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		stateMachine.player.IsFlying = true;
		stateMachine.player.Copter.SetActive(true);
		rb.useGravity = false;
		stateMachine.player.StartCoroutine(Landing(stateMachine.player));
		stateMachine.flyupAcc = 0f;
	}

	public override void OnUpdate(){
		base.OnUpdate();
		Fly(stateMachine.player);
	}

	public override void OnExit(){
		base.OnExit();
		stateMachine.player.IsFlying = false;
		rb.velocity = Vector3.zero;
	}
	
	protected override void OnGroundContact(){
		base.OnGroundContact();
		stateMachine.ChangeState(stateMachine.RunningState);
	}

	// PRIVATE METHODS
	private void Fly(Player player){
		// Offset Calculation
		if (Input.GetKey(KeyCode.A)){
			Debug.Log("Moving left");
			stateMachine.offsetDir = -1;
		}
		else if (Input.GetKey(KeyCode.D)){
			Debug.Log("Moving right");
			stateMachine.offsetDir = 1;
		}
		
		stateMachine.targetOffset = Mathf.Lerp(stateMachine.targetOffset, stateMachine.offsetDir, setting.offsetAcceleration * Time.deltaTime);
		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			stateMachine.targetOffset = 0f;
		player.offset += stateMachine.targetOffset * setting.maxOffset * Time.deltaTime;
		player.offset = Mathf.Clamp(player.offset, -setting.maxOffset, setting.maxOffset);
		
		// Speed Calculation
		if (player.IsFlying){
			// Height Calculation
			stateMachine.flyupAcc = setting.flyupSpeed - player.height;
			player.height += stateMachine.flyupAcc * Time.deltaTime;
			player.height = Mathf.Clamp(player.height, 0, setting.flyHeight);
			// Speed Calculation
			stateMachine.speedAcc = setting.flySpeed - player.Speed;
			player.Speed += stateMachine.speedAcc * Time.deltaTime;
			player.Speed = Mathf.Clamp(player.Speed, setting.baseSpeed, setting.maxSpeed);
			player.travelledDst += Time.deltaTime * player.Speed;
			var point = stateMachine.player.Curve.InterpolateByDistance(player.travelledDst);
			
			player.transform.position = new Vector3(point.x + player.offset, player.height, point.z);
		}
		else{
			// Speed Calculation
			if (Input.GetKey(KeyCode.W))
				stateMachine.speedAcc = setting.accelerateSpeed - player.Speed;
			else
				stateMachine.speedAcc = setting.baseSpeed - player.Speed;
			
			player.height = rb.position.y;
			player.Speed += stateMachine.speedAcc * Time.deltaTime;
			player.Speed = Mathf.Clamp(player.Speed, setting.baseSpeed, setting.maxSpeed);
			player.travelledDst += Time.deltaTime * player.Speed;
			var point = stateMachine.player.Curve.InterpolateByDistance(player.travelledDst);
			
			player.transform.position = new Vector3(point.x + player.offset, rb.position.y, point.z);
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
	
	private void OnFall(Player player){
		
	}
}