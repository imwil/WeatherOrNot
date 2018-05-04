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
	public SpriteRenderer[] rects;
	
	public enum Lane
	{
		LEFT, MIDDLE, RIGHT, COUNT
	};
	public bool IsGameOver { get; private set; }

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
			int tmp = (int)m_score;
			labelScore.text = (tmp - tmp % 10).ToString();
		}
	}
	public float UnsavedScore { get; set; }

	private void Awake()
	{
		m_instance = this;
		Score = 0;
	} 

	// Use this for initialization
	void Start ()
	{
		Debug.Log(rects[0].transform.position + " " + rects[0].bounds.size);
		Debug.Log("(" + rects[0].size.x * rects[0].transform.localScale.x + ", " + rects[0].size.y * rects[0].transform.localScale.y + ")");
		var tapLeftRecognizer = new TKTapRecognizer();
		tapLeftRecognizer.boundaryFrame = new TKRect(rects[0].transform.position.x, rects[0].transform.position.y, rects[0].bounds.size.x, rects[0].bounds.size.y);
		tapLeftRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("tap recognizer fired: " + r);
			if (crew.Lane > GameSettings.Lane.LEFT)
			{
				crew.Lane -= 1;
			}
		};
		TouchKit.addGestureRecognizer(tapLeftRecognizer);

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
