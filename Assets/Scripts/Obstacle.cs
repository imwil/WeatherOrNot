using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public GameObject m_destroyMarker;
	private Rigidbody2D m_rigidBody2D;
	public ObstacleSpawner.ObstacleType Type { get; set; }
	public GameSettings.Lane Lane { get; set; }

	// Use this for initialization
	void Start ()
	{
		m_rigidBody2D = GetComponent<Rigidbody2D>();
		m_rigidBody2D.velocity = Vector2.down * GameSettings.m_instance.obstacleSpeed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameSettings.m_instance.IsGameOver)
		{
			m_rigidBody2D.velocity = Vector2.zero;
		}

		if (transform.position.y < m_destroyMarker.transform.position.y)
		{
			GameSettings.m_instance.healthPoint -= 1;
			Debug.Log("healthPoint = " + GameSettings.m_instance.healthPoint);
			Destroy(this.gameObject);
			ObstacleSpawner.m_instance.RemoveObstacle(this.Lane, this.Type, this);
		}
	}
}
