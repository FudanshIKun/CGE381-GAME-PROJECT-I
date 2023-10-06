using UnityEngine;

public sealed class JumpingState : AirborneState{
	public JumpingState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// PRIVATE MEMBERS
	private bool isFalling;

	// IState INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		stateMachine.player.IsJumping = true;
		rb.useGravity = true;
		rb.velocity = new Vector3(rb.velocity.x, setting.jumpVelocity, rb.velocity.z);
	}

	public override void OnUpdate(){
		base.OnUpdate();
		Move(stateMachine.player);
	}

	public override void OnExit(){
		base.OnExit();
		stateMachine.player.IsJumping = false;
		rb.velocity = Vector3.zero;
	}
	
	protected override void OnGroundContact(){
		base.OnGroundContact();
		stateMachine.ChangeState(stateMachine.RunningState);
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
			player.Speed = Mathf.Clamp(player.Speed, setting.baseSpeed, setting.maxSpeed);
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
		player.offset = Mathf.Clamp(player.offset, -setting.maxOffset, setting.maxOffset);

		player.height = rb.position.y;
		
		player.travelledDst += Time.deltaTime * player.Speed;
		var point = player.Curve.InterpolateByDistance(player.travelledDst);
		
		player.transform.position = new Vector3(point.x + player.offset, rb.position.y, point.z);
		player.transform.rotation = Quaternion.LookRotation(player.Curve.GetTangentByDistance(player.travelledDst));
	}

	private void OnFall(Player player){
		
	}
}