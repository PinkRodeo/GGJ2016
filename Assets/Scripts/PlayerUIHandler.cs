using UnityEngine;
using System.Collections;

public class PlayerUIHandler : MonoBehaviour {
    public GameObject[] holder = new GameObject[3];
    public GameObject prefab;
    int playeramount = 4;   // the amount of player in the game TODO number should come from somewhere else
	// Use this for initialization
	void Start () {
	    for (int ii = 0; ii < playeramount; ii++)
        {
            GameObject nextplayer = (GameObject)Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
            nextplayer.transform.SetParent(holder[ii].transform);
            int pnr = ii + 1;
            nextplayer.GetComponent<PlayerUI>().playerID.text = "Player " + pnr;
            nextplayer.transform.localPosition = Vector3.zero;
            nextplayer.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        }
	}
	
	// Update is called once per frame
	void Update () {
	    for (var jj = 0; jj < playeramount; jj++)
        {
            ScoreHandler.getInstance().addScore(jj+1, jj+1);// add score for now
            holder[jj].transform.GetChild(0).GetComponent<PlayerUI>().score.text = ScoreHandler.getInstance().getScore(jj + 1) + ""; // get the score to display for in items

        }
    }
}
