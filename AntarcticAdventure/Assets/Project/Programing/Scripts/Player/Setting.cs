using System;
using UnityEngine;

[Serializable]
public sealed class Setting{
	// PUBLIC MEMBERS
	[field: SerializeField]
	public float baseSpeed            { get; private set; }
	[field: SerializeField]
	public float flySpeed             { get; private set; }
	[field: SerializeField]
	public float accelerateSpeed      { get; private set; }
	[field: SerializeField]
	public float maxSpeed             { get; private set; }
	[field: SerializeField]
	public float offsetAcceleration   { get; private set; } = 1f;
	[field: SerializeField]
	public float maxOffset            { get; private set; }
	[field: SerializeField]
	public float jumpVelocity         { get; private set; } = 1f;
	[field: SerializeField] 
	public float floatRay             { get; private set; } = .75f;
	[field: SerializeField]
	public float floatHeight          { get; private set; } = 2f;
	[field: SerializeField]
	public float flyHeight            { get; private set; } = 4f;
	[field: SerializeField]
	public float flyupSpeed           { get; private set; } = 1f;
	[field: SerializeField] 
	public float flyDuration          { get; private set; } = 3f;
	[field: SerializeField] 
	public float stumblingDistance    { get; private set; } = 1.5f;
	[field: SerializeField] 
	public float stumblingJumpPower   { get; private set; } = 1f;
	[field: SerializeField] 
	public float stumblingDuration    { get; private set; } = 1.5f;
	[field: SerializeField] 
	public int stumblingJumpAmount    { get; private set; } = 2;
	[field: SerializeField] 
	public float toFinishingDuration  { get; private set; } = 10;
	[field: SerializeField] 
	public float fishCollectingRadius { get; private set; } = 1f;
	[field: SerializeField]
	public LayerMask groundLayer      { get; private set; }
	[field: SerializeField]
	public LayerMask pickupLayer      { get; private set; }
	

	// PUBLIC METHODS
	public bool IsGroundLayer(int layer) => ContainLayer(groundLayer, layer);
	public bool IsPickupLayer(int layer) => ContainLayer(pickupLayer, layer);
	
	// PRIVATE METHODS
	private bool ContainLayer(LayerMask layerMask, int layer){
		return ((1 << layer) & layerMask) != 0;
	}
}