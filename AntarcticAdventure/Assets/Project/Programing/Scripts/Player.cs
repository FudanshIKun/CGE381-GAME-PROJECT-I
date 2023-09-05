using System;
using FluffyUnderware.Curvy.Controllers;
using UnityEngine;

public class Player : MonoBehaviour{
	private bool IsKnockingBack;
	private bool IsFalling;
	private bool IsStumbling;
	[SerializeField] private float baseSpeed;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float maxOffset;
	private SplineController controller { get; set; }

	private void Awake(){
		controller = GetComponent<SplineController>();
	}

	private void Update(){
		Move();
	}

	private void Move(){
		if (IsKnockingBack || IsFalling || IsStumbling)
			return;
		
		if (Input.GetKey(KeyCode.W)){
			controller.Speed += Time.deltaTime * baseSpeed;
			controller.Speed = Mathf.Clamp(controller.Speed, baseSpeed, maxSpeed);
		}
		else{
			controller.Speed -= Time.deltaTime * baseSpeed;
			controller.Speed = Mathf.Clamp(controller.Speed, baseSpeed, maxSpeed);
		}

		var offsetChange = 0f;
		if (Input.GetKey(KeyCode.A)){
			offsetChange -= Time.deltaTime * maxOffset; 
		}
		if (Input.GetKey(KeyCode.D)){
			offsetChange += Time.deltaTime * maxOffset;
		}

		controller.OffsetRadius += offsetChange;
		controller.OffsetRadius = Mathf.Clamp(controller.OffsetRadius, -maxOffset, maxOffset);
	}

	public void KnockBack(){
		IsKnockingBack = true;
	}

	public void FallIntoHole(){
		IsFalling = true;
	}

	public void Stumble(){
		IsStumbling = true;
	}
}