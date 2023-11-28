using System;
using System.Collections.Generic;
using FluffyUnderware.Curvy;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
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
	private void Start(){
		if (!Application.isEditor || Application.isPlaying){
			Initialize();
		}
	}

	private void Update(){
		if (Application.isEditor && !Application.isPlaying){
			timer += Time.deltaTime;
			if (timer >= spawnRate){
				PreviewCloud();
				timer = 0f;
			}
		}
		else{
			timer += Time.deltaTime;
			if(Spawning)
				if (timer >= spawnRate){
					SpawningCloud();
					timer = 0f;
				}
		}
	}
	
	// PUBLIC METHODS
	public void startSpawning() => Spawning = true;
	public void StopSpawning()  => Spawning = false;

	// PRIVATE METHODS
	private void Initialize(){
	}

	private void PreviewCloud(){
		if (LevelHandler.Instance == null || LevelHandler.Instance.Curve == null)
			return;
		
		var previewPosition = new Vector3(
			LevelHandler.Instance.Curve.InterpolateByDistance(LevelHandler.Instance.Player.travelledDst).x + Random.Range(-spawnMaxOffset, spawnMaxOffset),
			HoleHandler.Instance.groundLevel + spawnHeight,
			LevelHandler.Instance.Curve.InterpolateByDistance(LevelHandler.Instance.Player.travelledDst).z + spawnDistance
		);
			
		cloudParticleSystems[^1].gameObject.transform.position = previewPosition;
		cloudParticleSystems[^1].Stop();
		cloudParticleSystems[^1].time = 0f;
		cloudParticleSystems[^1].Play();
	}

	private void SpawningCloud(){
		if (LevelHandler.Instance == null || LevelHandler.Instance.Curve == null)
			return;
		
		if (counter >= 4)
			counter = 0;
		
		var cloudParticleSystem = cloudParticleSystems[counter];
		cloudParticleSystem.Stop();
		cloudParticleSystem.gameObject.SetActive(false);
		
		var spawnPosition = new Vector3(
			LevelHandler.Instance.Curve.InterpolateByDistance(LevelHandler.Instance.Player.travelledDst).x + Random.Range(-spawnMaxOffset, spawnMaxOffset),
			HoleHandler.Instance.groundLevel + spawnHeight,
			LevelHandler.Instance.Curve.InterpolateByDistance(LevelHandler.Instance.Player.travelledDst).z + spawnDistance
		);
		
		cloudParticleSystem.gameObject.SetActive(true);
		cloudParticleSystem.gameObject.transform.position = spawnPosition;
		cloudParticleSystem.Play();

		counter++;
	}
}