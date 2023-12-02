using UnityEngine;

[ExecuteInEditMode]
public sealed class SunHandler : Handler<SunHandler>{
	[SerializeField]
	private GameObject sun;
	[SerializeField]
	private GameObject directionalLight;
	[Header("Setting")] 
	[SerializeField]
	private float sunOffset;

	private Vector3 _initialOffset;
	
	private void Start(){
		if (Application.isPlaying)
			if (sun != null && CameraHandler.Instance._gameplayCamera.VirtualCamera != null)
				RecordCameraOffset();
	}

	private void Update(){
		if (Application.isEditor && !Application.isPlaying){
			if (sun != null && directionalLight != null)
				SetUpSun();

			return;
		}
		
		if (Application.isPlaying)
			if (sun != null && CameraHandler.Instance._gameplayCamera.VirtualCamera != null)
				MoveSun();
	}

	private void SetUpSun(){
		sun.transform.position = directionalLight.transform.position;

		// Apply an offset along the opposite direction of the directional light's rotation
		var offset = -directionalLight.transform.forward * sunOffset;
		sun.transform.position += offset;
	}
    
	private void RecordCameraOffset()
		=> _initialOffset = sun.transform.position - CameraHandler.Instance._gameplayCamera.VirtualCamera.transform.position;

	private void MoveSun(){
		Debug.Log("[SunHandler] MoveSun");
		sun.transform.position = CameraHandler.Instance._gameplayCamera.VirtualCamera.transform.position + _initialOffset;
	}
}