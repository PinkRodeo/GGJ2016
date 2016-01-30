using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasHandler : MonoBehaviour {
    public Text score1;
    public Text score2;
    public Text score3;
    public Text score4;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        score1.text = ScoreHandler.getInstance().getScore(1)+"";
        score2.text = ScoreHandler.getInstance().getScore(2)+"";
        score3.text = ScoreHandler.getInstance().getScore(3)+"";
        score4.text = ScoreHandler.getInstance().getScore(4)+"";
    }


}
