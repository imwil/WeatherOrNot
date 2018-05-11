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
		m_rigidBody2D.velocity = Vector2.down * GameSettings.instance.backgroundSpeed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameSettings.instance.IsGameOver)
		{
			m_rigidBody2D.velocity = Vector2.zero;
		}
	}
}
