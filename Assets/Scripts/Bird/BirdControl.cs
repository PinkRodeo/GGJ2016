﻿using UnityEngine;

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

	public ControllerInput input;


	public ControllerInput GetInput()
	{
		return input;
	}

	public bool IsInitialized()
	{
		return _isInitialized;
	}

	public bool DEBUG_StartSongTimer = false;
	public bool DEBUG_InitControllerOnStart = false;
	public bool DEBUG_ENABLE_KEYBOARD = false;

	[Range(0, 400f)] public float DEBUG_BPM = 128f;

	// Use this for initialization
	void Start ()
	{	
		if (DEBUG_StartSongTimer)
			SongTimer.StartSong(DEBUG_BPM);
	
		if (DEBUG_InitControllerOnStart)
			_initializeController();
	}

	public void _initializeController()
	{
		if (IsInitialized()) return;

		input = new ControllerInput(playerId);

		leftWing = new BirdWing(transform.FindInChildren("Shoulder_L"), "_L", input, this);
		rightWing = new BirdWing(transform.FindInChildren("Shoulder_R"), "_R", input, this);

		leftWing.trigger = ControllerAction.R2;
		rightWing.trigger = ControllerAction.L2;


		leftLeg = new BirdLeg(transform.FindInChildren("Leg_Feet_L"), "_L", input);
		rightLeg = new BirdLeg(transform.FindInChildren("Leg_Feet_R"), "_R", input);


		head = new BirdHead(transform.FindInChildren("Neck"), input, this);

		body = new BirdBody(transform.FindInChildren("Body"), input, this);

		tail = new BirdTail(transform.FindInChildren("Tail_1"), input, this);

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

		if (!_isInitialized) return;

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
