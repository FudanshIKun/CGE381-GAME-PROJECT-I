using System;
using System.Collections.Generic;
using System.Linq;
using FluffyUnderware.Curvy;
using UnityEngine;

[ExecuteInEditMode]
public sealed class CopterHandler : Handler<CopterHandler>{
#if UNITY_EDITOR
	// PUBLIC MEMBERS
	public List<Copter> Copters = new ();
	[Header("Setting")] 
	public float pickUpLevel;
    
	// PRIVATE MEMBERS
	[Header("Required")]
	[SerializeField] 
	private CurvySpline Curve;
	
	// MonoBehavior INTERFACE
	private void Update(){
		if (Application.isEditor && !Application.isPlaying){
			var copters = GetComponentsInChildren<Copter>();
			var copterSet = new HashSet<Copter>(Copters);
			copterSet.RemoveWhere(c => !Array.Exists(copters, copter => copter == c));

			if (Curve != null && copters.Length > 0)
				foreach (var c in copters){
					copterSet.Add(c);

					var point = Curve.InterpolateByDistance(c.distance);
					var rotation = Curve.GetTangentByDistance(c.distance);
					c.transform.position = new Vector3(point.x + c.offset, pickUpLevel, point.z);
					c.transform.rotation = Quaternion.LookRotation(rotation);
				}
			
			Copters = copterSet.ToList();
			Copters.Sort((a, b) => a.CompareTo(b));
		}
	}
#endif	
}