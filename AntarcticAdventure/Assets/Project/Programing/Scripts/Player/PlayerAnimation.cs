using System;
using System.Collections;
using UnityEngine;

public sealed class PlayerAnimation : MonoBehaviour{
	// PUBLIC MEMBERS
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

	private static readonly int IsGround       = Animator.StringToHash("is_ground");
	private static readonly int RunBlend       = Animator.StringToHash("run_blend");
	private static readonly int IsJump         = Animator.StringToHash("is_jump");
	private static readonly int IsJumpRight    = Animator.StringToHash("is_jump_right");
	private static readonly int IsCopter       = Animator.StringToHash("is_copter");
	private static readonly int IsWin          = Animator.StringToHash("is_win");
	private static readonly int IsStumbleRight = Animator.StringToHash("is_stumble_right");
	private static readonly int IsStumble      = Animator.StringToHash("is_stumble");
	private static readonly int IsInHole       = Animator.StringToHash("is_in_hole");

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
	public void StartRunning(){
		if (playAnimation)
			Animator.SetBool(IsGround, true);
	}
	
	public void RunningBlending(float target){
		if (playAnimation)
			Animator.SetFloat(RunBlend, Mathf.Lerp(Animator.GetFloat(RunBlend), target, Time.deltaTime * blendingSpeed));
	}

	public void StopRunning(){
		if (playAnimation)
			Animator.SetBool(IsGround, false);
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
	
	public void StartFlying(){
		if (playAnimation)
			Animator.SetBool(IsCopter, true);
	}

	public void StopFlying(){
		if (playAnimation)
			Animator.SetBool(IsCopter, false);
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

	public void StartFinishing(){
		if (playAnimation)
			Animator.SetBool(IsWin, true);
	}

	public void StartHanging(){
		if (playAnimation)
			Animator.SetBool(IsInHole, true);
	}

	public void StopHanging(){
		if(playAnimation)
			Animator.SetBool(IsInHole, false);
	}

	public void StartJumpOff(){
		if (playAnimation)
			Animator.SetBool(IsJump, true);
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