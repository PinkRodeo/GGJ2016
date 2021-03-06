﻿using UnityEngine;

public class BirdWing
{

	private BirdBone shoulder;
	private BirdBone wing_1;
	//private BirdBone wing_1_f;

	private BirdBone wing_2;
	private BirdBone wing_2_f;

	private BirdBone wing_3;
	//private BirdBone wing_3_f;

	private float sign = 1f;

	private float currentApplied;

	private ControllerInput input;
	private BirdControl birdControl;

	public ControllerAction trigger = ControllerAction.R2;

	public BirdWing(Transform shoulderTransform, string suffix, ControllerInput input, BirdControl birdControl)
	{
		this.input = input;
		this.birdControl = birdControl;

		if (suffix == "_L")
			sign = -1f;


		shoulder = BirdBone.CreateBirdBone(shoulderTransform);

		wing_1 = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_1" + suffix));

		//wing_1_f = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_F_1" + suffix));

		wing_2 = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_2" + suffix));
		wing_2_f = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_F_2" + suffix));

		wing_3 = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_3" + suffix));
		//wing_3_f = BirdBone.CreateBirdBone(shoulderTransform.FindInChildren("Wing_F_3" + suffix));
	}

	private float _keyboardInput = 0f;

	public void Update(float dt)
	{
		float currentInput = input.GetAxis(trigger);
		currentInput = Mathf.MoveTowards(currentApplied, currentInput, 9f * dt);
		currentApplied = currentInput;

		if (birdControl.DEBUG_ENABLE_KEYBOARD)
		{
			const float MOV_AMOUNT = 0.2f;

			if (trigger == ControllerAction.L2 && Input.GetKey(KeyCode.A))
				_keyboardInput += MOV_AMOUNT;
			if (trigger == ControllerAction.R2 && Input.GetKey(KeyCode.D))
				_keyboardInput += MOV_AMOUNT;

			currentInput = _keyboardInput;
			_keyboardInput *= 0.9f;
		}

		shoulder.bone.localRotation = shoulder.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(-20f * sign, 0, currentInput), Vector3.up)
									  * Quaternion.AngleAxis(Mathf.Lerp(20f * sign, 0, currentInput), Vector3.forward);


		wing_1.bone.localRotation = wing_1.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(-80f, 40f, currentInput), Vector3.right);


		wing_2.bone.localRotation = wing_2.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(50f* sign, 0, currentInput), Vector3.forward)
									*  Quaternion.AngleAxis(Mathf.Lerp(-10f, 0, currentInput), Vector3.right);

		wing_2.bone.localScale = new Vector3(1, Mathf.Lerp(0.7511079f, 1f, currentInput), 1f);


		wing_2_f.bone.localRotation = wing_2_f.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(-10f, 0f, currentInput), Vector3.forward);

		wing_2_f.bone.localScale = new Vector3(1, Mathf.Lerp(0.5f, 1f, currentInput), 1f);


		wing_3.bone.localRotation = wing_3.initialLocalRotation * Quaternion.AngleAxis(Mathf.Lerp(-5f, 10f, currentInput), Vector3.right);
	}

}
