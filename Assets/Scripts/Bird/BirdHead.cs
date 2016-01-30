using UnityEngine;
using System.Collections;

public class BirdHead 
{

	private BirdBone neck;
	private BirdBone head;

	private BirdBone beack;
	private BirdBone headFeather;

	private ControllerInput input;

	private float r3_held = 0f;

	private float beak_held = 0f;


	public BirdHead(Transform neckTransfrom, ControllerInput input)
	{
		this.input = input;

		neck = BirdBone.CreateBirdBone(neckTransfrom);

		head = BirdBone.CreateBirdBone(neckTransfrom.FindInChildren("Head"));

		beack = BirdBone.CreateBirdBone(neckTransfrom.FindInChildren("Head_Beak_Lower"));

		headFeather = BirdBone.CreateBirdBone(neckTransfrom.FindInChildren("Head_Feather"));

	}

	const float HEAD_POS_MOD = -0.005f;
	const float NECK_POS_MOD = -0.002f;


	public void Update(float dt)
	{

		Vector2 stick = input.GetRightStick();
		
		neck.bone.localPosition = neck.initialLocalPosition + new Vector3(stick.x * NECK_POS_MOD, stick.y * NECK_POS_MOD);
		head.bone.localPosition = head.initialLocalPosition + new Vector3(stick.x*HEAD_POS_MOD, stick.y*HEAD_POS_MOD);

		if (input.GetKey(ControllerInput.ControllerAction.R3))
		{
			r3_held += dt*10f;
			r3_held = Mathf.Min(r3_held, 1f);
		}
		else
		{
			r3_held -= dt*5f;
			r3_held = Mathf.Max(r3_held, 0f);
			
		}
		var normalized = -stick.normalized;

		head.bone.localRotation = head.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(0, 30f * r3_held, stick.magnitude), new Vector2(normalized.y, normalized.x));
		neck.bone.localRotation = neck.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(0, 5f * r3_held, stick.magnitude), new Vector2(normalized.y, normalized.x));


		if (input.GetKey(ControllerInput.ControllerAction.A))
		{
			beak_held += dt * 10f;
			beak_held = Mathf.Min(beak_held, 1f);
		}
		else
		{
			beak_held -= dt * 15f;
			beak_held = Mathf.Max(beak_held, 0f);
		}

		beack.bone.localRotation = beack.initialLocalRotation *
					   Quaternion.AngleAxis(-35f * beak_held, Vector3.right);
	}
}
