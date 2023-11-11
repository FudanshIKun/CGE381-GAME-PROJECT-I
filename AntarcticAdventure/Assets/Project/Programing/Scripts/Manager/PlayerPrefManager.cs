using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class PlayerPrefManager : MonoBehaviour{
	// PUBLIC MEMBERS
	public static PlayerPrefManager Instance {
		get{
			if (instance == null){
				instance = FindObjectOfType<PlayerPrefManager>();
				if (instance == null){
					var gameManagerGO = new GameObject("GameManager");
					instance = gameManagerGO.AddComponent<PlayerPrefManager>();
				}
			}
    			
			return instance;
		}
	}
	
	public Dictionary<string, int> HighestScores = new ();
	
	// PRIVATE MEMBERS
	private static readonly string[]          ScenesToExclude = { "MainMenu", "Map" };
	private static          PlayerPrefManager instance;
	
	private const string ScoreKeyPrefix     = "HighestScore_";
	private const string LastLevelPrefix = "LastLevel_";

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
		LoadScore();
	}

	private void OnApplicationQuit(){
		SaveScore();
	}

	// PUBLIC METHODS
	public void RecordScore(GameManager.Scenes scene, int score){
		Debug.Log("[FUNCTION] RecordScore");
		var sceneKey = scene.ToString();
		if (HighestScores.ContainsKey(sceneKey) || score > HighestScores[sceneKey]) {
			HighestScores[sceneKey] = score;
		}
	}

	public void RecordLastLevel(GameManager.Scenes scene){
		var sceneName = scene.ToString();
		if (ScenesToExclude.Contains(sceneName))
			return;
		
		PlayerPrefs.SetString(LastLevelPrefix , sceneName);
		PlayerPrefs.Save();
	}

	public void DeleteRecord(){
		
	}

	// PRIVATE METHODS
	private void LoadScore(){
		Debug.Log("[FUNCTION] LoadScore");
		foreach (GameManager.Scenes scene in Enum.GetValues(typeof(GameManager.Scenes))){
			var sceneKey = scene.ToString();
			if (ScenesToExclude.Contains(sceneKey)){
				continue;
			}

			var savedScore = PlayerPrefs.GetInt(ScoreKeyPrefix + sceneKey, 0);
			HighestScores[sceneKey] = savedScore;
		}
	}

	private void SaveScore(){
		Debug.Log("[FUNCTION] SaveScore");
		foreach (var pair in HighestScores) {
			PlayerPrefs.SetInt(ScoreKeyPrefix + pair.Key, pair.Value);
		}
		PlayerPrefs.Save();
	}
}