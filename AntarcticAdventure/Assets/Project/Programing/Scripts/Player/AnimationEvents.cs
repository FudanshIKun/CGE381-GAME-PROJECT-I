using Cinemachine;
using UnityEngine;

public sealed class AnimationEvents : MonoBehaviour{
	public void OnWin() => LevelHandler.Instance.StageCleared();
}