using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour{
	// PUBLIC MEMBERS
	public static GameManager Instance{
		get{
			if (instance == null){
				instance = FindObjectOfType<GameManager>();
				if (instance == null){
					var gameManagerGO = new GameObject("GameManager");
					instance = gameManagerGO.AddComponent<GameManager>();
				}
			}
    			
			return instance;
		}
	}
	
	// PRIVATE MEMBERS
	private static GameManager instance;
	
	
	// MonoBehavior Interface
	private void Awake(){
		if (instance != null && instance != this){
			Destroy(gameObject);
			return;
		}
		
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	
	// DATA STRUCTURE
	public enum Scenes{
		Prototype,
		MainMenu,
		Map,
		Level1,
		Level2,
		Level3,
		Level4,
		Level5,
		Level6,
		Level7,
		Level8,
		Level9,
		Level10
	}
}