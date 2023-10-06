using System;
using FluffyUnderware.Curvy;
using UnityEngine;

public sealed class Player : MonoBehaviour{
	// PUBLIC MEMBERS
	[Header("Status")]
	public bool  IsGrounded;
	public bool  IsAirborne;
	public bool  IsKnocked;
	public bool  IsFallen;
	public bool  IsFlying;
	public bool  IsJumping;
	public float Speed;
	public float offset;
	public float height;
	public float travelledDst;
	
	[field: SerializeField]
	public GameObject Copter { get; set; }
	[field: SerializeField]
	public Rigidbody RB { get; set; }
	[field: SerializeField]
	public Animator Animator { get; set; }
	[field: SerializeField]
	public CurvySpline Curve { get; set; }
	[field: SerializeField]
	public BoxCollider GroundCollider { get; set; }
	[field: SerializeField]
	public CapsuleCollider BodyCollider { get; set; }
	[field: SerializeField]
	public StateMachine StateMachine { get; set; }
	[field: SerializeField]
	public Setting Setting { get; set; }

	// MonoBehavior Interface
	private void OnValidate(){
		RB ??= GetComponent<Rigidbody>();
		Animator ??= GetComponent<Animator>();
		BodyCollider ??= GetComponent<CapsuleCollider>();
		GroundCollider ??= GetComponent<BoxCollider>();
	}

	private void Awake(){
		RB = GetComponent<Rigidbody>();
		Animator = GetComponent<Animator>();
		BodyCollider = GetComponent<CapsuleCollider>();
		GroundCollider = GetComponent<BoxCollider>();
		StateMachine = new StateMachine(this, Setting);
	}

	private void Start(){
		StateMachine.OnStart();
	}

	private void FixedUpdate(){
		StateMachine.OnFixedUpdate();
	}

	private void Update(){
		StateMachine.OnUpdate();
	}

	private void OnTriggerEnter(Collider other){
		StateMachine.OnTriggerEnter(other);
	}

	private void OnTriggerExit(Collider other){
		StateMachine.OnTriggerExit(other);
	}
}