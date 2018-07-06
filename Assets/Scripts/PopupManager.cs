using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
	public GameObject popupDeath;
	public PopupResult popupResult;
	public PopupHighscore popupNewHighscore;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameSettings.instance.State == GameSettings.GameState.GAME_OVER && !(popupResult.gameObject.active || popupNewHighscore.gameObject.active))
		{
			popupDeath.SetActive(true);
			popupResult.SetScore(ScoreManager.instance.Score);
			popupNewHighscore.SetScore(ScoreManager.instance.Score);
		}
		else
		{
			popupDeath.SetActive(false);
		}
	}

	public void OnWatchAdsButtonPressed()
	{
		popupDeath.SetActive(false);
		//please show ads here as the cost for revival
		GameSettings.instance.Revive();
	}

	public void OnPayCurrencyButtonPressed()
	{
		popupDeath.SetActive(false);
		//please remove some currency here as the cost for revival
		GameSettings.instance.Revive();
	}

	public void OnGiveUpButtonPressed()
	{
		popupDeath.SetActive(false);
		GameSettings.instance.State = GameSettings.GameState.PAUSE;

		if (ScoreManager.instance.IsHighScore())
		{
			ScoreManager.instance.SaveHighScore();
			popupNewHighscore.gameObject.SetActive(true); 
		}
		else
		{
			popupResult.gameObject.SetActive(true);
		}
	}

	public void OnNewJourneyButtonPressed()
	{
		popupNewHighscore.gameObject.SetActive(false);
		popupResult.gameObject.SetActive(false);
		GameSettings.instance.ResetGame();
	}

	public void OnGoHomeButtonPressed()
	{
		popupNewHighscore.gameObject.SetActive(false);
		popupResult.gameObject.SetActive(false);
		GameSettings.instance.State = GameSettings.GameState.MAIN_MENU;
	}
}
