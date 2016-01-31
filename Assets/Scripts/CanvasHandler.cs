using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasHandler : MonoBehaviour
{
	public Text score1;
	public Text score2;
	public Text score3;
	public Text score4;
	// Use this for initialization
	void Start ()
	{
		Debug.LogWarning("[CanvasHandler] Needs to pass the songs bpm the timer, not a hardcoded float.");
		SongTimer.StartSong(128f);
	}

	// Update is called once per frame
	void Update ()
	{
		score1.text = ScoreHandler.GetInstance().GetScore(1)+"";
		score2.text = ScoreHandler.GetInstance().GetScore(2)+"";
		score3.text = ScoreHandler.GetInstance().GetScore(3)+"";
		score4.text = ScoreHandler.GetInstance().GetScore(4)+"";
	}


}
