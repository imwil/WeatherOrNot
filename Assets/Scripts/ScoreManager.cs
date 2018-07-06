using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;

	public Text labelScore;
	public Text labelHighScore;
	public float baseScorePerTick;
	public int tickPerSecond;
	public float multiplier;

	public float HighScore
	{
		get
		{
			return PlayerPrefs.GetFloat(scoreKey, 0);
		}
		set
		{
			labelHighScore.text = ((int)value).ToString();
			PlayerPrefs.SetFloat(scoreKey, value);
			PlayerPrefs.Save();
		}
	}

	private float m_score = 0;
	public float Score
	{
		get
		{
			return m_score;
		}
		set
		{
			m_score = value;
			labelScore.text = ((int)m_score).ToString();
		}
	}
	public float ScorePerTick { get; set; }

	private const string scoreKey = "Score";

	private float tickInterval;
	private float lastTickTime;

	private void Awake()
	{
		instance = this;
		
		Score = 0;
		labelHighScore.text = ((int)HighScore).ToString();

		tickInterval = 1f / tickPerSecond;
		lastTickTime = 0f;
		ApplyMultiplier(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		lastTickTime += Time.deltaTime;
		if (GameSettings.instance.State == GameSettings.GameState.IN_GAME && lastTickTime >= tickInterval)
		{
			Score += ScorePerTick;
			lastTickTime -= tickInterval;
		}
	}

	public void ApplyMultiplier(bool isApply)
	{
		if (isApply)
		{
			ScorePerTick = baseScorePerTick * multiplier;
		}
		else
		{
			ScorePerTick = baseScorePerTick;
		}
	}

	public void SaveHighScore()
	{
		HighScore = Score;
	}

	public bool IsHighScore()
	{
		return Score > HighScore;
	}
}
