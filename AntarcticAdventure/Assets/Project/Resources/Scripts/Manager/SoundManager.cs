using System;
using UnityEngine;

public sealed class SoundManager : MonoBehaviour{
	// PUBLIC MEMBERS
	public static SoundManager Instance {
	get{
			if (instance == null){
				instance = FindObjectOfType<SoundManager>();
				if (instance == null){
					var gameManagerGO = new GameObject("GameManager");
					instance = gameManagerGO.AddComponent<SoundManager>();
				}
			}
    			
			return instance;
		}
	}

	public bool music_enabled = true;
	public bool sfx_enabled = true;
	public AudioSource musicSource;
	public AudioSource OtherAmbientSource;
	public AudioSource playerAmbientSource;
	public AudioSource sfxSource;
	
	// PRIVATE MEMBERS
	private static SoundManager instance;
	
	// MonoBehavior INTERFACE
	private void Awake(){
		Initialize();
	}

	private void Start() {
		musicSource.loop = true;
		OtherAmbientSource.loop = true;
	}

	private void OnApplicationQuit(){
		PlayerPrefManager.Instance.RecordSetting(music_enabled, sfx_enabled);
		PlayerPrefManager.Instance.Save();
	}

	// PUBLIC METHODS
	public void ToggleSfxVolume(bool value){
		sfx_enabled = value;
	}

	public void ToggleMusicVolume(bool value){
		music_enabled = value;
	}
	
	public void PlaySound(SoundType soundType, AudioClip clip){
		switch (soundType){
			case SoundType.Music:
				Debug.Log("[SoundManager] PlaySound (Music)");
				musicSource.clip = clip;
				musicSource.Play();
				break;
			case SoundType.PlayerAmbient:
				Debug.Log("[SoundManager] PlaySound (PlayerAmbient)");
				playerAmbientSource.clip = clip;
				playerAmbientSource.Play();
				break;
			case SoundType.OtherAmbient:
				Debug.Log("[SoundManager] PlaySound (OtherAmbient)");
				OtherAmbientSource.clip = clip;
				OtherAmbientSource.Play();
				break;
			case SoundType.SFX:
				Debug.Log("[SoundManager] PlaySound (SFX)");
				sfxSource.PlayOneShot(clip);
				break;
		}
	}
	
	public void StopSound(SoundType soundType){
		switch (soundType){
			case SoundType.Music:
				Debug.Log("[SoundManager] StopSound (Music)");
				musicSource.Stop();
				break;
			case SoundType.PlayerAmbient:
				Debug.Log("[SoundManager] StopSound (PlayerAmbient)");
				playerAmbientSource.Stop();
				break;
			case SoundType.OtherAmbient:
				Debug.Log("[SoundManager] StopSound (OtherAmbient)");
				OtherAmbientSource.Stop();
				break;
			case SoundType.SFX:
				Debug.Log("[SoundManager] StopSound (SFX)");
				sfxSource.Stop();
				break;
		}
	}
	
	// PRIVATE METHODS
	private void Initialize(){
		if (instance != null && instance != this){
			Destroy(gameObject);
			return;
		}
		
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	
	// DATA STRUCTURE
	public enum SoundType{
		Music,
		PlayerAmbient,
		OtherAmbient,
		SFX
	}
}