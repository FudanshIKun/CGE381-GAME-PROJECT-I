using System;
using DG.Tweening;
using FluffyUnderware.Curvy;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public sealed class Fallen : Interactable{
	// PUBLIC MEMBERS
	public Hole      parentHole;
	[Tooltip("X's value doesn't included in calculation")]
	public Transform fallTransform;
	[Tooltip("X's value doesn't included in calculation")]
	public Transform jumpOffTransform;
	public Vector3   fallOffset;
	public Vector3   jumpOffset;
	
	// PRIVATE MEMBERS
	[Header("Required")]
	[SerializeField] 
	private CurvySpline Curve;

	private void OnValidate(){
		parentHole ??= GetComponentInParent<Hole>();
		Curve ??= FindObjectOfType<CurvySpline>();
	}

	private void OnEnable(){
		if (Application.isEditor && !Application.isPlaying){
			if (fallTransform == null)
				fallTransform = Instantiate(new GameObject("Fall Transform"), transform).GetComponent<Transform>();

			if (jumpOffTransform == null)
				jumpOffTransform = Instantiate(new GameObject("JumpOff Transform"), transform).GetComponent<Transform>();
		} 
	}

	private void Start(){
		parentHole ??= GetComponentInParent<Hole>();
	}

	private void Update(){
		if (Application.isEditor && !Application.isPlaying){
			if (fallTransform != null){
				var point = Curve.GetNearestPointTF(transform.position);
				var pos = Curve.Interpolate(point);
				var rot = Curve.GetTangent(point);
				fallTransform.position = new Vector3(pos.x, parentHole.transform.position.y + fallOffset.y, pos.z)
				                         + transform.right * parentHole.offset 
				                         + transform.forward * fallOffset.z;
				fallTransform.rotation = Quaternion.LookRotation(rot);
			}

			if (jumpOffTransform != null){
				var point = Curve.GetNearestPointTF(transform.position);
				var pos = Curve.Interpolate(point);
				var rot = Curve.GetTangent(point);
				jumpOffTransform.position = new Vector3(pos.x, parentHole.transform.position.y + jumpOffset.y, pos.z)
				                            + transform.right * parentHole.offset 
				                            + transform.forward * jumpOffset.z;
				jumpOffTransform.rotation = Quaternion.LookRotation(rot);
			}
		}
	}

	public override void OnPlayerApprochead(){
	}

	protected override void OnInteract(Player player){
		if (player.StateMachine.currentState == player.StateMachine.FlyingState)
			return;
		
		Debug.Log("[Player 0] fall into " + gameObject.name);
		GetComponent<BoxCollider>().enabled = false;
		var point = Curve.GetNearestPointTF(transform.position);
		var pos = Curve.Interpolate(point);
		var rot = Curve.GetTangent(point);
		fallTransform.position = new Vector3(pos.x, parentHole.transform.position.y + fallOffset.y, pos.z) 
		                         + transform.right * player.offset 
		                         + transform.forward * fallOffset.z;
		jumpOffTransform.position = new Vector3(pos.x, parentHole.transform.position.y + jumpOffset.y, pos.z)
		                            + transform.right * player.offset 
		                            + transform.forward * jumpOffset.z;
		fallTransform.rotation = Quaternion.LookRotation(Curve.GetTangentFast(Curve.GetNearestPointTF(fallTransform.position)));
		jumpOffTransform.rotation = Quaternion.LookRotation(Curve.GetTangentFast(Curve.GetNearestPointTF(jumpOffTransform.position)));
		
		player.currentFallen = this;
		player.StateMachine.ChangeState(player.StateMachine.FallenState);
	}
}