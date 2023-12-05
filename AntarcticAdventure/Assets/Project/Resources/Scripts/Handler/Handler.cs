using UnityEngine;

public abstract class Handler<H> : MonoBehaviour where H : Handler<H>{
	protected static bool AutoCreateInstance;
	
	// Handler Interface
	private static H instance;
	public static H Instance{
		get{
			if (instance != null)
				return instance;
            
			instance = FindObjectOfType<H>();
			if (instance == null && AutoCreateInstance){
				var handler = new GameObject(typeof(H).Name);
				instance = handler.AddComponent<H>();
			}
            
			return instance;
		}
	}

	// MonoBehavior Interface
	protected virtual void Awake(){
		if (instance == null){
			instance = this as H;
		}
		else
			Destroy(gameObject);
	}
}