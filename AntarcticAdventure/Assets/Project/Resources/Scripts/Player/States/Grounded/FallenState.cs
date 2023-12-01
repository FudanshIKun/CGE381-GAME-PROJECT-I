using DG.Tweening;
using UnityEngine;

public sealed class FallenState : GroundedState{
	public FallenState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}

	// PRIVATE MEMBERS
	private bool hasCompletedFall;
	private bool hasJumpOff;
	
	// State INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		Debug.Log("[Player] has fallen into " + stateMachine.player.currentFallen.gameObject.name);
		animation.SetRunning(true);
		stateMachine.player.IsFallen = true;
		Fallen(stateMachine.player, stateMachine.player.currentFallen);
		SoundHandler.Instance.PlayCollision();
	}

	public override void OnUpdate(){
		TryToJumpOff(stateMachine.player, stateMachine.player.currentFallen);
	}

	public override void OnExit(){
		base.OnExit();
		animation.StopJumping();
		stateMachine.player.IsFallen = false;
		stateMachine.player.currentFallen = null;
		hasJumpOff = false;
		hasCompletedFall = false;
	}
	
	// PRIVATE METHODS
	private void Fallen(Player player, Fallen fallen){
		Debug.Log("[Player 0] Fallen!");
		player.transform.DOMove(fallen.fallTransform.position, setting.fallenTranslateDuration)
			.OnUpdate(((() => {
				var playerPoint = player.Curve.GetNearestPointTF(player.transform.position);
				var onCompleteDst = player.Curve.TFToDistance(playerPoint);
				player.travelledDst = onCompleteDst;
			})))
			.OnComplete((() => {
				hasCompletedFall = true;
				animation.SetHanging(true);
			}));
	}
	
	private void TryToJumpOff(Player player, Fallen fallen){
		Debug.Log("[Player 0] TryingToJumpOff!");
		if (InputHandler.Instance.IsPressingJump){
			if (!hasCompletedFall)
				return;
			if (hasJumpOff)
				return;
			
			hasJumpOff = true;
			animation.SetJumpOff(true);
			player.transform.DOMove(fallen.jumpOffTransform.position, setting.jumpOffTranslateDuration)
				.OnUpdate((() => {
					var point = player.Curve.GetNearestPointTF(player.transform.position);
					var onCompleteDst = player.Curve.TFToDistance(point);
					player.travelledDst = onCompleteDst;
				}))
				.OnComplete((() => {
					Debug.Log("[Player] has JumpOff from " + stateMachine.player.currentFallen.gameObject.name);
					animation.SetHanging(false);
					player.Speed = 0f;
					stateMachine.ChangeState(stateMachine.RunningState);
				}));
		}
	}
}