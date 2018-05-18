using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
	public static GameSettings instance = null;

	public int healthPoint;
	public Crew crew;
	public SpriteRenderer[] rects;

	public enum Lane
	{
		LEFT, MIDDLE, RIGHT, COUNT
	};
	public bool IsGameOver { get; private set; }

	private void Awake()
	{
		instance = this;
		IsGameOver = false;
	}

	// Use this for initialization
	void Start ()
	{
		//var moveLeftRecognizer = new TKMultiDirectionalSwipeRecognizer();
		//moveLeftRecognizer.addSwipeDirection(TKSwipeDirection.Left);
		//moveLeftRecognizer.gestureRecognizedEvent += (r) =>
		//{
		//	Debug.Log("Move Left gesture recognizer fired: " + r);
		//	if (crew.Lane > GameSettings.Lane.LEFT)
		//	{
		//		crew.Lane -= 1;
		//	}
		//};
		//TouchKit.addGestureRecognizer(moveLeftRecognizer);

		//var moveRightRecognizer = new TKMultiDirectionalSwipeRecognizer();
		//moveRightRecognizer.addSwipeDirection(TKSwipeDirection.Right);
		//moveRightRecognizer.gestureRecognizedEvent += (r) =>
		//{
		//	Debug.Log("Move Right gesture recognizer fired: " + r);
		//	if (crew.Lane < GameSettings.Lane.RIGHT)
		//	{
		//		crew.Lane += 1;
		//	}
		//};
		//TouchKit.addGestureRecognizer(moveRightRecognizer);

		var lightningRecognizer = new TKMultiDirectionalSwipeRecognizer();
		lightningRecognizer.AddSwipe(120);
		lightningRecognizer.AddSwipe(350);
		lightningRecognizer.AddSwipe(120);
		lightningRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Lightning gesture recognizer fired: " + r);
			ObstacleManager.instance.spawners[(int)crew.Lane].DestroyObstacle(ObstacleSpawner.ObstacleType.LIGHTNING);
		};
		TouchKit.addGestureRecognizer(lightningRecognizer);

		var rainRecognizer = new TKMultiDirectionalSwipeRecognizer();
		rainRecognizer.AddSwipe(135);
		rainRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Rain gesture recognizer fired: " + r);
			ObstacleManager.instance.spawners[(int)crew.Lane].DestroyObstacle(ObstacleSpawner.ObstacleType.RAIN);
		};
		TouchKit.addGestureRecognizer(rainRecognizer);

		//var earthQuakeRecognizer = new TKMultiDirectionalSwipeRecognizer();
		//earthQuakeRecognizer.addSwipeDirection(TKSwipeDirection.UpRight);
		//earthQuakeRecognizer.addSwipeDirection(TKSwipeDirection.DownRight);
		//earthQuakeRecognizer.addSwipeDirection(TKSwipeDirection.UpRight);
		//earthQuakeRecognizer.addSwipeDirection(TKSwipeDirection.DownRight);
		//earthQuakeRecognizer.gestureRecognizedEvent += (r) =>
		//{
		//	Debug.Log("Earthquake gesture recognizer fired: " + r);
		//	ObstacleManager.instance.spawners[(int)crew.Lane].DestroyObstacle(ObstacleSpawner.ObstacleType.EARTHQUAKE);
		//};
		//TouchKit.addGestureRecognizer(earthQuakeRecognizer);

		//var stormRecognizer = new TKDiscreteCurveRecognizer();
		//stormRecognizer.gestureRecognizedEvent += (r) =>
		//{
		//	Debug.Log("Storm gesture recognizer fired: " + r);
		//	ObstacleManager.instance.spawners[(int)crew.Lane].DestroyObstacle(ObstacleSpawner.ObstacleType.STORM);
		//};
		//TouchKit.addGestureRecognizer(stormRecognizer);

		var volcanoRecognizer = new TKMultiDirectionalSwipeRecognizer();
		volcanoRecognizer.AddSwipe(315);
		volcanoRecognizer.AddSwipe(45);
		volcanoRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Volcano gesture recognizer fired: " + r);
			ObstacleManager.instance.spawners[(int)crew.Lane].DestroyObstacle(ObstacleSpawner.ObstacleType.VOLCANO);
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
