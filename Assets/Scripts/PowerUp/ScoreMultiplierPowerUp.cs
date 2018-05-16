using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMultiplierPowerUp : PowerUp
{
	override public void Activate(bool isActive = true)
	{
		base.Activate(isActive);
		ScoreManager.instance.ApplyMultiplier(isActive);
	}
}
