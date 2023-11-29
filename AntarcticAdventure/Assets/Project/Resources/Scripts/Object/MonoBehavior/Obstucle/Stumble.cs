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
		player.AnimationController.stumbleSide = player.transform.position.x > transform.position.x;
		player.StateMachine.ChangeState(player.StateMachine.StumblingState);
	}
}