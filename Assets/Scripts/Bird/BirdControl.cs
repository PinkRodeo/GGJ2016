using UnityEngine;

public class BirdControl : MonoBehaviour
{
	private BirdWing leftWing;
	private BirdWing rightWing;

	private BirdHead head;


	private ControllerInput input;

	// Use this for initialization
	void Start ()
	{
		input = new ControllerInput(1);

		leftWing = new BirdWing(transform.FindInChildren("Shoulder_L"), "_L", input);
		rightWing = new BirdWing(transform.FindInChildren("Shoulder_R"), "_R", input);

		head = new BirdHead(transform.FindInChildren("Neck"), input);



	}

	// Update is called once per frame
	void Update ()
	{
		float dt = Time.deltaTime;

		leftWing.Update(dt);
		rightWing.Update(dt);

		head.Update(dt);
	}
}
