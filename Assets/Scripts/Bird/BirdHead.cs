using UnityEngine;
using System.Collections;

public class BirdHead 
{

	private BirdBone neck;
	private BirdBone head;

	private BirdBone beack;
	private BirdBone headFeather;

	private ControllerInput input;

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
		Debug.Log(stick);

		neck.bone.localPosition = neck.initialLocalPosition + new Vector3(stick.x * NECK_POS_MOD, 0, stick.y * NECK_POS_MOD);


		head.bone.localPosition = head.initialLocalPosition + new Vector3(stick.x*HEAD_POS_MOD, 0, stick.y*HEAD_POS_MOD);

	}
}
