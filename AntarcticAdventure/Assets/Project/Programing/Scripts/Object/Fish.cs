using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public sealed class Fish : Interactable{
	// PUBLIC MEMBERS
	[Header("Setting")]
	public Transform dropTransform;
	
	public int   plusScore    = 5;
	public float jumpPower    = 1;
	public int   jumpAmount   = 1;
	public float jumpDuration = 1;
	public float destroyTimer = 5f;
	
	// PRIVATE MEMBERS
	[SerializeField]
	private Animator animator;
	
	// MonoBehavior INTERFACE
	private void OnValidate(){
		animator ??= GetComponent<Animator>();
	}

	// Interactable INTERFACE
	public override void OnPlayerApprochead(){
		Jump();
	}

	protected override void OnInteract(Player player){
		LevelHandler.Instance.score += plusScore;
		Destroy(gameObject);
	}
	
	// PRIVATE METHODS
	 private void Jump(){
		Debug.Log("A fish has jump from the hole!");
		gameObject.transform.DOJump(dropTransform.position, jumpPower, jumpAmount, jumpDuration);
		StartCoroutine(Destroy());
	 }

	 private IEnumerator Destroy(){
		 yield return new WaitForSeconds(destroyTimer);
		 Destroy(gameObject);
	 }
}