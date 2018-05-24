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
		VOLCANO
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
		if (!GameSettings.instance.IsGameOver)
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

	public void DestroyObstacle(ObstacleType type)
	{
		Debug.LogFormat("{0}, {1}, {2}, {3}, {4}", m_obstacles[(int)ObstacleType.LIGHTNING].Count, m_obstacles[(int)ObstacleType.RAIN].Count, m_obstacles[(int)ObstacleType.EARTHQUAKE].Count, m_obstacles[(int)ObstacleType.STORM].Count, m_obstacles[(int)ObstacleType.VOLCANO].Count);
		if (m_obstacles[(int)type].Count > 0)
		{
			if (m_obstacles[(int)type].FirstOrDefault() != null)
			{
				Destroy(m_obstacles[(int)type].FirstOrDefault().gameObject);
			}
			RemoveObstacle(type, m_obstacles[(int)type].FirstOrDefault());
		}
	}

	public void RemoveObstacle(ObstacleType type, Obstacle obstacle)
	{
		m_obstacles[(int)type].Remove(obstacle);
	}
}
