
using System;
using UnityEngine;

public sealed class InputHandler : Handler<InputHandler>{
	public bool IsPressingForward { get; private set; }
	public bool IsPressingLeft    { get; private set; }
	public bool IsPressingRight   { get; private set; }
	public bool IsPressingJump    { get; private set; }

	public bool       IsPrototypingMobile;

	[SerializeField]
	private Transform canvasTransform;
	[SerializeField]
	private GameObject controllerUIPrefab;
	private MobileController _controller;

	// MonoBehavior INTERFACE
	private void Start(){
		CheckPlatform();
	}

	private void Update(){
		if (_controller == null){
			IsPressingForward = Input.GetAxis("Vertical") > 0;
			IsPressingLeft = Input.GetAxis("Horizontal") < 0;
			IsPressingRight = Input.GetAxis("Horizontal") > 0;
			IsPressingJump = Input.GetButton("Jump");
		}
	}

	// PRIVATE METHODS
	private void CheckPlatform(){
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || IsPrototypingMobile) {
			Debug.Log("Running on mobile platform");
			_controller = Instantiate(controllerUIPrefab, canvasTransform).GetComponent<MobileController>();
			_controller.UpButton.OnPointerDownCallback += () => { IsPressingForward = true; };
			_controller.UpButton.OnPointerUpCallback += () => { IsPressingForward = false; };
			
			_controller.LeftButton.OnPointerDownCallback += () => { IsPressingLeft = true; };
			_controller.LeftButton.OnPointerUpCallback += () => { IsPressingLeft = false; };
			
			_controller.RightButton.OnPointerDownCallback += () => { IsPressingRight = true; };
			_controller.RightButton.OnPointerUpCallback += () => { IsPressingRight = false; };
			
			_controller.JumpButton.OnPointerDownCallback += () => { IsPressingJump = true; };
			_controller.JumpButton.OnPointerUpCallback += () => { IsPressingJump = false; };
		}
	}
}