using DG.Tweening;
using UnityEngine;

public sealed class Stumble : Interactable{
	public override void OnPlayerApprochead(){
	}

	protected override void OnInteract(Player player){
		if (player.StateMachine.currentState != player.StateMachine.FlyingState)
			StumblePlayer(player);
	}

	private void StumblePlayer(Player player){
		var playerPosition = Vector3.Dot(player.transform.position - transform.position, transform.right);
		var stumblePosition = Vector3.Dot(transform.position - transform.position,       transform.right);
		player.AnimationController.stumbleSide = playerPosition > stumblePosition;
		player.AnimationController.stumbleSide = playerPosition > stumblePosition;
		player.StateMachine.ChangeState(player.StateMachine.StumblingState);
	}
}