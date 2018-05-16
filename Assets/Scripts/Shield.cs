using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
	public static Shield instance = null;

	private Collider2D trigger;

	private void Awake()
	{
		instance = this;
		trigger = GetComponent<Collider2D>();
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Enable(bool isEnable)
	{
		trigger.enabled = isEnable;
	}
}
