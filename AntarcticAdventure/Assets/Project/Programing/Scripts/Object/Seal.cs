using DG.Tweening;
using UnityEngine;

public sealed class Seal : Interactable{
	// PRIVATE MEMBERS
	[SerializeField] 
	private float targetHeight = .4f;

	[SerializeField] 
	private float showUpDuration = 1f;
	
	// Interactable INTERFACE
	public override void OnPlayerApprochead(){
		transform.DOMoveY(targetHeight, showUpDuration);
	}

	protected override void OnInteract(Player player){
		StumblePlayer(player);
	}

	private void StumblePlayer(Player player){
		player.AnimationController.stumbleSide = player.transform.position.x > transform.position.x;
		player.StateMachine.ChangeState(player.StateMachine.StumblingState);
	}
}