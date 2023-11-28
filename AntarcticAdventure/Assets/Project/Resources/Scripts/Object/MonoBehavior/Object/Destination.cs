using System;
using UnityEngine;

public class Destination : MonoBehaviour{
	[field: SerializeField]
	public float distance { get; private set; }
	[field: SerializeField]
	public Vector3 size { get; private set; }
	[SerializeField] 
	private BoxCollider Box;

	// MonoBehavior INTERFACE
	private void OnValidate(){
		Box ??= GetComponent<BoxCollider>();
		Box.size = size;
		var halfHeight = Mathf.Abs(Box.center.y - Box.size.y / 2f);
		Box.center = new Vector3(0f, halfHeight, 0f);
	}

	private void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("Player")){
			Debug.Log("[Player 0] has reach " + GetType().Name);
			var player = other.gameObject.GetComponent<Player>();
			if (player.HasFinished)
				return;
			
			player.StateMachine.ChangeState(player.StateMachine.FinishedState);
		}
	}
}