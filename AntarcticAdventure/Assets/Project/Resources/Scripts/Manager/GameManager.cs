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
	
	public static class RunMode {
		public enum ApplicationRunMode {
			Device,
			Editor,
			Simulator
		}
		
		public static ApplicationRunMode Current {
			get {
			#if UNITY_EDITOR
				return UnityEngine.Device.Application.isEditor && !UnityEngine.Device.Application.isMobilePlatform ? ApplicationRunMode.Editor : ApplicationRunMode.Simulator;
				
			#else
				return ApplicationRunMode.Device;
				
			#endif
			}
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
		Level1  = 0,
		Level2  = 1,
		Level3  = 2,
		Level4  = 3,
		Level5  = 4,
		Level6  = 5,
		Level7  = 6,
		Level8  = 7,
		Level9  = 8,
		Level10 = 9,
		MainMenu,
		Map,
		Ending
	}
}