using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public ObstacleSpawner.ObstacleType Type { get; set; }
	public GameSettings.Lane Lane { get; set; }

	private Rigidbody2D m_rigidBody2D;

	private void Awake()
	{
		Lane = GameSettings.Lane.MIDDLE;
		m_rigidBody2D = GetComponent<Rigidbody2D>();
	}

	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		m_rigidBody2D.velocity = Vector2.down * SpeedManager.instance.ObstacleSpeed;

		if (GameSettings.instance.State != GameSettings.GameState.IN_GAME)
		{
			m_rigidBody2D.velocity = Vector2.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Collider2D>() != null)
		{
			bool isKill = false;
			if (collision.CompareTag("Ship"))
			{
				GameSettings.instance.HealthPoint -= 1;
				Debug.Log("healthPoint = " + GameSettings.instance.HealthPoint);
				isKill = false;
			}
			else if (collision.CompareTag("Shield"))
			{
				isKill = true;
			}
			ObstacleManager.instance.spawners[(int)Lane].DestroyObstacle(this, isKill);
		}
	}
}
