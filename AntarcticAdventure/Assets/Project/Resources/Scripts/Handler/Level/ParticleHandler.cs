using System;
using System.Collections.Generic;
using FluffyUnderware.Curvy;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class ParticleHandler : Handler<ParticleHandler>{
	// PUBLIC MEMBERS
	public bool Spawning;
	
	// PRIVATE MEMBERS
	[SerializeField] 
	private CurvySpline Curve;
	[SerializeField] 
	private List<ParticleSystem> cloudParticleSystems = new();
	[SerializeField]
	private float spawnDistance = 20f;
	[SerializeField] 
	private float spawnMaxOffset = 3f;
	[SerializeField]
	private float spawnRate = 3f;
	[SerializeField]
	private float spawnHeight = 10f;
	
	private float timer;
	private int   counter;
	
	// MonoBehavior INTERFACE
	private void Update(){
		timer += Time.deltaTime;
		if(Spawning)
			if (timer >= spawnRate){
				SpawningCloud();
				timer = 0f;
			}
	}
	
	// PUBLIC METHODS
	public void startSpawning() => Spawning = true;
	public void StopSpawning()  => Spawning = false;

	// PRIVATE METHODS
	private void SpawningCloud(){
		if (LevelHandler.Instance == null || LevelHandler.Instance.Curve == null)
			return;
		
		if (counter >= 4)
			counter = 0;
		
		var cloudParticleSystem = cloudParticleSystems[counter];
		cloudParticleSystem.Stop();
		cloudParticleSystem.gameObject.SetActive(false);

		var player = LevelHandler.Instance.Player;
		var point = LevelHandler.Instance.Curve.InterpolateByDistance(LevelHandler.Instance.Player.travelledDst);
		var spawnPosition = new Vector3(point.x, HoleHandler.Instance.groundLevel + spawnHeight, point.z) 
		                    + player.transform.right * Random.Range(-spawnMaxOffset, spawnMaxOffset)
		                    + player.transform.forward * + spawnDistance;
		
		cloudParticleSystem.gameObject.SetActive(true);
		cloudParticleSystem.gameObject.transform.position = spawnPosition;
		cloudParticleSystem.Play();

		counter++;
	}
}