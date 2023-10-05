﻿using UnityEngine;

public sealed class RunningState : GroundedState{
	public RunningState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}

	public override void OnEnter(){
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
	}

	// IState INTERFACE
	public override void OnUpdate(){
		base.OnUpdate();
		Move(stateMachine.player);
		Jump();
	}

	// PRIVATE METHODS
	private void Move(Player player){
		// Speed Calculation
		if (player.Speed < setting.baseSpeed){
			player.Speed += setting.baseSpeed * Time.deltaTime;
		}
		else{
			if (player.IsFlying){
				Debug.Log("Flying");
				stateMachine.speedAcc = setting.flySpeed - player.Speed;
			}
			else{
				if (Input.GetKey(KeyCode.W)){
					Debug.Log("Moving forward");
					stateMachine.speedAcc = setting.accelerateSpeed - player.Speed;
				}
				else{
					stateMachine.speedAcc = setting.baseSpeed - player.Speed;
				}
			}

			player.Speed += stateMachine.speedAcc * Time.deltaTime;
			player.Speed = Mathf.Clamp(stateMachine.player.Speed, setting.baseSpeed, setting.maxSpeed);
		}
		
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
		
		player.travelledDst += Time.deltaTime * stateMachine.player.Speed;
		player.offset = Mathf.Clamp(stateMachine.player.offset, -setting.maxOffset, setting.maxOffset);
		var point = stateMachine.player.Curve.InterpolateByDistance(stateMachine.player.travelledDst);
		var floating = FloatOnGround();
		
		player.transform.position = new Vector3(point.x + player.offset, floating, point.z);
		player.transform.rotation = Quaternion.LookRotation(player.Curve.GetTangentByDistance(player.travelledDst));
	}

	private void Jump(){
		if (Input.GetKeyDown(KeyCode.Space)){
			Debug.Log("Jump");
			stateMachine.ChangeState(stateMachine.JumpingState);
		}
	}
}