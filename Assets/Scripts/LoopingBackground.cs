using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
	private SpriteRenderer m_background;
	private float m_bgVerticalLength;

	// Use this for initialization
	void Start ()
	{
		m_background = GetComponent<SpriteRenderer>();
		m_bgVerticalLength = m_background.bounds.size.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.position.y < -m_bgVerticalLength)
		{
			Reposition();
		}
	}

	private void Reposition()
	{
		Vector2 bgOffset = new Vector2(0f, m_bgVerticalLength * 2f);
		transform.position = (Vector2)transform.position + bgOffset;
	}
}
