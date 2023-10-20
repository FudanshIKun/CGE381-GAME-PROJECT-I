using UnityEngine;

public abstract class Interactable : MonoBehaviour{
	private bool hasInteracted;

	public virtual void OnPlayerApprochead(){
	}
	protected abstract void OnInteract(Player player);
	
	// MonoBehavior INTERFACE
	private void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag("Player")){
			if (hasInteracted)
				return;

			var player = other.gameObject.GetComponent<Player>();
			Debug.Log("Player has interacted with " + GetType().Name);
			OnInteract(player);
			hasInteracted = true;
		}
	}
}