using System;
using UnityEngine;

[ExecuteInEditMode]
public class CameraHandler : Handler<CameraHandler>{
	public GameplayCamera GameplayCamera { 
		get{
			if (_gameplayCamera == null){
				_gameplayCamera = new GameplayCamera(gameplayCamera);
			}

			return _gameplayCamera;
		}
	}
	
	// PRIVATE MEMBERS
	[SerializeField]
	private Camera gameplayCamera;
	[SerializeField] 
	private GameplayCamera _gameplayCamera;

	// MonoBehavior INTERFACE
	private void Update(){
		if (Application.isEditor && !Application.isPlaying){
			if (_gameplayCamera != null && gameplayCamera != null){
				_gameplayCamera.camera ??= gameplayCamera;
				
				if (_gameplayCamera.setting.target != null){
					if (_gameplayCamera.setting.target.TryGetComponent(out Player player)){
						_gameplayCamera.player ??= player;
					
						var position = player.Curve.InterpolateByDistance(player.travelledDst);
						var offset = _gameplayCamera.setting.offset;
						gameplayCamera.transform.position = new Vector3(position.x + offset.x, position.y + offset.y, position.z + offset.z);
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
			GameplayCamera.Move();
			GameplayCamera.Rotate();
		}
	}
}