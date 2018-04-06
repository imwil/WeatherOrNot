using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	public static GameSettings m_instance = null;

	public float backgroundSpeed;
	public float obstacleSpeed;

	private bool m_isGameOver;
	public bool IsGameOver { get { return m_isGameOver; } }

	public float obstacleSpawnInterval;

	// Use this for initialization
	void Start ()
	{
		m_instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
