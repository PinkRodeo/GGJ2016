using UnityEngine;

public class TestingRigidbodyCS : MonoBehaviour {

	private GameObject _ball1;
	// Use this for initialization
	private void Start () {
		_ball1 = GameObject.Find("Sphere1");

		LeanTween.rotateAround( _ball1, Vector3.forward, -90f, 1.0f);

		LeanTween.move( _ball1, new Vector3(2f,0f,7f), 1.0f).setDelay(1.0f).setRepeat(-1);
	}
	
	// Update is called once per frame
	private void Update () {
	
	}
}
