using Cinemachine;
using UnityEngine;

public sealed class AnimationEvents : MonoBehaviour{
	// PUBLIC METHODS
	public void OnWin() => LevelHandler.Instance.StageCleared();
}