using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
	public static SpeedManager instance;

	public float baseBackgroundSpeed;
	public float baseObstacleSpeed;
	public float baseObstacleSpawnInterval;
	public float multiplier;

	public float BackgroundSpeed { get; set; }
	public float ObstacleSpeed { get; set; }
	public float ObstacleSpawnInterval { get; set; }

	private void Awake ()
	{
		instance = this;
		ApplyMultiplier(false);
	}

	public void ApplyMultiplier(bool isApply)
	{
		if (isApply)
		{
			BackgroundSpeed = baseBackgroundSpeed * multiplier;
			ObstacleSpeed = baseObstacleSpeed * multiplier;
			ObstacleSpawnInterval = baseObstacleSpawnInterval / multiplier;
		}
		else
		{
			BackgroundSpeed = baseBackgroundSpeed;
			ObstacleSpeed = baseObstacleSpeed;
			ObstacleSpawnInterval = baseObstacleSpawnInterval;
		}
	}
}
