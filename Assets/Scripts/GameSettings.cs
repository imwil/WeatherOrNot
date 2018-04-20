using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
	public static GameSettings m_instance = null;

	public float backgroundSpeed;
	public float obstacleSpeed;
	public float obstacleSpawnInterval;
	public int healthPoint;
	public Crew crew;
	public Text labelScore;
	
	public enum Lane
	{
		LEFT, MIDDLE, RIGHT, COUNT
	};
	public bool IsGameOver { get; private set; }

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

	private void Awake()
	{
		m_instance = this;
		Score = 0;
	} 

	// Use this for initialization
	void Start ()
	{
		var moveLeftRecognizer = new TKMultiDirectionalSwipeRecognizer();
		moveLeftRecognizer.addSwipeDirection(TKSwipeDirection.Left);
		moveLeftRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Move Left gesture recognizer fired: " + r);
			if (crew.Lane > GameSettings.Lane.LEFT)
			{
				crew.Lane -= 1;
			}
		};
		TouchKit.addGestureRecognizer(moveLeftRecognizer);

		var moveRightRecognizer = new TKMultiDirectionalSwipeRecognizer();
		moveRightRecognizer.addSwipeDirection(TKSwipeDirection.Right);
		moveRightRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Move Right gesture recognizer fired: " + r);
			if (crew.Lane < GameSettings.Lane.RIGHT)
			{
				crew.Lane += 1;
			}
		};
		TouchKit.addGestureRecognizer(moveRightRecognizer);

		var lightningRecognizer = new TKMultiDirectionalSwipeRecognizer();
		lightningRecognizer.addSwipeDirection(TKSwipeDirection.DownLeft);
		lightningRecognizer.addSwipeDirection(TKSwipeDirection.Right);
		lightningRecognizer.addSwipeDirection(TKSwipeDirection.DownLeft);
		lightningRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Lightning gesture recognizer fired: " + r);
			ObstacleSpawner.m_instance.DestroyObstacle(crew.Lane, ObstacleSpawner.ObstacleType.LIGHTNING);
		};
		TouchKit.addGestureRecognizer(lightningRecognizer);

		var rainRecognizer = new TKMultiDirectionalSwipeRecognizer();
		rainRecognizer.addSwipeDirection(TKSwipeDirection.DownLeft);
		rainRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Rain gesture recognizer fired: " + r);
			ObstacleSpawner.m_instance.DestroyObstacle(crew.Lane, ObstacleSpawner.ObstacleType.RAIN);
		};
		TouchKit.addGestureRecognizer(rainRecognizer);

		var earthQuakeRecognizer = new TKMultiDirectionalSwipeRecognizer();
		earthQuakeRecognizer.addSwipeDirection(TKSwipeDirection.UpRight);
		earthQuakeRecognizer.addSwipeDirection(TKSwipeDirection.DownRight);
		earthQuakeRecognizer.addSwipeDirection(TKSwipeDirection.UpRight);
		earthQuakeRecognizer.addSwipeDirection(TKSwipeDirection.DownRight);
		earthQuakeRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Earthquake gesture recognizer fired: " + r);
			ObstacleSpawner.m_instance.DestroyObstacle(crew.Lane, ObstacleSpawner.ObstacleType.EARTHQUAKE);
		};
		TouchKit.addGestureRecognizer(earthQuakeRecognizer);
		
		var stormRecognizer = new TKDiscreteCurveRecognizer();
		stormRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Storm gesture recognizer fired: " + r);
			ObstacleSpawner.m_instance.DestroyObstacle(crew.Lane, ObstacleSpawner.ObstacleType.STORM);
		};
		TouchKit.addGestureRecognizer(stormRecognizer);

		var volcanoRecognizer = new TKMultiDirectionalSwipeRecognizer();
		volcanoRecognizer.addSwipeDirection(TKSwipeDirection.UpRight);
		volcanoRecognizer.addSwipeDirection(TKSwipeDirection.DownRight);
		volcanoRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Volcano gesture recognizer fired: " + r);
			ObstacleSpawner.m_instance.DestroyObstacle(crew.Lane, ObstacleSpawner.ObstacleType.VOLCANO);
		};
		TouchKit.addGestureRecognizer(volcanoRecognizer);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (healthPoint <= 0)
		{
			IsGameOver = true;
		}
	}
}
