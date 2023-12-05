using System;
using System.Collections.Generic;
using System.Linq;
using FluffyUnderware.Curvy;
using UnityEngine;

[ExecuteInEditMode]
public sealed class HoleHandler : Handler<HoleHandler>{
	// PUBLIC MEMBERS
	public List<Hole> Holes = new ();
	[Header("Setting")] 
	public float groundLevel;
    
	// PRIVATE MEMBERS
	[Header("Required")]
	[SerializeField] 
	private CurvySpline Curve;
	
	// MonoBehavior INTERFACE
	private void Update(){
	#if UNITY_EDITOR
		if (Application.isEditor && !Application.isPlaying){
			var holes = GetComponentsInChildren<Hole>();
			var holeSet = new HashSet<Hole>(Holes);
			holeSet.RemoveWhere(h => !Array.Exists(holes, hole => hole == h));

			if (Curve != null && holes.Length > 0)
				foreach (var h in holes){
					holeSet.Add(h);

					var point = Curve.InterpolateByDistance(h.distance);
					var rotation = Curve.GetTangentByDistance(h.distance);
					h.transform.rotation = Quaternion.LookRotation(rotation);
					h.transform.position = new Vector3(point.x, groundLevel, point.z) + h.transform.right * h.offset;
					
					var index = Holes.FindIndex(x => x == h);
					h.transform.SetSiblingIndex(index);
				}
			
			Holes = holeSet.ToList();
			Holes.Sort((a, b) => a.CompareTo(b));
		}
	#endif
	}
}