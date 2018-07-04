using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
	public GameObject popupDeath;
	public GameObject popupResult;
	public GameObject popupNewHighscore;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameSettings.instance.State == GameSettings.GameState.GAME_OVER)
		{
			popupDeath.SetActive(true);
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
		GameSettings.instance.ResetGame();
		GameSettings.instance.State = GameSettings.GameState.PAUSE;
		popupNewHighscore.SetActive(true);
	}

	public void OnNewJourneyButtonPressed()
	{
		popupNewHighscore.SetActive(false);
		popupResult.SetActive(false);
		GameSettings.instance.State = GameSettings.GameState.IN_GAME;
	}

	public void OnGoHomeButtonPressed()
	{
		popupNewHighscore.SetActive(false);
		popupResult.SetActive(false);
		GameSettings.instance.State = GameSettings.GameState.MAIN_MENU;
	}
}
