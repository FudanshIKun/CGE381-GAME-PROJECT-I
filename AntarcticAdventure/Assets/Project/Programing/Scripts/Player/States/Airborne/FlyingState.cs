using System.Collections;
using UnityEngine;

public sealed class FlyingState : AirborneState{
	public FlyingState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// PRIVATE MEMBERS
	private bool isFalling;

	// IState INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		stateMachine.player.IsFlying = true;
		stateMachine.player.Copter.SetActive(true);
		rb.useGravity = false;
		stateMachine.player.StartCoroutine(Landing(stateMachine.player));
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
		
		// Speed Calculation
		var point = stateMachine.player.Curve.InterpolateByDistance(player.travelledDst);
		if (player.IsFlying){
			stateMachine.speedAcc = setting.flySpeed - player.Speed;
			player.Speed += stateMachine.speedAcc * Time.deltaTime;
			player.Speed = Mathf.Clamp(player.Speed, setting.baseSpeed, setting.maxSpeed);
			player.transform.position = new Vector3(point.x + player.offset, setting.flyHeight, point.z);
		}
		else{
			if (Input.GetKey(KeyCode.W))
				stateMachine.speedAcc = setting.accelerateSpeed - player.Speed;
			else
				stateMachine.speedAcc = setting.baseSpeed - player.Speed;
			
			player.Speed += stateMachine.speedAcc * Time.deltaTime;
			player.Speed = Mathf.Clamp(player.Speed, setting.baseSpeed, setting.maxSpeed);
			player.transform.position = new Vector3(point.x + player.offset, rb.position.y, point.z);
		}
		
		stateMachine.targetOffset = Mathf.Lerp(stateMachine.targetOffset, stateMachine.offsetDir, setting.offsetAcceleration * Time.deltaTime);
		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			stateMachine.targetOffset = 0f;
		player.offset += stateMachine.targetOffset * setting.maxOffset * Time.deltaTime;
		
		player.travelledDst += Time.deltaTime * player.Speed;
		player.offset = Mathf.Clamp(player.offset, -setting.maxOffset, setting.maxOffset);
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