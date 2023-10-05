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
			// Create a HashSet to store Hole objects
			var copterSet = new HashSet<Copter>(Copters);

			// Get all Hole components in children
			var holes = GetComponentsInChildren<Copter>();

			// Remove elements from Holes HashSet that no longer exist
			copterSet.RemoveWhere(h => !Array.Exists(holes, hole => hole == h));

			foreach (var h in holes){
				copterSet.Add(h);

				var point = Curve.InterpolateByDistance(h.distance);
				var rotation = Curve.GetTangentByDistance(h.distance);
				h.transform.position = new Vector3(point.x + h.offset, pickUpLevel, point.z);
				h.transform.rotation = Quaternion.LookRotation(rotation);
			}
			
			Copters = copterSet.ToList();
			Copters.Sort((a, b) => a.CompareTo(b));
		}
	}
#endif	
}