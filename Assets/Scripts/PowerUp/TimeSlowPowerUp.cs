using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowPowerUp : PowerUp
{
	override public void Activate(bool isActive = true)
	{
		base.Activate(isActive);
		SpeedManager.instance.ApplyMultiplier(isActive);
	}
}
