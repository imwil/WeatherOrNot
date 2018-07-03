using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
	public static ObstacleManager instance;

	public GameObject[] m_holders;
	public ObstacleSpawner spawnerPrefab;

	[HideInInspector] public ObstacleSpawner[] spawners;

	private void Awake()
	{
		instance = this;

		spawners = new ObstacleSpawner[(int)GameSettings.Lane.COUNT];
		for (int i = 0; i < (int)GameSettings.Lane.COUNT; i++)
		{
			spawners[i] = Instantiate<ObstacleSpawner>(spawnerPrefab, m_holders[i].transform.position, Quaternion.identity, this.transform);
			spawners[i].Lane = (GameSettings.Lane)i;
		}
	}

	public void DestroyAll()
	{
		for (int i = 0; i < (int)GameSettings.Lane.COUNT; i++)
		{
			spawners[i].DestroyAll();
		}
	}
}
