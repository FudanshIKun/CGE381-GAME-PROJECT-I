using System.Collections;
using UnityEngine;

public sealed class Seal : Interactable{
	// PRIVATE MEMBERS
	[SerializeField] 
	private Animator animator;

	// MonoBehavior INTERFACE
	private void Start(){
		gameObject.SetActive(false);	
	}

	// Interactable INTERFACE
	public override void OnPlayerApprochead(){
		gameObject.SetActive(true);
	}

	protected override void OnInteract(Player player){
		StumblePlayer(player);
	}

	// PRIVATE METHODS
	private void StumblePlayer(Player player){
		animator.SetBool("is_collided", true);
		player.AnimationController.stumbleSide = player.transform.position.x > transform.position.x;
		player.StateMachine.ChangeState(player.StateMachine.StumblingState);
		StartCoroutine(SelfDestroy());
	}
	
	private IEnumerator SelfDestroy(){
		yield return new WaitForSeconds(4);
		Destroy(gameObject);
	}
}