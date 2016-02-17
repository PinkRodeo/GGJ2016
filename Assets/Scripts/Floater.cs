using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


[Serializable]
public struct HitFeedbackSprites
{
	public Sprite bad;
	public Sprite good;
	public Sprite great;
	public Sprite perfect;
}

public class Floater : MonoBehaviour
{
	public HitFeedbackSprites sprites;

	public GameObject uiHolder;
	[Obsolete]
	private readonly List<Vector2> locations = new List<Vector2>();
	public GameObject locationHolder;
	private GameSceneMaster gameManager;

	//these 2 should be combined
	private readonly Pulse[] pulses = new Pulse[4];
	private readonly Vector3[] pulsePosition = new Vector3[4];

	void Start ()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameSceneMaster>();
		for (int i = 0; i < gameManager.birds.Length; i++)
		{
			const float offset = 7;
			Vector3 position = gameManager.birds[i].transform.position + Vector3.up*offset;

			pulsePosition[i] = Camera.main.WorldToScreenPoint(position);
		}

		//locations.Add(locationHolder.transform.position);
		for (int i = 0; i < locationHolder.transform.childCount; i++)
		{
			Vector3 position = locationHolder.transform.GetChild(i).transform.position;
			Vector2 loc = new Vector2(position.x, position.y);
			locations.Add(loc);
		}
		for (int i = 0; i < uiHolder.transform.childCount; i++)
		{
			Vector3 position = uiHolder.transform.GetChild(i).transform.position;
			Vector2 loc = new Vector2(position.x, position.y);
			locations.Add(loc);
		}

		for (int i = 0; i < 4; i++)
		{
			SpawnPrecisionFeedback(i);
		}

	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetPulse(0, sprites.bad);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SetPulse(0, sprites.good);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SetPulse(0, sprites.great);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			SetPulse(0, sprites.perfect);
		}

	}

	public void SetPulse(int playerNumber, Sprite img)
	{
		var pulse = pulses[playerNumber];
		pulse.GetComponent<Image>().sprite = img;
		pulse.DoIt();
	}

	/// <summary>
	/// Spawns the given sprite at player position
	/// </summary>
	/// <param name="playerNumber">Number from 1 to 4</param>
	/// <param name="img">The sprite to use</param>
	public void SpawnFloater(int playerNumber, Sprite img)
	{
		var obj = CreateFeedbackTextObject(playerNumber, img);
		obj.AddComponent<FloatingText>();
	}

	private void SpawnPrecisionFeedback(int playerNumber)
	{
		var obj = CreateFeedbackTextObject(playerNumber, null);
		var pulse = obj.AddComponent<Pulse>();
		pulses[playerNumber] = pulse;
		pulse.Hide();
	}

	private GameObject CreateFeedbackTextObject(int playerNumber, Sprite img)
	{
		GameObject obj = new GameObject("myTextGO");
		obj.transform.SetParent(transform, false);

		obj.AddComponent<Outline>();

		Image myText = obj.AddComponent<Image>();
		myText.sprite = img;

		//if (playerNumber > 4) newGO.GetComponent<FloatingText>().isScore = true;
		obj.GetComponent<RectTransform>().position = pulsePosition[playerNumber];//locations[playerNumber];
		obj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);
		return obj;
	}

	/*
	public void SpawnFloater(string text, int pos, Color color)
	{
		GameObject newGO = new GameObject("myTextGO");
		newGO.transform.SetParent(this.transform, false);

		newGO.AddComponent<Outline>();

		Text myText = newGO.AddComponent<Text>();
		myText.font = quicksand;
		myText.text = text;
		myText.color = color;

		newGO.AddComponent<FloatingText>();
		if (pos > 4) newGO.GetComponent<FloatingText>().isScore = true;
		newGO.GetComponent<RectTransform>().position = locations[pos];
		newGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100,20);

	}

	public void SpawnFloater(string text, int pos, Color color, Color outlineColor)
	{
		GameObject newGO = new GameObject("myTextGO");
		newGO.transform.SetParent(this.transform, false);
		newGO.AddComponent<Outline>();

		Text myText = newGO.AddComponent<Text>();
		myText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		GetComponent<Outline>().effectColor = outlineColor;
		myText.text = text;
		myText.color = color;
		newGO.AddComponent<FloatingText>();
		if (pos > 4) newGO.GetComponent<FloatingText>().isScore = true;
		newGO.GetComponent<RectTransform>().position = locations[pos];
	}*/
}
