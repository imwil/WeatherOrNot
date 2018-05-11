using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public class ObstacleSpawner : MonoBehaviour
{
	private const float defaultTimeOffset = 0f;
	public enum ObstacleType
	{
		LIGHTNING = 0,
		RAIN,
		EARTHQUAKE,
		STORM,
		VOLCANO
	};

	public Obstacle[] obstaclePrefabs;
	public float spawnInterval;

	public GameSettings.Lane Lane { get; set; }

	private float m_lastSpawnTime;
	private float m_spawnTimeOffset;
	//private bool[] m_isSpawningLane = new bool[3] { false, false, false };
	private List<Obstacle>[] m_obstacles;

	private void Awake()
	{
		m_spawnTimeOffset = defaultTimeOffset;

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
		if (!GameSettings.m_instance.IsGameOver)
		{
			m_lastSpawnTime += Time.deltaTime;
			if (m_spawnTimeOffset == defaultTimeOffset)
			{
				m_spawnTimeOffset = Random.Range(0f, 2 * spawnInterval);
			}
			if (m_lastSpawnTime >= spawnInterval + m_spawnTimeOffset)
			{
				m_lastSpawnTime -= (spawnInterval + m_spawnTimeOffset);
				m_spawnTimeOffset = defaultTimeOffset;
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
		if (m_obstacles[(int)type].Count > 0)
		{
			Destroy(m_obstacles[(int)type].FirstOrDefault().gameObject);
			RemoveObstacle(type, m_obstacles[(int)type].FirstOrDefault());
		}
	}

	public void RemoveObstacle(ObstacleType type, Obstacle obstacle)
	{
		m_obstacles[(int)type].Remove(obstacle);
	}
}
