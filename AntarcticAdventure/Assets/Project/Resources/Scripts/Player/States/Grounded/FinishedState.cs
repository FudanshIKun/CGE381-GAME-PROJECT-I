using DG.Tweening;
using UnityEngine;

public sealed class FinishedState : GroundedState{
	public FinishedState(StateMachine stateMachine, Setting setting) : base(stateMachine, setting){
	}
	
	// PRIVATE MEMBERS
	private bool finished;
		
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
		if (finished){
			if (stateMachine.player.HasFinished)
				return;
			
			PostFinished(stateMachine.player);
		}
	}

	protected override void OnGroundContact(){
		base.OnGroundContact();
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		animation.StopJumping();
		animation.SetRunning(true);
		animation.PlayImpactAnimation();
	}

	// PRIVATE METHODS
	private void OnFinishedEnter(Player player){
		player.IsFinishing = true;
		player.transform.DOMove(LevelHandler.Instance.destination.transform.position, setting.toFinishingDuration)
			.OnComplete((() => {
				finished = true;
			}));
	}

	private void PostFinished(Player player){
		Debug.Log("[Player 0] PostFinished");
		player.HasFinished = true;
		animation.SetFinishing(true);
	}
}