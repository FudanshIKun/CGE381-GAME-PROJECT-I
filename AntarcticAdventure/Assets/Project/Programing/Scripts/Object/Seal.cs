using UnityEngine;

public sealed class Seal : Interactable{
	public override void OnPlayerApprochead(){
		ShowUp();
	}

	protected override void OnInteract(Player player){
		KnockPlayerBack(player);
	}

	private void ShowUp(){
		Debug.Log("A seal has show up!");
	}

	private void KnockPlayerBack(Player player){
		
	}
}