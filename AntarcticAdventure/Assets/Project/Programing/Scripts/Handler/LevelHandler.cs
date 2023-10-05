using System;
using FluffyUnderware.Curvy;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LevelHandler : Handler<LevelHandler>{
	// Public Members
	public Player      Player;
	public CurvySpline Curve;
	public GameManager.Scenes Level;
	public int score;

	// PRIVATE MEMBERS
	
	
	// MonoBehavior Interface
	private void OnEnable(){
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	private void Update(){
		CheckWinConditions();
	}

	private void OnDisable(){
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	public void OnSceneUnloaded(Scene scene){
		ScoreManager.Instance.RecordScore(Level, score);
	}
	
	// PRIVATE MEMBERS
	private void CheckWinConditions(){
		
	}
}