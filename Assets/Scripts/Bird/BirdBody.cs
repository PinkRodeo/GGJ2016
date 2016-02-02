using UnityEngine;

public class BirdBody
{
	private ControllerInput input;

	private BirdBone body;

	private Vector2 currentAppliedVector2;

	public BirdBody(Transform temp, ControllerInput input)
	{
		this.input = input;

		body = BirdBone.CreateBirdBone(temp);
	}
	
	private const float ROTATE_SIDEWAYS = 25f;

	public void Update(float dt)
	{
		Vector2 stick = input.GetRightStick();
		stick.x = stick.x * -1f;
		stick.y = stick.y * -1f;


		stick.x = Mathf.MoveTowards(currentAppliedVector2.x, stick.x, 18f*dt);
		stick.y = Mathf.MoveTowards(currentAppliedVector2.y, stick.y, 18f * dt);

		currentAppliedVector2 = stick;


		//body.bone.localPosition = body.initialLocalPosition + new Vector3(stick.x * BODY_POS_MOD, stick.y * BODY_POS_MOD);
		body.bone.localRotation = body.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(-10f, 20f, (stick.y+1f)/2f), Vector3.right)
		* Quaternion.AngleAxis(Mathf.Lerp(-ROTATE_SIDEWAYS, ROTATE_SIDEWAYS, (stick.x + 1f) / 2f), Vector3.forward);

		if (SongTimer.isSongRunning)
		{
			float moveDown = Mathf.Cos(SongTimer.timedValue()) * SongTimer.leadInRatio();
			float moveSideway = Mathf.Sin(SongTimer.timedValue(4f)) * SongTimer.leadInRatio();

			body.bone.position = body.initialWorldPosition + new Vector3(moveSideway * .1f, moveDown * .1f, 0);
		}
	}

}
