using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
	public Text playerID;
	public Text textUI;
	public Text multiplierTextUI;


	private int currentScore = 0;
	private int currentMultiplier = 0;

	public Gradient comboGradient;

	public void Start()
	{
		gameObject.SetActive(false);
	}

	public void UpdateScore(int newScore, int newMultiplier)
	{
		if (newScore > 0)
		{
			gameObject.SetActive(true);

		}

		if (newScore != currentScore)
		{

			LeanTween.value(gameObject, textUI.color, Color.white, 0.1f).setOnUpdate((Action<Color>)(newColor =>
				{
					textUI.color = newColor;
				})).setOnComplete(() =>
				{
					LeanTween.value(gameObject, textUI.color, new Color(1, 1, 1, 0.5f), 0.5f).setOnUpdate((Action<Color>) (newColor =>
					{
						textUI.color = newColor;
					}));
				});

			
			textUI.text = (newScore).ToString();


			currentScore = newScore;
		}
		if (newMultiplier != currentMultiplier)
		{
			LeanTween.value(gameObject, multiplierTextUI.color, Color.white, 0.1f).setOnUpdate((Action<Color>)(newColor =>
			{
				multiplierTextUI.color = newColor;
			})).setOnComplete(() =>
			{
				LeanTween.value(gameObject, multiplierTextUI.color, comboGradient.Evaluate((newMultiplier -1f)/ 3f), 0.5f).setOnUpdate((Action<Color>)(newColor =>
				{
					multiplierTextUI.color = newColor;
				}));
			});

			multiplierTextUI.text = newMultiplier.ToString() + "x";

			currentMultiplier = newMultiplier;
		}




	}
}
