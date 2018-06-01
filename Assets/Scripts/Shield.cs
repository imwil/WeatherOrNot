using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
	public static Shield instance = null;

	private Collider2D trigger;
	private SpriteRenderer sprite;

	private void Awake()
	{
		instance = this;
		//this.gameObject.SetActive(false);
		trigger = GetComponent<Collider2D>();
		sprite = GetComponent<SpriteRenderer>();
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
		//this.gameObject.SetActive(isEnable);
		trigger.enabled = isEnable;
		sprite.enabled = isEnable;
	}
}
