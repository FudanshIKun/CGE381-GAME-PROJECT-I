using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
public sealed class Fish : Interactable{
	// PUBLIC MEMBERS
	[Header("Status")]
	public bool hasCollected;
	[Header("Setting")] 
	public Transform dropTransform;
	public float dropOffset;
	
	public int   plusScore    = 5;
	public float jumpPower    = 1;
	public int   jumpAmount   = 1;
	public float jumpDuration = 1;
	public float destroyTimer = 5f;
	
	// MonoBehavior INTERFACE
	private void Update(){
		if (Application.isEditor && !Application.isPlaying){
			if (dropTransform == null){
				dropTransform = Instantiate(new GameObject("Fish's drop transform"), transform.position, Quaternion.identity, transform).GetComponent<Transform>();
			}else
				dropTransform.position = new Vector3(transform.position.x, 0, transform.position.z) 
				                         + transform.parent.right * dropOffset;
		}
	}

	// Interactable INTERFACE
	public override void OnPlayerApprochead(){
		Jump();
	}

	protected override void OnInteract(Player player){
		hasCollected = true;
		LevelHandler.Instance.score += plusScore;
		LevelHandler.Instance.OnFishContact(this);
		SoundHandler.Instance.PlayScore();
		Destroy(gameObject);
	}
	
	// PRIVATE METHODS
	 private void Jump(){
		gameObject.transform.DOJump(dropTransform.position, jumpPower, jumpAmount, jumpDuration);
		SoundHandler.Instance.PlayFishJump();
		StartCoroutine(SelfDestroy());
	 }

	 private IEnumerator SelfDestroy(){
		 yield return new WaitForSeconds(destroyTimer);
		 Destroy(gameObject);
	 }
}