﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
	public static HUDManager instance;

	[SerializeField] private Button pauseButton;
	[SerializeField] private Button shieldButton;
	[SerializeField] private Button doubleScoreButton;
	[SerializeField] private Button slowTimeButton;
	[SerializeField] private Slider omegaBarSlider;
	[SerializeField] private Button startGameTestButton;
	[SerializeField] private Text lblGamePaused;

	private Button m_btnHandle;

	private void Awake()
	{
		instance = this;

		if (omegaBarSlider)
		{
			m_btnHandle = omegaBarSlider.handleRect.GetComponent<Button>();
		}
	}

	// Use this for initialization
	void Start()
	{
		if (pauseButton)
		{
			pauseButton.onClick.AddListener(() =>
			{
				GameSettings.instance.Pause();
				lblGamePaused.gameObject.SetActive(GameSettings.instance.State == GameSettings.GameState.PAUSE);
			});
		}
		if (shieldButton)
		{
			shieldButton.onClick.AddListener(() => { PowerUpManager.instance.Activate(PowerUpManager.Type.SHIELD); });
		}
		if (doubleScoreButton)
		{
			doubleScoreButton.onClick.AddListener(() => { PowerUpManager.instance.Activate(PowerUpManager.Type.SCORE_MULTIPLY); });
		}
		if (slowTimeButton)
		{
			slowTimeButton.onClick.AddListener(() => { PowerUpManager.instance.Activate(PowerUpManager.Type.TIME_SLOW); });
		}
		if (startGameTestButton)
		{
			startGameTestButton.onClick.AddListener(() => { GameSettings.instance.State = GameSettings.GameState.IN_GAME_TRANSITION; });
		}
		if (m_btnHandle)
		{
			m_btnHandle.onClick.AddListener(() =>
			{
				ObstacleManager.instance.DestroyAll();
				ResetOmegaBar();
			});
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (omegaBarSlider)
		{
			omegaBarSlider.value = (float)GameSettings.instance.KilledObstaclesCount / GameSettings.instance.maxKilledObstacles;
			if (omegaBarSlider.value == omegaBarSlider.maxValue)
			{
				m_btnHandle.interactable = true;
				m_btnHandle.transition = Button.Transition.ColorTint;
			}
		}
	}

	public void ResetOmegaBar()
	{
		GameSettings.instance.KilledObstaclesCount = 0;
		omegaBarSlider.value = 0f;
		m_btnHandle.interactable = false;
		m_btnHandle.transition = Button.Transition.None;
	}
}
