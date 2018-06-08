using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
	[SerializeField] private Button pauseButton;
	[SerializeField] private Button shieldButton;
	[SerializeField] private Button doubleScoreButton;
	[SerializeField] private Button slowTimeButton;

	// Use this for initialization
	void Start ()
	{
		if (pauseButton)
			pauseButton.onClick.AddListener(() => { GameSettings.instance.IsGamePause = !GameSettings.instance.IsGamePause; });
		if (shieldButton)
			shieldButton.onClick.AddListener(() => { PowerUpManager.instance.Activate(PowerUpManager.Type.SHIELD); });
		if (doubleScoreButton)
			doubleScoreButton.onClick.AddListener(() => { PowerUpManager.instance.Activate(PowerUpManager.Type.SCORE_MULTIPLY); });
		if (slowTimeButton)
			slowTimeButton.onClick.AddListener(() => { PowerUpManager.instance.Activate(PowerUpManager.Type.TIME_SLOW); });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
