using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;


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
	[Obsolete]
	public List<Sprite> spriteList= new List<Sprite>();
	public HitFeedbackSprites sprites;

	private float guitime = 6;

	public GameObject uiHolder;
	[Obsolete]
	public Font quicksand;
	private readonly List<Vector2> locations = new List<Vector2>();
	public GameObject locationHolder;

	private Pulse[] pulses = new Pulse[4];

	void Start ()
	{
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

	public void SpawnPrecisionFeedback(int playerNumber)
	{
		var obj = CreateFeedbackTextObject(playerNumber, null);
		var pulse = obj.AddComponent<Pulse>();
		pulses[playerNumber] = pulse;
		pulse.Hide();
	}

	private GameObject CreateFeedbackTextObject(int playerNumber, Sprite img)
	{
		GameObject obj = new GameObject("myTextGO");
		obj.transform.SetParent(this.transform, false);

		obj.AddComponent<Outline>();

		Image myText = obj.AddComponent<Image>();
		myText.sprite = img;

		//if (playerNumber > 4) newGO.GetComponent<FloatingText>().isScore = true;
		obj.GetComponent<RectTransform>().position = locations[playerNumber];
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
