using System;
using UnityEngine;

public sealed class Copter : Interactable, IComparable<Copter>{
	[field: SerializeField]
	public float distance { get; private set; }
	[field: SerializeField]
	public float offset { get; private set; }
	
	// Interactable INTERFACE
	protected override void OnInteract(Player player){
		player.StateMachine.ChangeState(player.StateMachine.FlyingState);
		Destroy(gameObject);
	}
	
	public int CompareTo(Copter other){
		return distance.CompareTo(other.distance);
	}
}