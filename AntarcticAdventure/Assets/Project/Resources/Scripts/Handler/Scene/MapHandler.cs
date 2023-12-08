using System;
using System.Collections;
using DG.Tweening;
using FluffyUnderware.Curvy;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public sealed class MapHandler : Handler<MapHandler>{
	// PUBLIC MEMBERS
	public GameObject       PlayerGO;
	public Animator         Animator;
	public CurvySpline      Curve;
	public PlayableDirector TimelinePlayer;
	
	// PRIVATE MEMBERS
	[SerializeField]
	private bool IsTesting;
	[Header("Sequencing")]
	[SerializeField]
	private PlayableAsset OnLeave;
	[Header("Line Rendering")]
	[SerializeField] 
	private LineRenderer journeyLine;
	[SerializeField] 
	private float drawBaseLineDuration = 1f;
	[Header("Player Movement")] 
	[SerializeField]
	private float MoveDuration = 2f;
	private int lastPoint;
	
	// MonoBehavior INTERFACE
	private void Start(){
		Initialize();
	}
	
	// PUBLIC METHODS
	public void Launch(){
		StartCoroutine(DrawLine(
			journeyLine,
			drawBaseLineDuration,
			onSuccess: () => {
				StartCoroutine(MovePlayer(
					Curve, 
					PlayerGO, 
					Animator, 
					2f, 
					onSuccess: () => {
						if (IsTesting)
							return;
						
						TimelinePlayer.Play(OnLeave);
					},
					2f
				));
			})
		);
	}

	public void Leave(){
		LoadNextStage(lastPoint);
	}

	// PRIVATE METHODS
	private void Initialize(){
		journeyLine.enabled = false;
		lastPoint = IsTesting ? 0 : PlayerPrefManager.Instance.GetLastStageNumber();
		SetStartPoint(lastPoint);
	}

	private void SetStartPoint(int lastPoint){
		var pos = Curve.ControlPointsList[IsTesting ? 0 : lastPoint - 1].transform.position;
		PlayerGO.transform.position = pos;
		
		var direction = Curve.ControlPointsList[IsTesting ? 1 : lastPoint].transform.position - PlayerGO.transform.position;
		PlayerGO.transform.rotation = Quaternion.LookRotation(direction);
	}
	
	private IEnumerator DrawLine(LineRenderer line, float duration = 0f, float? startDelaySec = 0f, Action onSuccess = null, float? onSuccessDelay = 0f){
		var pointCount = line.positionCount;
		var linePoints = new Vector3[pointCount];
		for (var i = 0; i < pointCount; i++){
			linePoints[i] = line.GetPosition(i);
		}

		yield return new WaitForSeconds(startDelaySec ?? 0f);
		
		var segmentDuration = duration / pointCount;
		line.enabled = true;
		for (var i = 0; i < pointCount - 1; i++){
			var initialTime = Time.time;
			var startPos = linePoints[i];
			var endPos = linePoints[i + 1];

			var pos = startPos;
			while (pos != endPos){
				var time = (Time.time - initialTime) / segmentDuration;
				pos = Vector3.Lerp(startPos, endPos, time);

				for (var j = i + 1; j < pointCount; j++){
					line.SetPosition(j, pos);
				}

				yield return null;
			}
		}
		
		yield return new WaitForSeconds(onSuccessDelay ?? 0f);
		onSuccess?.Invoke();
	}
	
	private IEnumerator MovePlayer(CurvySpline curve, GameObject player, Animator animator, float? startDelaySec = 0f, Action onSuccess = null, float? onSuccessDelay = 0f){
		yield return new WaitForSeconds(startDelaySec ?? 0f);
		
		var nextPoint = lastPoint;
		animator.SetBool(PlayerAnimation.IsRunning, true);
		var pos = curve.ControlPointsList[nextPoint].transform.position;
		player.transform.DOMove(pos, MoveDuration)
			.OnComplete((() => {
				animator.SetBool(PlayerAnimation.IsRunning, false);
			}));
		
		yield return new WaitForSeconds(onSuccessDelay ?? 0f);
		onSuccess?.Invoke();
	}

	private void LoadNextStage(int value)
		=> SceneManager.LoadScene(CalculateNextScene(value));

	private string CalculateNextScene(int value){
		var sceneName = Enum.ToObject(typeof(GameManager.Scenes), value).ToString();
		return sceneName;
	}
}