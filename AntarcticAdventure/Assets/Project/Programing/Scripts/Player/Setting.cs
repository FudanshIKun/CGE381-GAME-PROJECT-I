using System;
using UnityEngine;

[Serializable]
public sealed class Setting{
	// PUBLIC MEMBERS
	[field: SerializeField]
	public float baseSpeed          { get; private set; }
	[field: SerializeField]
	public float flySpeed           { get; private set; }
	[field: SerializeField]
	public float accelerateSpeed    { get; private set; }
	[field: SerializeField]
	public float maxSpeed           { get; private set; }
	[field: SerializeField]
	public float offsetAcceleration { get; private set; } = 1f;
	[field: SerializeField]
	public float maxOffset          { get; private set; }
	[field: SerializeField]
	public float jumpVelocity       { get; private set; } = 1f;
	[field: SerializeField] 
	public float floatRay           { get; private set; } = .75f;
	[field: SerializeField]
	public float floatHeight        { get; private set; } = 2f;
	[field: SerializeField]
	public float flyHeight          { get; private set; } = 4f;
	[field: SerializeField]
	public float flyupSpeed         { get; private set; } = 1f;
	[field: SerializeField] 
	public float flyDuration        { get; private set; } = 3f;
	[field: SerializeField]
	public LayerMask GroundLayer    { get; private set; }

	// PUBLIC METHODS
	public bool IsGroundLayer(int layer){
		return ContainLayer(GroundLayer, layer);
	}
	
	// PRIVATE METHODS
	private bool ContainLayer(LayerMask layerMask, int layer){
		return ((1 << layer) & layerMask) != 0;
	}
}