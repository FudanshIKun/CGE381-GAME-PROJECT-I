using System;
using UnityEngine;

[Serializable]
public sealed class GameplayCamera{
	public GameplayCamera(Camera camera){
		this.camera = camera;
	}
	
	// PUBLIC MEMBERS
	public Camera camera;
	
	[SerializeField] 
	public Player player;
	[SerializeField] 
	public CameraSetting setting;
	
	// PUBLIC METHODS
	public void Move(){
		var position = player.Curve.InterpolateByDistance(player.travelledDst);
		var offset = setting.offset;
		camera.transform.position = new Vector3(position.x + offset.x, position.y + offset.y, position.z + offset.z);
	}

	public void Rotate(){
		camera.transform.rotation = Quaternion.LookRotation(new Vector3(camera.transform.rotation.x, player.transform.rotation.y, camera.transform.rotation.z));
	}
	
	[Serializable]
	public class CameraSetting{
		public Transform target;
		public Vector3   offset;
	}
}