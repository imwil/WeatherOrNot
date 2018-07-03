using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
	public static GameSettings instance = null;

	public int healthPoint;
	public Crew crew;
	public int maxKilledObstacles;

	public enum Lane
	{
		LEFT, MIDDLE, RIGHT, COUNT
	};
	public bool IsGameOver { get; private set; }
	public bool IsGamePause { get; set; }
	public int KilledObstaclesCount { get; set; }

	private float tapZoneWidth;
	private float tapZoneHeight;

	private void Awake()
	{
		instance = this;
		IsGameOver = false;
		IsGamePause = false;
		KilledObstaclesCount = 0;

		tapZoneWidth = 106.65f;	// hardcode this, do not change the value if there is no problem with moving crew when tapping
		tapZoneHeight = 180f;   // hardcode this, do not change the value if there is no problem with moving crew when tapping
	}

	// Use this for initialization
	void Start ()
	{
		var leftTapRecognizer = new TKTapRecognizer();
		leftTapRecognizer.boundaryFrame = new TKRect(0f, 0f, tapZoneWidth, tapZoneHeight);
		leftTapRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Left Zone Tap recognizer fired: " + r);
			crew.Lane = GameSettings.Lane.LEFT;
		};
		TouchKit.addGestureRecognizer(leftTapRecognizer);

		var middleTapRecognizer = new TKTapRecognizer();
		middleTapRecognizer.boundaryFrame = new TKRect(tapZoneWidth, 0f, tapZoneWidth, tapZoneHeight);
		middleTapRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Middle Zone Tap recognizer fired: " + r);
			crew.Lane = GameSettings.Lane.MIDDLE;
		};
		TouchKit.addGestureRecognizer(middleTapRecognizer);

		var rightTapRecognizer = new TKTapRecognizer();
		rightTapRecognizer.boundaryFrame = new TKRect(2 * tapZoneWidth, 0f, tapZoneWidth, tapZoneHeight);
		rightTapRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Right Zone Tap recognizer fired: " + r);
			crew.Lane = GameSettings.Lane.RIGHT;
		};
		TouchKit.addGestureRecognizer(rightTapRecognizer);

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

		var stormRecognizer = new TKDiscreteCurveRecognizer();
		stormRecognizer.SetAngles(-540f, 270f);
		stormRecognizer.gestureRecognizedEvent += (r) =>
		{
			Debug.Log("Storm gesture recognizer fired: " + r);
			ObstacleManager.instance.spawners[(int)crew.Lane].DestroyObstacle(ObstacleSpawner.ObstacleType.STORM);
		};
		TouchKit.addGestureRecognizer(stormRecognizer);

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
