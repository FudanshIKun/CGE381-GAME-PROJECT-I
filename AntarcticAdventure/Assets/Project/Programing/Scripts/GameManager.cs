using UnityEngine;

public class GameManager : MonoBehaviour{
	private static GameManager instance;
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
    
    	private void Awake(){
    		if (instance != null && instance != this){
    			Destroy(gameObject);
    			return;
    		}
		    
    		instance = this;
    		DontDestroyOnLoad(gameObject);
    	}
	    
	    
}