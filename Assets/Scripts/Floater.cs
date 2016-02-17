using System;
using UnityEngine;
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

		for (int i = 0; i < 4; i++)
		{
			SpawnPrecisionFeedback(i);
		}
	}

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

		obj.GetComponent<RectTransform>().position = pulsePosition[playerNumber];
		obj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);
		return obj;
	}
}
