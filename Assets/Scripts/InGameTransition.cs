using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTransition : MonoBehaviour
{
	public Transform transformShip;
	public Transform transformBackground;

	[SerializeField] private Vector2 inGameShipPosition;
	[SerializeField] private Vector2 inGameShipScale;
	[SerializeField] private float transitionTime;
	[SerializeField] private CanvasGroup cgInGame;
	[SerializeField] private CanvasGroup cgMainMenu;

	private Vector2 targetBackgroundScale;
	private float shipPositionSpeed, shipScaleSpeed, backgroundScaleSpeed;

	private void Awake()
	{
		targetBackgroundScale.x = transformBackground.localScale.x * inGameShipScale.x / transformShip.localScale.x;
		targetBackgroundScale.y = transformBackground.localScale.y * inGameShipScale.y / transformShip.localScale.y;

		shipPositionSpeed = Vector2.Distance(transformShip.position, inGameShipPosition) / transitionTime;
		shipScaleSpeed = Vector2.Distance(transformShip.localScale, inGameShipScale) / transitionTime;
		backgroundScaleSpeed = Vector2.Distance(transformBackground.localScale, targetBackgroundScale) / transitionTime;
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameSettings.instance && GameSettings.instance.State == GameSettings.GameState.IN_GAME_TRANSITION)
		{
			transformShip.position = Vector2.MoveTowards(transformShip.position, inGameShipPosition, shipPositionSpeed * Time.deltaTime);
			transformShip.localScale = Vector2.MoveTowards(transformShip.localScale, inGameShipScale, shipScaleSpeed * Time.deltaTime);
			transformBackground.localScale = Vector2.MoveTowards(transformBackground.localScale, targetBackgroundScale, backgroundScaleSpeed * Time.deltaTime);

			if ((Vector2)transformShip.position == inGameShipPosition)
			{
				GameSettings.instance.State = GameSettings.GameState.IN_GAME;
			}

			cgInGame.alpha = Mathf.MoveTowards(cgInGame.alpha, 1.0f, Time.deltaTime / transitionTime);
			cgMainMenu.alpha = Mathf.MoveTowards(cgMainMenu.alpha, 0.0f, Time.deltaTime / transitionTime);
			cgMainMenu.interactable = false;
			cgMainMenu.blocksRaycasts = false;
			if (cgInGame.alpha == 1.0f)
			{
				cgInGame.interactable = true;
				cgInGame.blocksRaycasts = true;
			}
		}
	}
}
