using UnityEngine;

public class BirdControl : MonoBehaviour
{
	[Range(1, 4)]
	public int playerId = 1;

	private BirdWing leftWing;
	private BirdWing rightWing;

	private BirdHead head;
	private BirdBody body;


	private ControllerInput input;

	// Use this for initialization
	void Start ()
	{
		input = new ControllerInput(playerId);

		leftWing = new BirdWing(transform.FindInChildren("Shoulder_L"), "_L", input);
		rightWing = new BirdWing(transform.FindInChildren("Shoulder_R"), "_R", input);

		leftWing.trigger = ControllerAction.R2;
		rightWing.trigger = ControllerAction.L2;


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
