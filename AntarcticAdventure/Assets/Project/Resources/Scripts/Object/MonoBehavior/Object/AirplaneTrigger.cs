using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public sealed class AirplaneTrigger : MonoBehaviour, IComparable<AirplaneTrigger>{
	[field: SerializeField]
	public float distance { get; private set; }
	[field: SerializeField]
	public GameObject Airplane { get; set; }
	[field: SerializeField]
	public Transform Destination { get; set; }

	[SerializeField] 
	private float duration = 2f;
	
	// PRIVATE MEMBERS
	private bool hasPlayed;
    
	// MonoBehavior INTERFACE
	private void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("Player")){
			if (hasPlayed)
				return;
            
			FlyPassBy();
			hasPlayed = true;
		}
	}
	
	// IComparable INTERFACE
	public int CompareTo(AirplaneTrigger other){
       return distance.CompareTo(other.distance);
    }
	
	// PRIVATE METHODS
	private void FlyPassBy(){
		Airplane.transform.DOMove(Destination.position, duration)
			.OnStart(() => {
				Debug.Log($"{name} has start flying pass by!");
			});
	}
}