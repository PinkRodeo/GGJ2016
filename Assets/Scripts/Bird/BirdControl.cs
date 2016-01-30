using UnityEngine;
using System.Collections;

public class BirdControl : MonoBehaviour
{
	private BirdWing leftWing;
	private BirdWing rightWing;

	private ControllerInput input;


	// Use this for initialization
	void Start ()
	{
		input = new ControllerInput();

		leftWing = new BirdWing(transform.FindInChildren("Shoulder_L"), "_L");
		rightWing = new BirdWing(transform.FindInChildren("Shoulder_R"), "_R");


	}

	// Update is called once per frame
	void Update ()
	{
		float dt = Time.deltaTime;

		leftWing.Update(dt, input.GetAxis(ControllerInput.ControllerAction.L2));

		rightWing.Update(dt, input.GetAxis(ControllerInput.ControllerAction.R2));

	}
}
