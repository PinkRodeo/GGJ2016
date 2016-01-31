using UnityEngine;

public class BirdLeg
{
	private BirdBone hip;
	private BirdBone leg_upper;
	private BirdBone leg_lower;
	private BirdBone leg_heel;
	private BirdBone leg_feet;

	private ControllerInput input;

	private float initialTime;

	private float sign = 1f;
	private bool isLeft = false;

	public BirdLeg(Transform feetTransform, string suffix, ControllerInput input)
	{
		if (suffix == "_L")
		{
			isLeft = true;
			sign = -1f;
		}

		initialTime = Time.time;

		this.input = input;

		leg_feet = BirdBone.CreateBirdBone(feetTransform);
		
		leg_heel = BirdBone.CreateBirdBone(feetTransform.FindInChildren("Leg_Heel" + suffix));

		leg_lower = BirdBone.CreateBirdBone(feetTransform.FindInChildren("Leg_Lower" + suffix));

		leg_upper = BirdBone.CreateBirdBone(feetTransform.FindInChildren("Leg_Upper" + suffix));

		hip = BirdBone.CreateBirdBone(feetTransform.FindInChildren("Hip" + suffix));

		
	}

	private const float FREQUENCY = 128f/60f/ 1f;

	public void Update(float dt)
	{
		float moveDown = Mathf.Cos(2 * Mathf.PI * FREQUENCY * (Time.time - initialTime));

		float moveSideway = Mathf.Sin(2 * Mathf.PI * FREQUENCY / 4f * (Time.time - initialTime));


		hip.bone.position = hip.initialWorldPosition + new Vector3(moveSideway * .1f, moveDown * .1f, 0);

		leg_upper.bone.position = leg_upper.initialWorldPosition + new Vector3(moveSideway * .08f, moveDown * .08f, 0);

		leg_lower.bone.position = leg_lower.initialWorldPosition + new Vector3(moveSideway * .05f, moveDown * .05f, 0);

		leg_heel.bone.position = leg_heel.initialWorldPosition + new Vector3(moveSideway * .02f, moveDown * .02f, 0);

		if (isLeft)
		{
			float tap = Mathf.Sin(2 * Mathf.PI * FREQUENCY / 8f * (Time.time - initialTime));

			tap -= 1f-1f/24f;


			leg_feet.bone.position = leg_feet.initialWorldPosition + new Vector3(0, Mathf.Max(tap, 0) * 5f, 0);
			//leg_feet.bone.position = leg_feet.initialWorldPosition;

		}

	}


}
