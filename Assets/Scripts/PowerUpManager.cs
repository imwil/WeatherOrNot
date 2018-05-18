using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
	public static PowerUpManager instance;

	public enum Type
	{
		SHIELD,
		SCORE_MULTIPLY,
		TIME_SLOW,
		COUNT
	}

	public float duration;
	private PowerUp[] m_powerUps;

	private void Awake()
	{
		instance = this;
		m_powerUps = new PowerUp[(int)Type.COUNT];
		for (int i = 0; i < m_powerUps.Length; i++)
		{
			switch ((Type)i)
			{
				case Type.SHIELD:
					m_powerUps[i] = new ShieldPowerUp();
					break;
				case Type.SCORE_MULTIPLY:
					m_powerUps[i] = new ScoreMultiplierPowerUp();
					break;
				case Type.TIME_SLOW:
					m_powerUps[i] = new TimeSlowPowerUp();
					break;
			}
		}
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Type type = 0;
		bool isKeyPressed = false;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			type = Type.SHIELD;
			isKeyPressed = true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			type = Type.SCORE_MULTIPLY;
			isKeyPressed = true;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			type = Type.TIME_SLOW;
			isKeyPressed = true;
		}
		if (isKeyPressed)
		{
			if (!m_powerUps[(int)type].IsActive)
			{
				Activate(type);
			}
			else
			{
				Deactivate(type);
			}
		}

		for (int i = 0; i < m_powerUps.Length; i++)
		{
			m_powerUps[i].Update();
		}
	}

	public void Activate(Type type, bool isActive = true)
	{
		if (type >= Type.SHIELD && type < Type.COUNT)
		{
			m_powerUps[(int)type].Activate(isActive);
		}
	}

	public void Deactivate(Type type)
	{
		Activate(type, false);
	}
}
