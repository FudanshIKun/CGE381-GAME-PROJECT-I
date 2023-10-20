using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public sealed class StumblingState : GroundedState{
	public StumblingState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}

	public override void OnEnter(){
		base.OnEnter();
		Stumbling(stateMachine.player);
		animation.StartRunning();
		animation.StartStumble(animation.stumbleSide);
	}

	public override void OnUpdate(){
	}

	public override void OnExit(){
		base.OnExit();
		stateMachine.player.IsStumbling = false;
		animation.StopStumble();
	}

	// PRIVATE METHODS
	private void Stumbling(Player player){
		player.IsStumbling = true;
		
		var ground = FloatOnGround();
		var point = LevelHandler.Instance.Curve.InterpolateByDistance(player.travelledDst);
		
		if (player.AnimationController.stumbleSide){
			player.transform.DOJump(new Vector3(Mathf.Clamp(point.x + setting.stumblingDistance, -setting.maxOffset, setting.maxOffset), ground, point.z), setting.stumblingJumpPower, setting.stumblingJumpAmount, setting.stumblingDuration)
				.OnUpdate((() => {
					ground = FloatOnGround();
				}))
				.OnComplete((() => {
					player.Speed = 0f;
					player.offset = player.transform.position.x;
					stateMachine.ChangeState(stateMachine.RunningState);
				}));
		}
		else{
			player.transform.DOJump(new Vector3(Mathf.Clamp(point.x - setting.stumblingDistance, -setting.maxOffset, setting.maxOffset), ground, point.z), setting.stumblingJumpPower, setting.stumblingJumpAmount, setting.stumblingDuration)
				.OnUpdate((() => {
					ground = FloatOnGround();
				}))
				.OnComplete((() => {
					player.Speed = 0f;
					player.offset = player.transform.position.x;
					stateMachine.ChangeState(stateMachine.RunningState);
				}));
		}
	}
}