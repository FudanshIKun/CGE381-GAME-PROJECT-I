using UnityEngine;

public abstract class Interactable : MonoBehaviour{
	private bool hasInteracted;

	public virtual void OnPlayerApprochead(){
	}
	protected abstract void OnInteract(Player player);
	
	private void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag("Player")){
			if (hasInteracted)
				return;
			
			OnInteract(other.gameObject.GetComponent<Player>());
			hasInteracted = true;
		}
	}
}