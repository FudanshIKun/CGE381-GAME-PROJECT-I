using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public sealed class Fish : Interactable{
	// PUBLIC MEMBERS
	[Header("Status")]
	public bool hasCollected;
	[Header("Setting")] 
	public bool      jumpOnStart;
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

	private void Start(){
		if (jumpOnStart)
			Jump();
	}

	// Interactable INTERFACE
	public override void OnPlayerApprochead(){
		Jump();
	}

	protected override void OnInteract(Player player){
		hasCollected = true;
		LevelHandler.Instance.score += plusScore;
		LevelHandler.Instance.OnFishContact(this);
		Destroy(gameObject);
	}
	
	// PRIVATE METHODS
	 private void Jump(){
		Debug.Log("A fish has jump from the hole!");
		gameObject.transform.DOJump(dropTransform.position, jumpPower, jumpAmount, jumpDuration);
		StartCoroutine(SelfDestroy());
	 }

	 private IEnumerator SelfDestroy(){
		 yield return new WaitForSeconds(destroyTimer);
		 Destroy(gameObject);
	 }
}