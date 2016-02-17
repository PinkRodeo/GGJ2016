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

			nextplayer.transform.localScale = Vector3.one;

			uiHolder[i] = holder[i].transform.GetChild(0).GetComponent<PlayerUI>();
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
			uiHolder[i].textUI.text = score.ToString();
			if (score == 0)
			{
				uiHolder[i].gameObject.SetActive(false);
			}
			else
			{
				uiHolder[i].gameObject.SetActive(true);
			}
		}
	}
}
