using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
	private Rigidbody2D m_rigidBody2D;

	// Use this for initialization
	void Start ()
	{
		m_rigidBody2D = GetComponent<Rigidbody2D>();
		m_rigidBody2D.velocity = Vector2.down * GameSettings.m_instance.backgroundSpeed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameSettings.m_instance.IsGameOver)
		{
			m_rigidBody2D.velocity = Vector2.zero;
		}

		GameSettings.m_instance.UnsavedScore = GameSettings.m_instance.backgroundSpeed * Time.deltaTime * 10;
		GameSettings.m_instance.Score += GameSettings.m_instance.backgroundSpeed * Time.deltaTime * 5;
	}
}
