using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ScoreManager : MonoBehaviour{
	// PUBLIC MEMBERS
	public static ScoreManager Instance {
		get{
			if (instance == null){
				instance = FindObjectOfType<ScoreManager>();
				if (instance == null){
					var gameManagerGO = new GameObject("GameManager");
					instance = gameManagerGO.AddComponent<ScoreManager>();
				}
			}
    			
			return instance;
		}
	}
	
	public Dictionary<string, int> HighestScores = new ();
	
	// PRIVATE MEMBERS
	private static readonly string[]     ScenesToExclude = { "MainMenu", "Map" };
	private static          ScoreManager instance;
	
	private const string ScoreKeyPrefix = "HighestScore_";

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

	// PRIVATE METHODS
	private void LoadScore(){
		Debug.Log("[FUNCTION] LoadScore");
		foreach (GameManager.Scenes scene in System.Enum.GetValues(typeof(GameManager.Scenes))){
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