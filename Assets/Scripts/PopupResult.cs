using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupResult : MonoBehaviour
{
	public Text content;
	public Text record;

	public void SetScore(float score)
	{
		content.text = string.Format(content.text, score / 1000);
		record.text = string.Format(record.text, ScoreManager.instance.HighScore);
	}
}
