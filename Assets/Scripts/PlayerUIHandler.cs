﻿using UnityEngine;
using System.Collections;

public class PlayerUIHandler : MonoBehaviour
{
	public GameObject[] holder = new GameObject[3];
	public GameObject prefab;
	private PlayerUI[] uiHolder = new PlayerUI[4];
	private int playerCount = 4;

	void Start ()
	{
		for (int i = 0; i < playerCount; i++)
		{
			GameObject nextplayer = holder[i].transform.GetChild(0).gameObject;
			nextplayer.transform.SetParent(holder[i].transform);
			nextplayer.transform.localPosition = Vector3.zero;
			nextplayer.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

			nextplayer.transform.localScale = Vector3.one;

			uiHolder[i] = holder[i].transform.GetChild(0).GetComponent<PlayerUI>();
		}
	}

	public void SetPlayerUIVisible(bool isVisible)
	{
		int numberOfPlayersActive = 0;

		foreach (var birdController in GameObject.FindObjectsOfType<BirdControl>())
		{
			if (birdController.IsInitialized())
			{
				numberOfPlayersActive = Mathf.Max(numberOfPlayersActive, birdController.playerId);
			}
		}

		if (isVisible == false)
		{
			numberOfPlayersActive = playerCount;
		}

		for (var i = 0; i < numberOfPlayersActive; i++)
		{
			uiHolder[i].gameObject.SetActive(isVisible);
		}
	}

	void Update ()
	{
		UpdateText();
	}

	private void UpdateText()
	{
		for (var i = 0; i < playerCount; i++)
		{
			int score = ScoreHandler.GetInstance().GetScore(i);
			int multiplier = ScoreHandler.GetInstance().GetComboCount(i);
			uiHolder[i].UpdateScore(score, multiplier);
		}
	}
}
