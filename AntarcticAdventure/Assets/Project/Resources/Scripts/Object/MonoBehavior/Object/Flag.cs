using DG.Tweening;
using UnityEngine;

public sealed class Flag : MonoBehaviour{
	[SerializeField]
	private GameObject flag;
	[SerializeField]
	private Transform _raisePoint;
	[SerializeField]
	private float _raiseDuration;

	// PUBLIC METHODS
	public void RaiseFlag()
		=> flag.transform.DOMove(_raisePoint.position, _raiseDuration);
}