using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp
{
	private const float INACTIVE_TIME = 0f;

	public bool IsActive
	{
		get
		{
			return remainingTime > INACTIVE_TIME;
		}
	}

	private float remainingTime;

	private void Awake()
	{
		Activate(false);
	}

	// Use this for initialization
	virtual public void Start ()
	{
		
	}

	// Update is called once per frame
	virtual public void Update ()
	{
		remainingTime -= Time.deltaTime;
		if (remainingTime <= INACTIVE_TIME)
		{
			Deactivate();
		}
	}

	virtual public void Activate(bool isActive = true)
	{
		if (isActive)
		{
			remainingTime = PowerUpManager.instance.duration;
		}
		else
		{
			remainingTime = INACTIVE_TIME;
		}
	}

	public void Deactivate()
	{
		Activate(false);
	}
}
