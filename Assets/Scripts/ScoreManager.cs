using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public Text labelScore;

	private int m_score = 0;
	public int Score
	{
		get
		{
			return m_score;
		}
		set
		{
			m_score = value;
			labelScore.text = m_score.ToString();
		}
	}
	public int scorePerTick;
	public int tickPerSecond;

	private float tickInterval;
	private float lastTickTime;

	private void Awake()
	{
		tickInterval = 1f / tickPerSecond;
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		lastTickTime += Time.deltaTime;
		if (lastTickTime >= tickInterval)
		{
			Score += scorePerTick;
			lastTickTime -= tickInterval;
		}
	}
}
