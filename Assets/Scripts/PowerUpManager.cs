using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
	public static PowerUpManager instance;

	public enum Type
	{
		SHIELD,
		DOUBLE_SCORE,
		SLOW,
		COUNT
	}

	public float duration;

	private float[] remainingTime;
	public bool this[int i]
	{
		get
		{
			return remainingTime[i] > 0f;
		}
	}

	private void Awake()
	{
		instance = this;
		remainingTime = new float[(int)Type.COUNT];
		Activate(Type.DOUBLE_SCORE);
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < remainingTime.Length; i++)
		{
			if (remainingTime[i] <= 0f)
			{
				remainingTime[i] = 0f;
			}
		}
	}

	public void Activate(Type type)
	{
		if (type >= Type.SHIELD && type < Type.COUNT)
		{
			remainingTime[(int)type] = duration;
		}
	}
}
