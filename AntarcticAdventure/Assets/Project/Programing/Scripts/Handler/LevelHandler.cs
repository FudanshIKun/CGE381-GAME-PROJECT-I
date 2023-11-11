using System;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using FluffyUnderware.Curvy;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public sealed class LevelHandler : Handler<LevelHandler>{
	// Public Members
	public Player             Player;
	public CurvySpline        Curve;
	public GameManager.Scenes Level;
	public int                score;
	public Destination        destination;
	public PlayableDirector   TimelinePlayer;
	public PlayableAsset      OnEnterStage;
	public PlayableAsset      OnStageCleared;
	public PlayableAsset      OnGameOver;

	// PRIVATE MEMBERS
	[SerializeField] 
	private int minutes = 1;
	[SerializeField] 
	private int seconds = 30;
	[SerializeField] 
	private Image i_fillTimer;
	[SerializeField] 
	private Image i_fillDistance;
	[SerializeField] 
	private Transform tf_scoreIconTarget;
	[SerializeField] 
	private Image tf_movableScoreIcon;
	[SerializeField] 
	private TMP_Text t_score;
	[SerializeField] 
	private UIContainer ctn_Transition;
	[SerializeField] 
	private UIContainer ctn_Gameplay;
	[SerializeField] 
	private UIContainer ctn_Pause;
	[SerializeField]
	private UIContainer ctn_StageCleared;
	[SerializeField]
	private UIContainer ctn_GameOver;

	private float totalTime;
	private float currentTime;
	private bool  startTimer;
	private bool  levelComplete;
	private bool  gameHasOver;
	
	// MonoBehavior Interface
	private void OnValidate(){
		destination ??= GetComponentInChildren<Destination>();
		if (destination == null){
			if (Curve == null){
				Debug.Log("LevelHandler require CurvySpline GameObject in the same scene to proceed");
				return;
			}
			
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
			EnterStage();
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
		if (Level != GameManager.Scenes.MainMenu 
		    && Level != GameManager.Scenes.Map){
			if (levelComplete)
				PlayerPrefManager.Instance.RecordScore(Level, score);
			
			PlayerPrefManager.Instance.RecordLastLevel(Level);
		}
	}
	
	// PUBLIC METHODS
	public void StartTimer() => startTimer = true;
	
	public void StageCleared(){
		Debug.Log("StageCleared");
		levelComplete = true;
		PlayerPrefManager.Instance.RecordLastLevel(Level);
		TimelinePlayer.Play(OnStageCleared);
	}

	public void RestartStage(){
		SceneManager.LoadScene(Level.ToString());
	}

	public void LoadMapStage(){
		SceneManager.LoadScene(GameManager.Scenes.Map.ToString());
	}
	
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
		var dst = Player.travelledDst / Curve.Length;
		i_fillDistance.fillAmount = dst;
		var timeAmount = currentTime / totalTime;
		i_fillTimer.fillAmount = timeAmount;
		t_score.text = score.ToString().PadLeft(4, '0');
	}
	
	private void CheckWinConditions(){
		if (Player.HasFinished){
			return;
		}
		
		if (currentTime > 0){
			if (!startTimer)
				return;
			currentTime -= Time.deltaTime;
			return;
		}

		GameOver();
	}
	
	private void EnterStage(){
		Debug.Log("EnterStage");
		TimelinePlayer.Play(OnEnterStage);
	}

	private void GameOver(){
		Debug.Log("Timeout");
		if (gameHasOver)
			return;

		gameHasOver = true;
		TimelinePlayer.Play(OnGameOver);
	}
}