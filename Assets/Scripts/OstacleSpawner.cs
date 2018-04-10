using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class OstacleSpawner : MonoBehaviour
{
	public Obstacle[] m_obstaclePrefabs;
	public int[] m_obstacleAmountChance;
	public GameObject[] m_obstacleHolders;

	private float m_lastSpawnTime;
	private bool[] m_isSpawningLane = new bool[3] { false, false, false };

	private void Awake()
	{
		int totalChance = 0;
		for (int i = 0; i < m_obstacleAmountChance.Length; i++)
		{
			totalChance += m_obstacleAmountChance[i];
		}
		Assert.AreEqual(100, totalChance, "ERROR: Total obstacle amount chance must be equal to 100!");
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
				Obstacle prefab = m_obstaclePrefabs[Random.Range(0, m_obstaclePrefabs.Length)];
				Instantiate<Obstacle>(prefab, m_obstacleHolders[i].transform);
				m_isSpawningLane[i] = false;
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
