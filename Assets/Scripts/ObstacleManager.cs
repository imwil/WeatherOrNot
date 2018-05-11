using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
	public GameObject[] m_holders;
	public ObstacleSpawner spawnerPrefab;

	public static ObstacleManager m_instance;

	[HideInInspector] public ObstacleSpawner[] spawners;

	private void Awake()
	{
		m_instance = this;

		spawners = new ObstacleSpawner[(int)GameSettings.Lane.COUNT];
		for (int i = 0; i < (int)GameSettings.Lane.COUNT; i++)
		{
			spawners[i] = Instantiate<ObstacleSpawner>(spawnerPrefab, m_holders[i].transform.position, Quaternion.identity, this.transform);
			spawners[i].Lane = (GameSettings.Lane)i;
		}
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
