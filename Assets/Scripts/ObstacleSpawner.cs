using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public class ObstacleSpawner : MonoBehaviour
{
	private const float DEFAULT_TIME_OFFSET = 0f;

	public enum ObstacleType
	{
		LIGHTNING = 0,
		RAIN,
		EARTHQUAKE,
		STORM,
		VOLCANO,
		COUNT
	};

	public Obstacle[] obstaclePrefabs;

	public GameSettings.Lane Lane { get; set; }

	private float m_lastSpawnTime;
	private float m_spawnTimeOffset;
	private List<Obstacle>[] m_obstacles;

	private void Awake()
	{
		Lane = GameSettings.Lane.MIDDLE;
		m_lastSpawnTime = 0f;
		m_spawnTimeOffset = DEFAULT_TIME_OFFSET;

		m_obstacles = new List<Obstacle>[obstaclePrefabs.Length];
		for (int i = 0; i < m_obstacles.Length; i++)
		{
			m_obstacles[i] = new List<Obstacle>();
		}
	}

	// Use this for initialization
	void Start ()
	{
		
	}

	private void FixedUpdate()
	{
		if (GameSettings.instance.State == GameSettings.GameState.IN_GAME)
		{
			m_lastSpawnTime += Time.deltaTime;
			if (m_spawnTimeOffset == DEFAULT_TIME_OFFSET)
			{
				m_spawnTimeOffset = Random.Range(0f, 2 * SpeedManager.instance.ObstacleSpawnInterval);
			}
			if (m_lastSpawnTime >= SpeedManager.instance.ObstacleSpawnInterval + m_spawnTimeOffset)
			{
				m_lastSpawnTime -= (SpeedManager.instance.ObstacleSpawnInterval + m_spawnTimeOffset);
				m_spawnTimeOffset = DEFAULT_TIME_OFFSET;
				SpawnObstacle();
			}
		}
	}

	private void SpawnObstacle()
	{
		int idx = Random.Range(0, obstaclePrefabs.Length);
		Obstacle obstacle = Instantiate<Obstacle>(obstaclePrefabs[idx], this.transform);
		obstacle.Type = (ObstacleType)idx;
		obstacle.Lane = Lane;
		m_obstacles[idx].Add(obstacle);
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	public void DestroyObstacle(Obstacle obstacle, bool isKill = true)
	{
		Debug.LogFormat("{0}, {1}, {2}, {3}, {4}", m_obstacles[(int)ObstacleType.LIGHTNING].Count, m_obstacles[(int)ObstacleType.RAIN].Count, m_obstacles[(int)ObstacleType.EARTHQUAKE].Count, m_obstacles[(int)ObstacleType.STORM].Count, m_obstacles[(int)ObstacleType.VOLCANO].Count);
		if (obstacle != null && m_obstacles[(int)obstacle.Type].Count > 0)
		{
			Destroy(obstacle.gameObject);
			if (isKill && GameSettings.instance.KilledObstaclesCount < GameSettings.instance.maxKilledObstacles)
			{
				GameSettings.instance.KilledObstaclesCount++;
			}
			m_obstacles[(int)obstacle.Type].Remove(obstacle);
		}
	}

	public void DestroyObstacle(ObstacleType type)
	{
		if (m_obstacles[(int)type].Count > 0)
		{
			DestroyObstacle(m_obstacles[(int)type].FirstOrDefault());
		}
	}

	public void DestroyAll()
	{
		for (int i = 0; i < m_obstacles.Length; i++)
		{
			m_obstacles[i].ForEach((Obstacle obstacle) => {
				Destroy(obstacle.gameObject);
				if (GameSettings.instance.KilledObstaclesCount < GameSettings.instance.maxKilledObstacles)
				{
					GameSettings.instance.KilledObstaclesCount++;
				}
			});
			m_obstacles[i].Clear();
		}
	}
}
