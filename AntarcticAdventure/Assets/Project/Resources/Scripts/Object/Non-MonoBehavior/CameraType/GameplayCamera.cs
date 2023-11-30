using System;
using Cinemachine;
using UnityEngine;

[Serializable]
public sealed class GameplayCamera{
	// PUBLIC MEMBERS
	[field: SerializeField]
	public CinemachineVirtualCamera VirtualCamera { get; private set; }
	
	/// <summary>
	/// Is main camera in cinematic or not?
	/// </summary>
	public bool IsInCinematic;
	
	/// <summary>
	/// Reference Resolution like 1920x1080
	/// </summary>
	public Vector2 ReferenceResolution;

	/// <summary>
	/// Zoom factor to fit different aspect ratios
	/// </summary>
	public Vector3 ZoomFactor = Vector3.one;
	
	[SerializeField] 
	public Player player;
	[SerializeField] 
	public CameraSetting setting;
	
	// PUBLIC METHODS
	public void Move(){
		if (IsInCinematic || player == null)
			return;
		
		var oInterpolateByDistance = player.Curve.InterpolateByDistance(player.travelledDst);
		var position = CalculateResponsive(oInterpolateByDistance + VirtualCamera.transform.forward * setting.offset.z + VirtualCamera.transform.up * setting.offset.y + VirtualCamera.transform.right * setting.offset.x);
		VirtualCamera.transform.position = new Vector3(position.x, position.y, position.z);
	}

	public void Rotate(){
		if (IsInCinematic || player == null)
			return;
		
		VirtualCamera.transform.rotation = Quaternion.LookRotation(player.Curve.GetTangentByDistance(player.travelledDst));
	}

	public Vector3 CalculateResponsive(Vector3 originPosition){
		var refRatio = ReferenceResolution.x / ReferenceResolution.y;
		var ratio = (float)Screen.width / (float)Screen.height;

		return originPosition + VirtualCamera.transform.forward * ((1f - refRatio / ratio) * ZoomFactor.z)
		                                    + VirtualCamera.transform.right * ((1f - refRatio / ratio) * ZoomFactor.x)
		                                    + VirtualCamera.transform.up * ((1f - refRatio / ratio) * ZoomFactor.y);
	}
	
	// DATA STRUCTURE
	[Serializable]
	public class CameraSetting{
		public Transform target;
		public Vector3   offset;
	}
}