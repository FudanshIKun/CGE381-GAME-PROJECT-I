using System;
using System.Collections;
using UnityEngine;

public sealed class PlayerAnimation : MonoBehaviour{
	[field: SerializeField] 
	public bool playAnimation { get; private set; } = true;
	[field: SerializeField]
	private Animator Animator { get; set; }
	[field: SerializeField]
	private Transform ScaleRoot { get; set; }

	[Header("Status")] 
	public bool jumpSide;
	public bool stumbleSide;
	[Header("Setting")] 
	[SerializeField] 
	public float blendingSpeed = 5f;
	[SerializeField] 
	private float xScaleWeight = 1f;
	[SerializeField]
	private AnimationCurve impactCurve;
	[SerializeField]
	private float impactInterval;
	[SerializeField]
	private float impactWeight;
	[SerializeField]
	private AnimationCurve jumpCurve;
	[SerializeField]
	private float jumpInterval;
	[SerializeField]
	private float jumpWeight;

	public static readonly int IsGround       = Animator.StringToHash("is_ground");
	public static readonly int IsRunning      = Animator.StringToHash("is_running");
	public static readonly int RunBlend       = Animator.StringToHash("run_blend");
	public static readonly int IsJump         = Animator.StringToHash("is_jump");
	public static readonly int IsJumpRight    = Animator.StringToHash("is_jump_right");
	public static readonly int IsCopter       = Animator.StringToHash("is_copter");
	public static readonly int IsWin          = Animator.StringToHash("is_win");
	public static readonly int IsStumbleRight = Animator.StringToHash("is_stumble_right");
	public static readonly int IsStumble      = Animator.StringToHash("is_stumble");
	public static readonly int IsInHole       = Animator.StringToHash("is_in_hole");

	// MonoBehavior INTERFACE
	private void OnValidate(){
		ScaleRoot ??= transform;
		Animator ??= GetComponentInChildren<Animator>();
	}

	private void Awake(){
		ScaleRoot ??= transform;
		Animator ??= GetComponentInChildren<Animator>();
		if (!playAnimation)
			Animator.enabled = false;
	}

	// PUBLIC METHODS
	public void SetGrounded(bool value){
		if (playAnimation)
			Animator.SetBool(IsGround, value);
	}
	
	public void SetRunning(bool value){
		if (playAnimation)
			Animator.SetBool(IsRunning, value);
	}
	
	public void SetRunningBlending(float target){
		if (playAnimation)
			Animator.SetFloat(RunBlend, Mathf.Lerp(Animator.GetFloat(RunBlend), target, Time.deltaTime * blendingSpeed));
	}
	
	public void StartJumping(){
		if (playAnimation){
			Animator.SetBool(IsJump, true);
			StartCoroutine(PlayAnimationCurve(jumpCurve, jumpInterval, jumpWeight));
			jumpSide = !jumpSide;
			Animator.SetBool(IsJumpRight, jumpSide);
		}
	}

	public void StopJumping(){
		if (playAnimation)
			Animator.SetBool(IsJump, false);
	}
	
	public void SetFlying(bool value){
		if (playAnimation)
			Animator.SetBool(IsCopter, value);
	}

	public void StartStumble(bool rightSide){
		if (playAnimation){
			Animator.SetBool(IsStumbleRight, rightSide);
			Animator.SetBool(IsStumble,      true);
		}
	}

	public void StopStumble(){
		if (playAnimation)
			Animator.SetBool(IsStumble, false);
	}

	public void SetFinishing(bool value){
		if (playAnimation)
			Animator.SetBool(IsWin, value);
	}

	public void SetHanging(bool value){
		if (playAnimation)
			Animator.SetBool(IsInHole, value);
	}

	public void SetJumpOff(bool value){
		if (playAnimation)
			Animator.SetBool(IsJump, value);
	}

	public void PlayImpactAnimation(){
		if (playAnimation)
			StartCoroutine(PlayAnimationCurve(impactCurve, impactInterval, impactWeight));
	}
	
	// PRIVATE METHODS
	private void SetScale(float yScale){
		var xScale = Utility.LerpWithoutClamp(1, (1 / yScale), xScaleWeight);
		ScaleRoot.transform.localScale = new Vector3(xScale, yScale, 1);
	}
	
	private IEnumerator PlayAnimationCurve(AnimationCurve curve, float interval, float weight){
		float elapseTime = 0;
		while(elapseTime < interval){
			elapseTime += Time.deltaTime;
			var value = curve.Evaluate(elapseTime / interval);
			var weightedValue = Utility.LerpWithoutClamp(1, value, weight);
			SetScale(weightedValue);
			yield return null;
		}
		
		ScaleRoot.transform.localScale = new Vector3(1, 1, 1);
	}
	
}