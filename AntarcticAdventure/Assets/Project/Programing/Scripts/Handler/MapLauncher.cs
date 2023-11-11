using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public sealed class MapLauncher : MonoBehaviour{
	// PRIVATE MEMBERS
	[SerializeField] 
	private LineRenderer baseLineRenderer;
	[SerializeField] 
	private float drawBaseLineDuration = 1f;
	
	
	// MonoBehavior INTERFACE
	private void Awake(){
		
	}

	private void Start(){
		StartCoroutine(DrawLine(baseLineRenderer, drawBaseLineDuration));

	}

	// PRIVATE METHODS
	private IEnumerator DrawLine(LineRenderer line, float duration, Action postDraWAction = null){
		line.enabled = true;
		var pointCount = line.positionCount;
		var linePoints = new Vector3[pointCount];
		for (var i = 0; i < pointCount; i++){
			linePoints[i] = line.GetPosition(i);
		}
		
		var segmentDuration = duration / pointCount;
		for (var i = 0; i < pointCount - 1; i++){
			var initialTime = Time.time;
			var startPos = linePoints[i];
			var endPos = linePoints[i + 1];

			var pos = startPos;
			while (pos != endPos){
				var t = (Time.time - initialTime) / segmentDuration;
				pos = Vector3.Lerp(startPos, endPos, t);

				for (var j = i + 1; j < pointCount; j++){
					line.SetPosition(j, pos);
				}

				yield return null;
			}
		}
		
		postDraWAction?.Invoke();
	}
}