using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public class OstacleSpawner : MonoBehaviour
{
	public enum ObstacleType
	{
		LIGHTNING = 0,
		RAIN,
		EARTHQUAKE,
		STORM,
		VOLCANO
	};
	public static OstacleSpawner m_instance = null;

	public Obstacle[] m_obstaclePrefabs;
	public int[] m_obstacleAmountChance;
	public GameObject[] m_obstacleHolders;

	private float m_lastSpawnTime;
	private bool[] m_isSpawningLane = new bool[3] { false, false, false };
	private List<Obstacle>[] m_obstacles;

	private void Awake()
	{
		m_instance = this;

		int totalChance = 0;
		for (int i = 0; i < m_obstacleAmountChance.Length; i++)
		{
			totalChance += m_obstacleAmountChance[i];
		}
		Assert.AreEqual(100, totalChance, "ERROR: Total obstacle amount chance must be equal to 100!");

		m_obstacles = new List<Obstacle>[m_obstaclePrefabs.Length];
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
			if (m_lastSpawnTime >= GameSettings.m_instance.obstacleSpawnInterval)
			{
				m_lastSpawnTime -= GameSettings.m_instance.obstacleSpawnInterval;
				SpawnObstacle();
			}
		}
	}

	private void SpawnObstacle()
	{
		int random = Random.Range(0, 100);
		int obstacleAmount = 0;
		do
		{
			random -= m_obstacleAmountChance[obstacleAmount];
			obstacleAmount++;
		}
		while (random >= 0);
		
		for (int i = 0; i < obstacleAmount; i++)
		{
			int index = -1;
			do
			{
				index = Random.Range(0, 3);
			}
			while (m_isSpawningLane[index]);

			m_isSpawningLane[index] = true;
		}

		for (int i = 0; i < m_isSpawningLane.Length; i++)
		{
			if (m_isSpawningLane[i])
			{
				int idx = Random.Range(0, m_obstaclePrefabs.Length);
				Obstacle prefab = m_obstaclePrefabs[idx];
				Obstacle obstacle = Instantiate<Obstacle>(prefab, m_obstacleHolders[i].transform);
				obstacle.Type = (ObstacleType)idx;
				m_obstacles[idx].Add(obstacle);
				m_isSpawningLane[i] = false;
			}
		}
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
