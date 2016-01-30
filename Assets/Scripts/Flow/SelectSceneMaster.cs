using UnityEngine;
using System.Collections;

public class SelectSceneMaster : MonoBehaviour {

	public BirdSelector[] birdSelectors;
	// Use this for initialization
	public float countDown = 5.0f;
	public bool showCountDown = false;

	void Start () {
		for (int ii = 0; ii < birdSelectors.Length; ii++) {
			birdSelectors [ii].SetSceneMaster (this);
		}
	}
	
	// Update is called once per frame
	void Update () {
		int inactiveCount = 0;
		int selectingCount = 0;
		int confirmedCount = 0;

		for (int ii = 0; ii < birdSelectors.Length; ii++) {
			switch (birdSelectors [ii].state) {
			case BirdSelectorState.INACTIVE:
				inactiveCount++;
				break;
			case BirdSelectorState.SELECTING:
				inactiveCount++;
				break;
			case BirdSelectorState.CONFIRMED:
				inactiveCount++;
				break;
			}
		}

		if (confirmedCount > 0 && selectingCount == 0) {
			countDown -= Time.deltaTime;
			showCountDown = true;
			if (countDown <= 0) {
				StartGame ();
			}
		} else {
			countDown = 5.0f;
			showCountDown = false;
		}
	}

	public int GetPreviousFreeBird(int fromIdx) {
		//TODO
		return 0;
	}

	public int NextPreviousFreeBird(int fromIdx) {
		//TODO
		return 0;
	}

	void StartGame(){
		//TODO
	}
}
