using System.Collections;
using UnityEngine;

public sealed class Fallen : Interactable{
	public override void OnPlayerApprochead(){
	}

	protected override void OnInteract(Player player){
		Debug.Log("Player fall into " + gameObject.name);
		player.StateMachine.ChangeState(player.StateMachine.FallenState);
	}
}