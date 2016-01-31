using UnityEngine;

public class BirdBody
{
	private ControllerInput input;

	private BirdBone body;

	private float initialTime;

	private Vector2 currentAppliedVector2;

	public BirdBody(Transform temp, ControllerInput input)
	{
		initialTime = Time.time;


		this.input = input;

		body = BirdBone.CreateBirdBone(temp);
	}

	const float HEAD_POS_MOD = -0.005f;
	const float BODY_POS_MOD = -0.004f;

	const float FREQUENCY = 128f / 60f / 1f;

	const float SIDWAYSROTATE = 25f;

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
		* Quaternion.AngleAxis(Mathf.Lerp(-SIDWAYSROTATE, SIDWAYSROTATE, (stick.x + 1f) / 2f), Vector3.forward);

		float moveDown = Mathf.Cos(2 * Mathf.PI * FREQUENCY * (Time.time - initialTime));
		float moveSideway = Mathf.Sin(2 * Mathf.PI * FREQUENCY / 4f * (Time.time - initialTime));

		body.bone.position = body.initialWorldPosition + new Vector3(moveSideway * .1f, moveDown * .1f, 0);

		//wing_1.bone.localRotation = wing_1.initialLocalRotation * 


		//body.bone.localPosition = body.initialLocalPosition + new Vector3(stick.x * HEAD_POS_MOD, stick.y * HEAD_POS_MOD);

	}

}
