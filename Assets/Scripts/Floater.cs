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
	public Font quicksand;
	List<Vector2> locations = new List<Vector2>();
	public GameObject locationHolder;
	private GameSceneMaster gameManager;
	// Use this for initialization

	void Start ()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameSceneMaster>();

		locations.Add(locationHolder.transform.position);
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
	}

	// Update is called once per frame
	void Update ()
	{
		if (!gameManager.end && ScoreHandler.GetInstance().GetScore(1) > 0)
		{
			guitime -= Time.deltaTime;
		}


		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SpawnFloater(1, sprites.bad);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SpawnFloater(2, sprites.good);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SpawnFloater(3, sprites.great);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			SpawnFloater(4, sprites.perfect);
		}

	}

	public void SpawnFloater(int pos, Sprite img)
	{
		GameObject newGO = new GameObject("myTextGO");
		newGO.transform.SetParent(this.transform, false);

		newGO.AddComponent<Outline>();

		Image myText = newGO.AddComponent<Image>();
		myText.sprite = img;

		newGO.AddComponent<FloatingText>();
		if (pos > 4) newGO.GetComponent<FloatingText>().isScore = true;
		newGO.GetComponent<RectTransform>().position = locations[pos];
		newGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);

	}

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
	}
}
