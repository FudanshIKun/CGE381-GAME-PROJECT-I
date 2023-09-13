using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour{
	// Public Member
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
	
	// GameManager Interface
#region Manager Management
	private static GameManager instance;
	private Dictionary<string, MonoBehaviour> managers;
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
	
	public void Register(string name, MonoBehaviour manager){
		if (!managers.ContainsKey(name)){
			managers.Add(name, manager);
			Debug.Log($"Register {manager} to GameManager");
		}
		else{
			Debug.LogWarning($"Manager with name '{name}' already exists in the GameManager.");
		}
	}

	public T Get<T>() where T : MonoBehaviour{
		var typeName = typeof(T).Name;
		if (managers.TryGetValue(typeName, out var M)){
			return (T)M;
		}

		var managerGO = new GameObject(typeName + "Manager");
		var manager = managerGO.AddComponent<T>();
		manager.transform.SetParent(transform);
		managers.Add(typeName, manager);
		return manager;
	}
	
#endregion
	
	// MonoBehavior Interface
#region MonoBehavior
	private void Awake(){
		if (instance != null && instance != this){
			Destroy(gameObject);
			return;
		}

		managers = new Dictionary<string, MonoBehaviour>();
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	
#endregion
}