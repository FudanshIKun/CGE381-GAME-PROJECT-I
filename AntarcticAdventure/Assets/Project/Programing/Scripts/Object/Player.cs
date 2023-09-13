using DG.Tweening;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Controllers;
using UnityEngine;

public sealed class Player : MonoBehaviour{
	// Private Members
	[field: SerializeField] private CurvySpline curve { get; set; }
	[field: SerializeField] private SplineController controller { get; set; }
	[field: SerializeField] private Animator animator { get; set; }
	[field: SerializeField] private BoxCollider boxCollider { get; set; }
	[Header("Setting")]
	[SerializeField] private float baseSpeed;
	[SerializeField] private float acceleration;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float maxOffset;
	[SerializeField] private float jumpHeight;
	[SerializeField] private float jumpPower;
	private float speedAcceleration;
	private float offsetAcceleration;
	private bool IsKnockingBack;
	private bool IsFalling;
	private bool IsStumbling;

	// MonoBehavior Interface
#region MonoBehavior
	private void OnValidate(){
		controller ??= GetComponent<SplineController>();
		animator ??= GetComponent<Animator>();
		boxCollider ??= GetComponent<BoxCollider>();
	}

	private void Awake(){
		controller = GetComponent<SplineController>();
		animator = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider>();
	}

	private void Update(){
		Move();
		Jump();
	}
	
#endregion
    // Public Methods
    public void KnockBack(){
    		IsKnockingBack = true;
    	}
    
    	public void FallIntoHole(){
    		IsFalling = true;
    	}
    
    	public void Stumble(){
    		IsStumbling = true;
    	}
   

	// Private Methods
	private void Move(){
		if (IsKnockingBack || IsFalling || IsStumbling)
			return;

		if (controller.Speed < baseSpeed){
			controller.Speed += Time.deltaTime * baseSpeed;
		}
		else{
			if (Input.GetKey(KeyCode.W)){
				Debug.Log("Moving forward");
				speedAcceleration = maxSpeed - controller.Speed;;
			}
			else{
				speedAcceleration = baseSpeed - controller.Speed;;
			}
			
			controller.Speed += Time.deltaTime * speedAcceleration;
			controller.Speed = Mathf.Clamp(controller.Speed, baseSpeed, maxSpeed);
		}

		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
			offsetAcceleration *= 0.5f;
			if (Mathf.Abs(offsetAcceleration) < 0.01f) 
				offsetAcceleration = 0f;
		}
		
		if (Input.GetKey(KeyCode.A)){
			Debug.Log("Moving left");
			offsetAcceleration -= Time.deltaTime * acceleration; 
		}
		if (Input.GetKey(KeyCode.D)){
			Debug.Log("Moving right");
			offsetAcceleration += Time.deltaTime * acceleration;
		}
		
		controller.OffsetRadius += offsetAcceleration * Time.deltaTime;
		controller.OffsetRadius = Mathf.Clamp(controller.OffsetRadius, -maxOffset, maxOffset);
	}

	private void Jump(){
		if (Input.GetKeyDown(KeyCode.Space)){
			if (ThereIsGroundUnderneath()){
				Debug.Log("Jump");
				var pos = curve.transform.position;
				curve.transform.DOJump(pos, jumpPower, 1, 1, false);
			}
		}
	}
	
	private bool ThereIsGroundUnderneath(){
		var bound = boxCollider.bounds;
		var centerInWorldSpace = bound.center;
		var extents = bound.extents;
		var rotation = boxCollider.transform.rotation;
		var groundLayer = LayerMask.GetMask("Ground"); // Replace "Ground" with your actual ground layer name.

		var overlappedColliders = new Collider[10];
		var overlappedCount = Physics.OverlapBoxNonAlloc(centerInWorldSpace, extents, overlappedColliders, rotation,
			groundLayer, QueryTriggerInteraction.Ignore);

		return overlappedCount > 0;
	}
}