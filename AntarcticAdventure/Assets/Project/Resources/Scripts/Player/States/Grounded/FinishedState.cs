using DG.Tweening;
using UnityEngine;

public sealed class FinishedState : GroundedState{
	public FinishedState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// PRIVATE MEMBERS
	private bool finishedX;
	private bool finishedZ;
		
	// State INTERFACE
	public override void OnEnter(){
		base.OnEnter();
		OnFinishedEnter(stateMachine.player);
		animation.SetRunning(true);
		animation.SetRunningBlending(0);
		SoundHandler.Instance.StopMusic();
		ParticleHandler.Instance.StopSpawning();
	}

	public override void OnUpdate(){
		base.OnUpdate();
		if (finishedX && finishedZ){
			if (stateMachine.player.HasFinished)
				return;
			
			PostFinished(stateMachine.player);
		}
	}

	protected override void OnGroundContact(){
		base.OnGroundContact();
		var position = stateMachine.player.transform.position;
		stateMachine.player.transform.position = new Vector3(position.x, 0, position.z);
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		animation.StopJumping();
		animation.SetRunning(true);
		animation.PlayImpactAnimation();
	}

	// PRIVATE METHODS
	private void OnFinishedEnter(Player player){
		player.IsFinishing = true;
		player.transform.DOMoveX(LevelHandler.Instance.destination.transform.position.x, setting.toFinishingDuration)
			.OnComplete((() => {
				finishedX = true;
			}));
		
		player.transform.DOMoveZ(LevelHandler.Instance.destination.transform.position.z, setting.toFinishingDuration)
			.OnComplete((() => {
				finishedZ = true;
			}));
	}

	private void PostFinished(Player player){
		Debug.Log("PostFinished");
		player.HasFinished = true;
		animation.SetFinishing(true);
	}
}