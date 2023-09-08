using UnityEngine;

public abstract class Manager : MonoBehaviour{
	// MonoBehavior Interface
	protected virtual void Awake(){
		GameManager.Instance.Register(name, this);
	}
}