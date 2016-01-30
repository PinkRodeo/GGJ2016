using UnityEngine;
using System.Collections;

public class InitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Globals.Init ();
		Application.LoadLevel ("SelectScene");
	}

}
