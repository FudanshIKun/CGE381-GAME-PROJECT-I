using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerPrefManager : MonoBehaviour{
	// PUBLIC MEMBERS
	public static PlayerPrefManager Instance
	{ get{
		if (instance == null){
			instance = FindObjectOfType<PlayerPrefManager>();
			if (instance == null){
				var gameManagerGO = new GameObject("GameManager");
				instance = gameManagerGO.AddComponent<PlayerPrefManager>();
			}
		}

		return instance;
	} }

	public Dictionary<string, int> HighestScores = new();
	
	private static          PlayerPrefManager instance;

	public static readonly string ScoreKeyPrefix  = "HighestScore_";
	public static readonly string LastStagePrefix = "LastLevel";
	public static readonly string SettingPrefix   = "_Setting";

	// MonoBehavior INTERFACE
	private void Awake(){
		if (instance != null && instance != this){
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void Start(){
		LoadScorePref();
		LoadSettingPref();
	}

	// PUBLIC METHODS
	public int GetLastStageNumber() => PlayerPrefs.GetInt(LastStagePrefix);
	
	public void CreateRecord(){
		// Define a mapping of enum values to PlayerPrefs keys
		var keyMapping = new Dictionary<GameManager.Scenes, string>{
			{ GameManager.Scenes.Level1, "Level1" },
			{ GameManager.Scenes.Level2, "Level2" },
			{ GameManager.Scenes.Level3, "Level3" },
			{ GameManager.Scenes.Level4, "Level4" },
			{ GameManager.Scenes.Level5, "Level5" },
			{ GameManager.Scenes.Level6, "Level6" },
			{ GameManager.Scenes.Level7, "Level7" },
			{ GameManager.Scenes.Level8, "Level8" },
			{ GameManager.Scenes.Level9, "Level9" },
			{ GameManager.Scenes.Level10, "Level10" },
		};

		// Load scores using the key mapping
		foreach (var scene in keyMapping.Keys){
			var sceneKey = keyMapping[scene];
			var savedScore = PlayerPrefs.GetInt(ScoreKeyPrefix + sceneKey, 0);
			HighestScores[sceneKey] = savedScore;
			Debug.Log($"[PlayerPrefManager] Score: {savedScore} has been loaded to {sceneKey}");
		}
		
		foreach (var pair in HighestScores) {
			PlayerPrefs.SetInt(ScoreKeyPrefix + pair.Key, pair.Value);
		}
		
		PlayerPrefs.SetInt(LastStagePrefix, 0);
	}

	public void RecordScore(GameManager.Scenes scene, int score){
		Debug.Log("[PlayerPrefManager] RecordScore");
		var sceneKey = scene.ToString();
		if (HighestScores.ContainsKey(sceneKey) && score > HighestScores[sceneKey]){
			Debug.Log("[FUNCTION] RecordScore: ContainKey & greater than old score");
			HighestScores[sceneKey] = score;
			PlayerPrefs.SetInt(ScoreKeyPrefix + sceneKey, score);
		}
	}

	public void RecordLastStage(int value){
		Debug.Log("[PlayerPrefManager] RecordLastStage");
		if (value > 10)
			return;
		
		PlayerPrefs.SetInt(LastStagePrefix, value);
	}

	public void RecordSetting(bool music_enabled, bool sfx_enabled){
		PlayerPrefs.SetInt("music" + SettingPrefix, music_enabled ? 1 : 0);
		PlayerPrefs.SetInt("sfx" + SettingPrefix,   sfx_enabled ? 1 : 0);
	}

	public void LoadScorePref(){
		Debug.Log("[PlayerPrefManager] LoadScore");
		// Define a mapping of enum values to PlayerPrefs keys
		var keyMapping = new Dictionary<GameManager.Scenes, string>{
			{ GameManager.Scenes.Level1, "Level1" },
			{ GameManager.Scenes.Level2, "Level2" },
			{ GameManager.Scenes.Level3, "Level3" },
			{ GameManager.Scenes.Level4, "Level4" },
			{ GameManager.Scenes.Level5, "Level5" },
			{ GameManager.Scenes.Level6, "Level6" },
			{ GameManager.Scenes.Level7, "Level7" },
			{ GameManager.Scenes.Level8, "Level8" },
			{ GameManager.Scenes.Level9, "Level9" },
			{ GameManager.Scenes.Level10, "Level10" },
		};

		// Load scores using the key mapping
		foreach (var scene in keyMapping.Keys){
			var sceneKey = keyMapping[scene];
			var savedScore = PlayerPrefs.GetInt(ScoreKeyPrefix + sceneKey, 0);
			HighestScores[sceneKey] = savedScore;
			Debug.Log($"[PlayerPrefManager] Score: {savedScore} has been loaded to {sceneKey}");
		}
	}

	public void LoadSettingPref(){
		var music_enabled = PlayerPrefs.GetInt("music" + SettingPrefix, 1);
		var sfx_enabled = PlayerPrefs.GetInt("sfx" + SettingPrefix,   1);
		SoundManager.Instance.musicSource.mute = music_enabled == 0;
		SoundManager.Instance.sfxSource.mute = sfx_enabled == 0;
		SoundManager.Instance.playerAmbientSource.mute = sfx_enabled == 0;
		SoundManager.Instance.OtherAmbientSource.mute = sfx_enabled == 0;
		SoundManager.Instance.ToggleMusicVolume(music_enabled == 1);
		SoundManager.Instance.ToggleSfxVolume(sfx_enabled == 1);
	}

	public void Save(){
		Debug.Log("[PlayerPrefManager] Save");
		PlayerPrefs.Save();
	}
}