using UnityEngine;
using System.Collections;

public enum BirdSelectorState {
	INACTIVE,
	SELECTING,
	CONFIRMED
}

public class BirdSelector : MonoBehaviour {

	public int controllerID = 0;
	public GameObject bird;
	public Light spotLight;

	public BirdSelectorState state = BirdSelectorState.INACTIVE;

	private int selectedBird = 0;
	private float originalIntensity;

	private SelectSceneMaster sceneMaster;

	// Use this for initialization
	void Start () {
		//turn off the lights!
		originalIntensity = spotLight.intensity;
		spotLight.intensity = 0;
	}

	public void SetSceneMaster(SelectSceneMaster master) {
		sceneMaster = master;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void HandleSelect() {
		if (state == BirdSelectorState.INACTIVE) {
			state = BirdSelectorState.SELECTING;
			spotLight.intensity = originalIntensity;
			return;
		}
		if (state == BirdSelectorState.INACTIVE) {
			state = BirdSelectorState.CONFIRMED;
			return;
		}
	}

	void HandleDeselect() {
		if (state == BirdSelectorState.CONFIRMED) {
			state = BirdSelectorState.SELECTING;
			return;
		}
		if (state == BirdSelectorState.SELECTING) {
			state = BirdSelectorState.INACTIVE;
			spotLight.intensity = 0;
			return;
		}
	}

	void HandleLeft() {
		if (state != BirdSelectorState.SELECTING)
			return;
		var newType = sceneMaster.GetPreviousFreeBird (selectedBird);
		SetBirdType (newType);
	}

	void HandleRight() {
		if (state != BirdSelectorState.SELECTING)
			return;
		var newType = sceneMaster.GetPreviousFreeBird (selectedBird);
		SetBirdType (newType);
	}

	void SetBirdType(int type) {
		selectedBird = type;
	}

	int GetBirdType() {
		return selectedBird;
	}
}
