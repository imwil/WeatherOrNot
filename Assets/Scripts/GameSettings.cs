using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
	public static GameSettings instance = null;

	public int maxHealthPoint;
	public Crew crew;
	public int maxKilledObstacles;

	public enum Lane
	{
		LEFT, MIDDLE, RIGHT, COUNT
	};

	public enum GameState
	{
		MAIN_MENU = 0,
		IN_GAME_TRANSITION,
		IN_GAME,
		PAUSE,
		GAME_OVER,
		COUNT
	};

	public int HealthPoint { get; set; }
	private GameState m_state;
	public GameState State
	{
		get
		{
			return m_state;
		}
		set
		{
			m_state = value;
			if (m_state == GameState.IN_GAME)
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
			else
			{
				TouchKit.removeAllGestureRecognizers();
			}
		}
	}
	public int KilledObstaclesCount { get; set; }

	private GameState prevStateBeforePause;
	private float tapZoneWidth;
	private float tapZoneHeight;

	private void Awake()
	{
		instance = this;

		HealthPoint = maxHealthPoint;
		State = GameState.MAIN_MENU;
		KilledObstaclesCount = 0;

		tapZoneWidth = 106.65f;	// hardcode this, do not change the value if there is no problem with moving crew when tapping
		tapZoneHeight = 180f;   // hardcode this, do not change the value if there is no problem with moving crew when tapping
	}

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if (HealthPoint <= 0)
		{
			State = GameState.GAME_OVER;
		}
	}

	public void Pause()
	{
		if (State == GameState.PAUSE)
		{
			State = prevStateBeforePause;
		}
		else
		{
			prevStateBeforePause = State;
			State = GameState.PAUSE;
		}
	}

	public void Revive()
	{
		HealthPoint = maxHealthPoint;
		State = GameState.IN_GAME;
	}

	public void ResetGame()
	{
		HealthPoint = maxHealthPoint;
		ObstacleManager.instance.DestroyAll();
		ScoreManager.instance.Score = 0;
		crew.Lane = Lane.MIDDLE;
		HUDManager.instance.ResetOmegaBar();
		State = GameState.IN_GAME;
	}
}
