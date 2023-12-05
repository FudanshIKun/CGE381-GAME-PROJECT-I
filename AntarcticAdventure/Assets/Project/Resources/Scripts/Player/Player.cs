using FluffyUnderware.Curvy;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public sealed class Player : MonoBehaviour{
	// PUBLIC MEMBERS
	public bool    IsPaused;
	public bool    IsGrounded;
	public bool    IsAirborne;
	public bool    IsStumbling;
	public bool    IsFallen;
	public bool    IsFlying;
	public bool    IsJumping;
	public bool    IsFinishing;
	public bool    HasFinished;
	public float   Speed;
	public float   offset;
	public float   height;
	public float   travelledDst;
	public Fallen  currentFallen;
	
	[field: SerializeField]
	public GameObject Copter { get; set; }
	[field: SerializeField]
	public Rigidbody RB { get; set; }
	[field: SerializeField]
	public PlayerAnimation AnimationController { get; set; }
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

	// PUBLIC METHODS
	public void StartRunning(){
		StateMachine.ChangeState(StateMachine.RunningState);
	}
	
	// MonoBehavior Interface
	private void OnValidate(){
		RB ??= GetComponent<Rigidbody>();
		AnimationController ??= GetComponent<PlayerAnimation>();
		BodyCollider ??= GetComponent<CapsuleCollider>();
		GroundCollider ??= GetComponent<BoxCollider>();
	}

	private void Awake(){
		RB ??= GetComponent<Rigidbody>();
		AnimationController ??= GetComponent<PlayerAnimation>();
		BodyCollider ??= GetComponent<CapsuleCollider>();
		GroundCollider ??= GetComponent<BoxCollider>();
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