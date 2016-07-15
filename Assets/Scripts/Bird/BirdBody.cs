using UnityEngine;

public class BirdBody
{
	private ControllerInput input;
	private BirdControl birdControl;

	private BirdBone body;

	private Vector2 currentAppliedVector2;

	public BirdBody(Transform temp, ControllerInput input, BirdControl birdControl)
	{
		this.input = input;
		this.birdControl = birdControl;

		body = BirdBone.CreateBirdBone(temp);
	}

	private const float ROTATE_SIDEWAYS = 25f;
	private Vector2 _keyboardInput = new Vector2();

	public void Update(float dt)
	{
		Vector2 stick = input.GetRightStick();
		stick.x = stick.x * -1f;
		stick.y = stick.y * -1f;


		stick.x = Mathf.MoveTowards(currentAppliedVector2.x, stick.x, 18f*dt);
		stick.y = Mathf.MoveTowards(currentAppliedVector2.y, stick.y, 18f * dt);

		currentAppliedVector2 = stick;

		if (birdControl.DEBUG_ENABLE_KEYBOARD)
		{
			const float MOV_AMOUNT = 0.15f;
			if (Input.GetKey(KeyCode.T))
				_keyboardInput.y -= MOV_AMOUNT;

			if (Input.GetKey(KeyCode.G))
				_keyboardInput.y += MOV_AMOUNT;

			_keyboardInput.y = Mathf.Clamp(_keyboardInput.y, -1f, 1f);


			if (Input.GetKey(KeyCode.F))
				_keyboardInput.x += MOV_AMOUNT;

			if (Input.GetKey(KeyCode.H))
				_keyboardInput.x -= MOV_AMOUNT;

			_keyboardInput.x = Mathf.Clamp(_keyboardInput.x, -1f, 1f);

			stick = _keyboardInput;

			_keyboardInput.y *= 0.9f;
			_keyboardInput.x *= 0.9f;
		}

		//body.bone.localPosition = body.initialLocalPosition + new Vector3(stick.x * BODY_POS_MOD, stick.y * BODY_POS_MOD);
		body.bone.localRotation = body.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(-10f, 20f, (stick.y+1f)/2f), Vector3.right)
								  * Quaternion.AngleAxis(Mathf.Lerp(-ROTATE_SIDEWAYS, ROTATE_SIDEWAYS, (stick.x + 1f) / 2f), Vector3.forward);

		if (SongTimer.isSongRunning)
		{
			float moveDown = Mathf.Cos(SongTimer.TimedValue()) * SongTimer.LeadInRatio();
			float moveSideway = Mathf.Sin(SongTimer.TimedValue(4f)) * SongTimer.LeadInRatio();

			body.bone.position = body.initialWorldPosition + new Vector3(moveSideway * .1f, moveDown * .1f, 0);
		}
	}

}
