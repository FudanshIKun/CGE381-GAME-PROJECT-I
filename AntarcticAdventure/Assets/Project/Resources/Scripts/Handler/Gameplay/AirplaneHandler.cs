using System;
using System.Collections.Generic;
using System.Linq;
using FluffyUnderware.Curvy;
using UnityEngine;

[ExecuteInEditMode]
public sealed class AirplaneHandler : Handler<AirplaneHandler>{
#if UNITY_EDITOR
	// PUBLIC MEMBERS
	public List<AirplaneTrigger> Airplanes = new ();
	[Header("Setting")]
	public float skyLevel;
	public float groundLevel;
	public float flyStartPointXOffset;
	public float flyPathZOffset;
    
	// PRIVATE MEMBERS
	[Header("Required")]
	[SerializeField] 
	private CurvySpline Curve;
	[SerializeField] 
	private GameObject AirplaneModel;
	
	// MonoBehavior INTERFACE
	private void Update(){
		if (Application.isEditor && !Application.isPlaying){
			var airplanes = GetComponentsInChildren<AirplaneTrigger>();
			var hashSet = new HashSet<AirplaneTrigger>(Airplanes);
			hashSet.RemoveWhere(a => !Array.Exists(airplanes, airplane => airplane == a));

			if (Curve != null && airplanes.Length > 0)
				foreach (var a in airplanes){
					hashSet.Add(a);
				
					var point = Curve.InterpolateByDistance(a.distance);
					var rotation = Curve.GetTangentByDistance(a.distance);
					var rightDir = new Vector3(rotation.z, 0, -rotation.x);
					a.transform.position = new Vector3(point.x, groundLevel, point.z);
					a.transform.rotation = Quaternion.LookRotation(rotation);

					if (a.Airplane != null){
						a.Airplane.transform.position = new Vector3(point.x - Mathf.Abs(flyStartPointXOffset), skyLevel, point.z + flyPathZOffset);
						a.Airplane.transform.rotation = Quaternion.LookRotation(rightDir);
					}
					else
					if (AirplaneModel != null)
						a.Airplane = Instantiate(AirplaneModel, new Vector3(point.x - Mathf.Abs(flyStartPointXOffset), skyLevel, point.z + flyPathZOffset), Quaternion.LookRotation(rightDir), a.gameObject.transform);
				
					if (a.Destination != null){
						a.Destination.transform.position = new Vector3(point.x + Mathf.Abs(flyStartPointXOffset), skyLevel, point.z + flyPathZOffset);
						a.Destination.transform.rotation = Quaternion.LookRotation(rightDir);
					}
					else
						a.Destination = Instantiate(new GameObject("Destination"), new Vector3(point.x + Mathf.Abs(flyStartPointXOffset), skyLevel, point.z + flyPathZOffset), Quaternion.LookRotation(rightDir), a.gameObject.transform).transform;
				}
			
			Airplanes = hashSet.ToList();
			Airplanes.Sort((a, b) => a.CompareTo(b));
		}
	}
#endif
}