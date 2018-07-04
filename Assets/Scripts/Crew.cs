using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crew : MonoBehaviour
{
	public float speed;
	public Transform[] markers;

	private GameSettings.Lane m_lane;
	public GameSettings.Lane Lane
	{
		get
		{
			return m_lane;
		}
		set
		{
			m_lane = value;
			target = new Vector2(markers[(int)value].position.x, transform.position.y);
		}
	}

	private Vector2 target;

	private void Awake()
	{
		Lane = GameSettings.Lane.MIDDLE;
		target = transform.position;
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		//if (GameSettings.instance && GameSettings.instance.State == GameSettings.GameState.IN_GAME)
		{
			if (target.y != transform.position.y)
			{
				target.y = transform.position.y;
			}
			transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
		}
	}
}
