using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ScoreManager : Manager{
	public Dictionary<string, int> HighestScores = new ();
	private const string ScoreKeyPrefix = "HighestScore_";
	private static readonly string[] ScenesToExclude = { "MainMenu", "Map" };

	// MonoBehavior Interface
	private void Start(){
		LoadScore();
	}

	private void OnApplicationQuit(){
		SaveScore();
	}

	// Public Methods
	public void RecordScore(GameManager.Scenes scene, int score){
		Debug.Log("[FUNCTION] RecordScore");
		var sceneKey = scene.ToString();
		if (!HighestScores.ContainsKey(sceneKey) || score > HighestScores[sceneKey]) {
			HighestScores[sceneKey] = score;
		}
		
	}

	// Private Methods
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