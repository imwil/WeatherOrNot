using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public GameObject m_destroyMarker;
	private Rigidbody2D m_rigidBody2D;

	// Use this for initialization
	void Start ()
	{
		m_rigidBody2D = GetComponent<Rigidbody2D>();
		m_rigidBody2D.velocity = Vector2.up * GameSettings.m_instance.obstacleSpeed;
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
			Destroy(this.gameObject);
		}
	}
}
