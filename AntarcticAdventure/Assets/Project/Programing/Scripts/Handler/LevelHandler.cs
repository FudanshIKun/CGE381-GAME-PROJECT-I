using System;
using DG.Tweening;
using FluffyUnderware.Curvy;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public sealed class LevelHandler : Handler<LevelHandler>{
	// Public Members
	public bool               TimeUp;
	public Player             Player;
	public CurvySpline        Curve;
	public GameManager.Scenes Level;
	public int                score;
	public Destination        destination;

	// PRIVATE MEMBERS
	[SerializeField] 
	private int minutes = 1;
	[SerializeField] 
	private int seconds = 30;
	[SerializeField] 
	private Image i_fillTimer;
	[SerializeField] 
	private TMP_Text t_rest;
	[SerializeField] 
	private Transform tf_scoreIconTarget;
	[SerializeField] 
	private Image tf_movableScoreIcon;
	[SerializeField] 
	private TMP_Text t_score;

	private float totalTime;
	private float currentTime;
	
	// MonoBehavior Interface
	private void OnValidate(){
		destination ??= GetComponentInChildren<Destination>();
		if (destination == null){
			if (Curve == null)
				return;
			
			destination = Instantiate(new GameObject("Destination"), Curve.InterpolateByDistance(0), Quaternion.LookRotation(Curve.GetTangentByDistance(0)), transform).AddComponent<Destination>();
			var box = destination.AddComponent<BoxCollider>();
			box.isTrigger = true;
		}
	}

	private void OnEnable(){
		if (Application.isEditor && !Application.isPlaying){
			Debug.Log(GetType().Name + " OnEnabled in EditMode");
		}
		else {
			Debug.Log(GetType().Name + " OnEnabled in PlayMode");
			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}
	}

	private void Start(){
		if (Application.isEditor && !Application.isPlaying){
			Debug.Log(GetType().Name + " Start in EditMode");
		}
		else {
			Debug.Log(GetType().Name + " Start in PlayMode");
			totalTime = minutes * 60 + seconds;
			currentTime = totalTime;
		}
	}

	private void Update(){
		if (Application.isEditor && !Application.isPlaying){
			if (Curve == null || destination == null)
				return;
			
			destination.transform.position = Curve.InterpolateByDistance(Mathf.Clamp(destination.distance, 0, Curve.Length));
			destination.transform.rotation = Quaternion.LookRotation(Curve.GetTangentByDistance(Mathf.Clamp(destination.distance, 0, Curve.Length)));
		}
		else {
			Debug.Log(GetType().Name + " Update in PlayMode");
			UpdateUserInterface();
			CheckWinConditions();
		}
	}

	private void OnDisable(){
		if (Application.isEditor && !Application.isPlaying){
			Debug.Log(GetType().Name + " OnDisable in EditMode");
		}
		else {
			Debug.Log(GetType().Name + " OnDisable in PlayMode");
			SceneManager.sceneUnloaded -= OnSceneUnloaded;
		}
	}

	public void OnSceneUnloaded(Scene scene){
		if (Level != GameManager.Scenes.Map){
			ScoreManager.Instance.RecordScore(Level, score);
		}
	}
	
	// PUBLIC METHODS
	public void OnFishContact(Fish fish){
		Debug.Log("OnFishContact");
		var screenPosition = Camera.main!.WorldToScreenPoint(fish.transform.position);
		tf_movableScoreIcon.transform.position = screenPosition;
		tf_movableScoreIcon.transform.DOMove(tf_scoreIconTarget.position, 1f)
			.OnStart((() => {
				tf_movableScoreIcon.transform.DOScale(Vector3.one, .5f);
			}))
			.OnComplete((() => {
				tf_movableScoreIcon.transform.DOScale(Vector3.zero, .5f);
			}));
	}
	
	// PRIVATE METHODS
	private void UpdateUserInterface(){
		var rest = Mathf.FloorToInt(Curve.Length - Player.travelledDst);
		t_rest.text = rest.ToString().PadLeft(4, '0');
		t_score.text = score.ToString().PadLeft(4, '0');
	}
	
	private void CheckWinConditions(){
		if (Player.IsFinishing){
			t_rest.text = "0000";
			return;
		}
		
		if (Player.HasFinished){
			return;
		}
		
		if (currentTime > 0){
			currentTime -= Time.deltaTime;
			var fillAmount = currentTime / totalTime;
			i_fillTimer.fillAmount = fillAmount;
			return;
		}

		Timeout();
	}

	private void Timeout(){
		Debug.Log("Timeout");
	}
}