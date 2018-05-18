using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public ObstacleSpawner.ObstacleType Type { get; set; }
	public GameSettings.Lane Lane { get; set; }

	private Rigidbody2D m_rigidBody2D;

	// Use this for initialization
	void Start ()
	{
		Lane = GameSettings.Lane.MIDDLE;
		m_rigidBody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		m_rigidBody2D.velocity = Vector2.down * SpeedManager.instance.ObstacleSpeed;

		if (GameSettings.instance.IsGameOver)
		{
			m_rigidBody2D.velocity = Vector2.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Collider2D>() != null)
		{
			if (collision.name == Ship.instance.name)
			{
				GameSettings.instance.healthPoint -= 1;
				Debug.Log("healthPoint = " + GameSettings.instance.healthPoint);
			}
			else if (collision.name == Shield.instance.name)
			{

			}
			Destroy(this.gameObject);
			ObstacleManager.instance.spawners[(int)Lane].RemoveObstacle(this.Type, this);
		}
	}
}
