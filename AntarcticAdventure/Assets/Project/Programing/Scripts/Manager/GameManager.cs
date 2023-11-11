using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour{
	public static GameManager Instance {
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
		Initialize();
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
	public enum Scenes{
		Prototype,
		MainMenu,
		Map,
		Level1  = 0,
		Level2  = 1,
		Level3  = 2,
		Level4  = 3,
		Level5  = 4,
		Level6  = 5,
		Level7  = 6,
		Level8  = 7,
		Level9  = 8,
		Level10 = 9
	}
}