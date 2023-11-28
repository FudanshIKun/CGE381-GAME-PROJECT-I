using System;
using UnityEngine;

[ExecuteInEditMode]
public class CameraHandler : Handler<CameraHandler>{
	// PRIVATE MEMBERS
	[SerializeField]
	private Camera mainCamera;
	[SerializeField] 
	private GameplayCamera _gameplayCamera;

	// MonoBehavior INTERFACE
	private void OnValidate(){
		mainCamera ??= Camera.main;
	}

	private void Update(){
		if (Application.isEditor && !Application.isPlaying){
			if (_gameplayCamera != null){
				if (_gameplayCamera.setting.target != null){
					if (_gameplayCamera.setting.target.TryGetComponent(out Player player)){
						_gameplayCamera.player ??= player;
					
						var position = player.Curve.InterpolateByDistance(player.travelledDst);
						var offset = _gameplayCamera.setting.offset;
						if (_gameplayCamera.VirtualCamera != null){
							_gameplayCamera.VirtualCamera.transform.position = new Vector3(position.x + offset.x, position.y + offset.y, position.z + offset.z);
						}
					}
					else{
						Debug.Log("Error! GameplayCamera's target must have Player as component.");
					}
				}
			}
		}
	}

	private void LateUpdate(){
		if (Application.isPlaying){
			if (_gameplayCamera != null){
				_gameplayCamera.Move();
				_gameplayCamera.Rotate();
			}
		}
	}
	
	// PUBLIC METHODS
	public void EnterCinematic() => _gameplayCamera.IsInCinematic = true;
	public void ExitCinematic()  => _gameplayCamera.IsInCinematic = false;
}