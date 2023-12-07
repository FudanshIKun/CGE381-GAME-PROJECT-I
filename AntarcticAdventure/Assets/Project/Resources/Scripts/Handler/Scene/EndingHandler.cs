using System.Collections;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public sealed class EndingHandler : Handler<EndingHandler>{
	public bool HasFinished;

	[SerializeField]
	private PlayableDirector timeline;
	[SerializeField]
	private PlayableAsset endingSequence;
	[SerializeField] 
	private UIContainer uc_pressToContinue;

	private bool _hasContinued;
	
	// MonoBehavior INTERFACE
	private void Update(){
		if (HasFinished){
			if (_hasContinued)
				return;
			
			if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetButton("Submit")){
				_hasContinued = true;
				timeline.Play(endingSequence);
			}
		}
	}
	
	// PUBLIC METHODS
	public void StartFading()
		=> StartCoroutine(Fading());
	
	public void LoadMainMenu()
		=> SceneManager.LoadScene(GameManager.Scenes.MainMenu.ToString());

	public void HasFinish()
		=> HasFinished = true;
	
	// PRIVATE METHODS
	private IEnumerator Fading(){
		while (HasFinished){
			uc_pressToContinue.Toggle();
			yield return new WaitForSeconds(1.2f);
		}
	}
}