using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : PowerUp
{
	override public void Activate(bool isActive = true)
	{
		base.Activate(isActive);
		Shield.instance.Enable(isActive);
	}
}
