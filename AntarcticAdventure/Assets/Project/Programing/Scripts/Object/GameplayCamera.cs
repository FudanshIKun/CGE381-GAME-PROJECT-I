using System;
using Cinemachine;
using UnityEngine;

[Serializable]
public sealed class GameplayCamera{
	// PUBLIC MEMBERS
	[field: SerializeField]
	public CinemachineVirtualCamera VirtualCamera { get; private set; }
	
	[SerializeField] 
	public Player player;
	[SerializeField] 
	public CameraSetting setting;
	
	// PUBLIC METHODS
	public void Move(){
		var position = player.Curve.InterpolateByDistance(player.travelledDst);
		var offset = setting.offset;
		VirtualCamera.transform.position = new Vector3(position.x + offset.x, position.y + offset.y, position.z + offset.z);
	}

	public void Rotate(){
		VirtualCamera.transform.rotation = Quaternion.LookRotation(new Vector3(VirtualCamera.transform.rotation.x, player.transform.rotation.y, VirtualCamera.transform.rotation.z));
	}
	
	[Serializable]
	public class CameraSetting{
		public Transform target;
		public Vector3   offset;
	}
}