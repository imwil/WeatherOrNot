using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstacleSpawner : MonoBehaviour
{
	//public float m_spawnInterval;
	public Obstacle[] m_obstaclePrefabs;

	private float m_lastSpawnTime;

	// Use this for initialization
	void Start ()
	{
		
	}

	private void FixedUpdate()
	{
		m_lastSpawnTime += Time.deltaTime;
		if (m_lastSpawnTime >= GameSettings.m_instance.obstacleSpawnInterval)
		{
			m_lastSpawnTime -= GameSettings.m_instance.obstacleSpawnInterval;
			SpawnObstacle();
		}
	}

	private void SpawnObstacle()
	{
		if (Random.Range(0, 2) == 1)
		{
			Obstacle prefab = m_obstaclePrefabs[Random.Range(0, m_obstaclePrefabs.Length)];
			Instantiate<Obstacle>(prefab, transform);
			//obstacle.transform.localPosition = this.transform.localPosition;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
