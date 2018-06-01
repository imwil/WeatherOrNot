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
			target = new Vector3(markers[(int)value].position.x, transform.position.y, transform.position.z);
		}
	}

	private Vector3 target;

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
		transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
	}
}
