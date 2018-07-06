using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupHighscore : MonoBehaviour
{
	public Text content;

	public void SetScore(float score)
	{
		content.text = string.Format(content.text, score / 1000);
	}
}
