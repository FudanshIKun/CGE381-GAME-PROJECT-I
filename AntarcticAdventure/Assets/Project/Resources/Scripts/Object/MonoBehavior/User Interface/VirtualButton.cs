using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
	public Action OnPointerDownCallback;
	public Action OnPointerUpCallback;
    
	[SerializeField] private float rotationLimit = 40;
	[SerializeField] private float rotationSpeed = 15;
	private                  bool  rotate        = false;

	// MonoBehavior INTERFACE
	private void FixedUpdate(){
		var targetRotate = rotate ? rotationLimit : 0f;

		// Rotate the cube by converting the angles into a quaternion.
		var target = Quaternion.Euler(targetRotate, 0, 0);

		// Dampen towards the target rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotationSpeed);
	}

	// IPointerDownHandler INTERFACE
	public void OnPointerDown(PointerEventData pointerEventData){
		rotate = true;
		OnPointerDownCallback?.Invoke();
	}

	// IPointerUpHandler INTERFACE
	public void OnPointerUp(PointerEventData pointerEventData){
		rotate = false;
		OnPointerUpCallback?.Invoke();
	}
}