﻿using Doozy.Runtime.UIManager.Components;
using UnityEngine;

public sealed class SoundHandler : Handler<SoundHandler>{
	// PRIVATE MEMBERS'
	[SerializeField]
	private UIToggle tg_music;
	[SerializeField]
	private UIToggle tg_sfx;
	[SerializeField]
	private AudioClip ui_selectedSFX;
	[SerializeField]
	private AudioClip ui_submitSFX;
	[SerializeField]
	private AudioClip ui_cancelSFX;
	[SerializeField]
	private AudioClip ui_intoGameSFX;
	[SerializeField]
	private AudioClip musicSFX;
	[SerializeField]
	private AudioClip ambientSFX;
	[SerializeField]
	private AudioClip countSFX;
	[SerializeField]
	private AudioClip startRunningSFX;
	[SerializeField]
	private AudioClip fishJumpSFX;
	[SerializeField]
	private AudioClip jumpSFX;
	[SerializeField]
	private AudioClip collisionSFX;
	[SerializeField]
	private AudioClip copterSFX;
	[SerializeField]
	private AudioClip scoreSFX;
	[SerializeField]
	private AudioClip winningSFX;
	[SerializeField]
	private AudioClip gameOverSFX;
	[SerializeField]
	private AudioClip enterMapSFX;
	
	// MonoBehavior INTERFACE
	private void Start(){
		Initialize();
	}

	// PUBLIC METHODS
	public void PlayEnterMap()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, enterMapSFX);
	
	public void PlayUISelect()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, ui_selectedSFX);
	
	public void PlayUISubmit()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, ui_submitSFX);

	public void PlayUICancel()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, ui_cancelSFX);

	public void PlayUIIntoGame(){
		SoundManager.Instance.StopSound(SoundManager.SoundType.Music);
		SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, ui_intoGameSFX);
	}
	
	public void StartMusic()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.Music, musicSFX);
	

	public void StopMusic()
		=> SoundManager.Instance.StopSound(SoundManager.SoundType.Music);
	

	public void StartAmbient()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.OtherAmbient, ambientSFX);
	

	public void StopAmbient()
		=> SoundManager.Instance.StopSound(SoundManager.SoundType.OtherAmbient);
	

	public void PlayCount()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, countSFX);
	

	public void PlayStartRunning()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, startRunningSFX);
	

	public void PlayFishJump()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, fishJumpSFX);
	

	public void PlayJump()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, jumpSFX);
	

	public void PlayCollision()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, collisionSFX);
	

	public void PlayScore()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, scoreSFX);
	

	public void StartCopter()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.PlayerAmbient, copterSFX);
	

	public void StopCopter() 
		=> SoundManager.Instance.StopSound(SoundManager.SoundType.PlayerAmbient);
	
	
	public void PlayWinning()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, winningSFX);
	

	public void PlayGameOver()
		=> SoundManager.Instance.PlaySound(SoundManager.SoundType.SFX, gameOverSFX);
	
	
	// PRIVATE METHODS
	private void Initialize(){
		if (tg_music != null){
			tg_music.OnToggleOnCallback.Event.AddListener(() => SoundManager.Instance.musicSource.mute = false);
			tg_music.OnToggleOnCallback.Event.AddListener(() => SoundManager.Instance.ToggleMusicVolume(true));
			tg_music.OnToggleOffCallback.Event.AddListener(() => SoundManager.Instance.musicSource.mute = true);
			tg_music.OnToggleOffCallback.Event.AddListener(() => SoundManager.Instance.ToggleMusicVolume(false));
		}
		
		if (tg_sfx != null){
			tg_sfx.OnToggleOnCallback.Event.AddListener(() => SoundManager.Instance.sfxSource.mute = false);
			tg_sfx.OnToggleOnCallback.Event.AddListener(() => SoundManager.Instance.ToggleSfxVolume(true));
			tg_sfx.OnToggleOffCallback.Event.AddListener(() => SoundManager.Instance.sfxSource.mute = true);
			tg_sfx.OnToggleOffCallback.Event.AddListener(() => SoundManager.Instance.ToggleSfxVolume(false));
		}
	}
}