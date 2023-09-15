using System;
using UnityEngine.SceneManagement;

public sealed class LevelHandler : Handler<LevelHandler>{
	// Public Members
	public GameManager.Scenes Level;
	public int score;

	// MonoBehavior Interface
#region MonoBehavior
	private void OnEnable(){
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	private void OnDisable(){
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}
	
#endregion

	public void OnSceneUnloaded(Scene scene){
		GameManager.Instance.Get<ScoreManager>().RecordScore(Level, score);
	}
}