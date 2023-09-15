using System;
using UnityEngine;

public sealed class Fish : MonoBehaviour{
	[SerializeField]
	private Animator animator;
	
	// MonoBehavior Interface
	private void OnValidate(){
		animator ??= GetComponent<Animator>();
	}

	private void OnCollisionEnter(Collision other){
		
	}
}