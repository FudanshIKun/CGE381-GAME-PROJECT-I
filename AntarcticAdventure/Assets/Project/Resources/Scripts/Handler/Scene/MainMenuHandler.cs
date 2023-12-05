using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Device.Application;

public sealed class MainMenuHandler : Handler<MainMenuHandler>{
	// PUBLIC MEMBERS
	public PlayableDirector TimelinePlayer;
	
	// PRIVATE MEMBERS
	[SerializeField] 
	private UIContainer UC_MainMenu;
	[SerializeField]
	private UIContainer UC_WarningForNewGame;
	[SerializeField]
	private UIContainer UC_QuestionForNewGame;
	[SerializeField]
	private UIContainer UC_QuestionForContinueGame;
	[SerializeField]
	private UIContainer UC_WarningForContinueGame;
	[SerializeField] 
	private UIContainer UC_VerifyLeaving;
    [SerializeField]
	private PlayableAsset NewGameSequence;
	[SerializeField]
	private PlayableAsset ContinueGameSequence;
	[SerializeField]
	private UIToggle tg_music;
	[SerializeField]
	private UIToggle tg_sfx;

	[SerializeField] 
	private List<TMP_Text> ScoreCards = new();
	
	// PUBLIC METHODS
	public void QuestionNewGame(){
		if (PlayerPrefManager.Instance.GetLastStageNumber() == 10){
			Debug.Log("[MainMenuHandler] WarningNewGame");
			SoundHandler.Instance.PlayUISubmit();
			UC_MainMenu.Hide();
			UC_WarningForNewGame.Show();
		}
		else if (PlayerPrefManager.Instance.GetLastStageNumber() != 0 && PlayerPrefManager.Instance.GetLastStageNumber() <= 9){
			Debug.Log("[MainMenuHandler] QuestionNewGame");
			SoundHandler.Instance.PlayUISubmit();
			UC_MainMenu.Hide();
			UC_QuestionForNewGame.Show();
		}
		else{
			Debug.Log("[MainMenuHandler] NewGame");
			NewGame();
		}
	}

	public void NewGame(){
		SoundHandler.Instance.PlayUIIntoGame();
		PlayerPrefManager.Instance.CreateRecord();
		TimelinePlayer.Play(NewGameSequence);
	}

	public void LoadFirstStage(){
		Debug.Log("[MainMenuHandler] LoadFirstStage");
		SoundHandler.Instance.StopMusic();
		SceneManager.LoadScene(GameManager.Scenes.Level1.ToString());
	}

	public void QuestionContinueGame(){
		Debug.Log("[MainMenuHandler] ContinueGame");
		if (!PlayerPrefs.HasKey(PlayerPrefManager.LastStagePrefix)){
			SoundHandler.Instance.PlayUISubmit();
			UC_MainMenu.Hide();
			UC_WarningForContinueGame.Show();
		}
		else if (PlayerPrefManager.Instance.GetLastStageNumber() <= 9){
			SoundHandler.Instance.PlayUIIntoGame();
			TimelinePlayer.Play(ContinueGameSequence);
		}
		else{
			SoundHandler.Instance.PlayUISubmit();
			UC_MainMenu.Hide();
			UC_QuestionForContinueGame.Show();
		}
	}

	public void ContinueGame(){
		SoundHandler.Instance.PlayUIIntoGame();
		TimelinePlayer.Play(ContinueGameSequence);
	}

	public void LoadLastPlayedStage(){
		Debug.Log("[MainMenuHandler] LoadLastPlayedStage");
		SoundHandler.Instance.StopMusic();
		var lastStage = Enum.ToObject(typeof(GameManager.Scenes), PlayerPrefManager.Instance.GetLastStageNumber()).ToString();
		SceneManager.LoadScene(lastStage);
	}
	
	public void LoadScoreBoard(){
		Debug.Log("[MainMenuHandler] LoadScoreBoard");
		var counter = 0;
		foreach (var pair in PlayerPrefManager.Instance.HighestScores){
			var score = pair.Value;
			ScoreCards[counter].text = score.ToString("D4");
			counter++;
		}
	}

	public void OnShowSetting(){
		tg_music.isOn = SoundManager.Instance.music_enabled;
		tg_sfx.isOn = SoundManager.Instance.sfx_enabled;
	}

	public void LeaveGame(){
		Debug.Log("[MainMenuHandler] LeaveGame");
		UC_MainMenu.Hide();
		UC_VerifyLeaving.Show();
	}

	public void Quit() => Application.Quit();
	
}