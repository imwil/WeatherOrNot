using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	public static GameSettings m_instance = null;

	public float backgroundSpeed;
	public float obstacleSpeed;

	public int healthPoint;

	private bool m_isGameOver = false;
	public bool IsGameOver { get { return m_isGameOver; } }

	public float obstacleSpawnInterval;

	private void Awake()
	{
		m_instance = this;
	} 

	// Use this for initialization
	void Start ()
	{
		var lightningRecognizer = new TKMultiDirectionalSwipeRecognizer();
		lightningRecognizer.addSwipeDirection(TKSwipeDirection.DownLeft);
		lightningRecognizer.addSwipeDirection(TKSwipeDirection.Right);
		lightningRecognizer.addSwipeDirection(TKSwipeDirection.DownLeft);
		lightningRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Lightning gesture recognizer fired: " + r);
			ObstacleSpawner.m_instance.DestroyObstacle(ObstacleSpawner.ObstacleType.LIGHTNING);
		};
		TouchKit.addGestureRecognizer(lightningRecognizer);

		var rainRecognizer = new TKMultiDirectionalSwipeRecognizer();
		rainRecognizer.addSwipeDirection(TKSwipeDirection.DownLeft);
		rainRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Rain gesture recognizer fired: " + r);
			ObstacleSpawner.m_instance.DestroyObstacle(ObstacleSpawner.ObstacleType.RAIN);
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
			ObstacleSpawner.m_instance.DestroyObstacle(ObstacleSpawner.ObstacleType.EARTHQUAKE);
		};
		TouchKit.addGestureRecognizer(earthQuakeRecognizer);
		
		var stormRecognizer = new TKDiscreteCurveRecognizer();
		stormRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Storm gesture recognizer fired: " + r);
			ObstacleSpawner.m_instance.DestroyObstacle(ObstacleSpawner.ObstacleType.STORM);
		};
		TouchKit.addGestureRecognizer(stormRecognizer);

		var volcanoRecognizer = new TKMultiDirectionalSwipeRecognizer();
		volcanoRecognizer.addSwipeDirection(TKSwipeDirection.UpRight);
		volcanoRecognizer.addSwipeDirection(TKSwipeDirection.DownRight);
		volcanoRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Volcano gesture recognizer fired: " + r);
			ObstacleSpawner.m_instance.DestroyObstacle(ObstacleSpawner.ObstacleType.VOLCANO);
		};
		TouchKit.addGestureRecognizer(volcanoRecognizer);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (healthPoint <= 0)
		{
			m_isGameOver = true;
		}
	}
}
