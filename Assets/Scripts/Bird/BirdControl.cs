using UnityEngine;

public class BirdControl : MonoBehaviour
{
	[Range(1, 4)]
	public int playerId = 1;

	private BirdWing leftWing;
	private BirdWing rightWing;

	private BirdHead head;
	private BirdBody body;

	private BirdLeg leftLeg;
	private BirdLeg rightLeg;

	private BirdTail tail;

	public bool _isInitialized = false;

	private ControllerInput input;


	public ControllerInput GetInput()
	{
		return input;
	}

	public bool IsInitialized()
	{
		return _isInitialized;
	}

	// Use this for initialization
	void Start ()
	{
		//		SongTimer.StartSong(128f);
		//_initializeController();

	}

	public void _initializeController()
	{
		input = new ControllerInput(playerId);

		leftWing = new BirdWing(transform.FindInChildren("Shoulder_L"), "_L", input);
		rightWing = new BirdWing(transform.FindInChildren("Shoulder_R"), "_R", input);

		leftWing.trigger = ControllerAction.R2;
		rightWing.trigger = ControllerAction.L2;


		leftLeg = new BirdLeg(transform.FindInChildren("Leg_Feet_L"), "_L", input);
		rightLeg = new BirdLeg(transform.FindInChildren("Leg_Feet_R"), "_R", input);


		head = new BirdHead(transform.FindInChildren("Neck"), input);

		body = new BirdBody(transform.FindInChildren("Body"), input);

		tail = new BirdTail(transform.FindInChildren("Tail_1"), input);

		_isInitialized = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!_isInitialized)
		{
			return;
			//_initializeController();
		}

		if (_isInitialized)
		{
			float dt = Time.deltaTime;

			leftWing.Update(dt);
			rightWing.Update(dt);

			head.Update(dt);

			body.Update(dt);

			leftLeg.Update(dt);
			rightLeg.Update(dt);

			tail.Update(dt);
		}
	}
}
