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
		m_rigidBody2D.velocity = new Vector2(0f, -2f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
