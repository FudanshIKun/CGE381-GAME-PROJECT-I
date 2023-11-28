using UnityEngine;

public sealed class MobileController : MonoBehaviour{
	[field: SerializeField]
	public VirtualButton UpButton { get; private set; }
	[field: SerializeField]
	public VirtualButton LeftButton { get; private set; }
	[field: SerializeField]
	public VirtualButton RightButton { get; private set; }
	[field: SerializeField]
	public VirtualButton JumpButton { get; private set; }
}