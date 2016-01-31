using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InitScript : MonoBehaviour {
	public TextAsset poses;
	// Use this for initialization
	void Start () {
		Globals.Init (poses);
		SceneManager.LoadScene ("SelectScene");
	}

}
