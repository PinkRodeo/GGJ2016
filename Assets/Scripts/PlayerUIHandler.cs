using UnityEngine;
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
			GameObject nextplayer = (GameObject)Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
			nextplayer.transform.SetParent(holder[i].transform);
			nextplayer.transform.localPosition = Vector3.zero;
			nextplayer.GetComponent<RectTransform>().sizeDelta = Vector2.zero;


			uiHolder[i] = holder[i].transform.GetChild(0).GetComponent<PlayerUI>();
		}
	}

	void Update ()
	{
		DebugKeys();
		UpdateText();
	}

	private static void DebugKeys()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			ScoreHandler.GetInstance().AddScore(1, 100);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			ScoreHandler.GetInstance().AddScore(2, 100);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ScoreHandler.GetInstance().AddScore(3, 100);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			ScoreHandler.GetInstance().AddScore(4, 100);
		}
	}

	private void UpdateText()
	{
		for (var i = 0; i < playerCount; i++)
		{
			uiHolder[i].textUI.text = ScoreHandler.GetInstance().GetScore(i + 1).ToString();
			// get the score to display for in items
		}
	}
}
