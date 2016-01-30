using System;
using UnityEngine;
using System.Collections.Generic;

public class DanceScript : MonoBehaviour
{
	[Serializable]
	public struct ObjectStrengthCombo
	{
		public GameObject moveableObject;
		public float modifierStrength;
		[HideInInspector]
		public Vector3 localStartingPosition;
		[HideInInspector]
		public Vector3 localStartingEulerAngles;
	}

	public float speed;

	public List<ObjectStrengthCombo> leftWing;
	public List<ObjectStrengthCombo> rightWing;
	public List<ObjectStrengthCombo> head;
	public List<ObjectStrengthCombo> tail;

	private ControllerInput input;

	//I really cannot be bothered with rotations
	private int leftWingCounter;
	private int rightWingCounter;


	void Start ()
	{
		input = new ControllerInput();
		SetStartingValues(head);
		SetStartingValues(tail);
		SetStartingValues(leftWing);
		SetStartingValues(rightWing);
	}

	private static void SetStartingValues(List<ObjectStrengthCombo> container)
	{
		for (int i = 0; i < container.Count; i++)
		{
			var combo = container[i];
			Transform trans = combo.moveableObject.transform;
			combo.localStartingPosition = trans.localPosition;
			combo.localStartingEulerAngles = trans.localEulerAngles;
			container[i] = combo;
		}
	}

	void Update ()
	{
		Dance();

		if (input.GetKeyDown(ControllerInput.ControllerAction.TOUCHPAD_PRESS))
		{
			Log.Weikie("asd");
		}
	}

	private void Dance()
	{
		DoHead();
		DoAss();

		DoLeftWing();
		DoRightWing();
	}

	private void DoAss()
	{
		float rightStickX = input.GetAxis(ControllerInput.ControllerAction.RIGHT_STICK_X);
		float rightStickY = input.GetAxis(ControllerInput.ControllerAction.RIGHT_STICK_Y);
		float posModifier = 0.1f;
		Vector3 inputValues = new Vector3(rightStickX, 0, rightStickY);
		foreach (var combo in tail)
		{
			Transform trans = combo.moveableObject.transform;
			Vector3 newPos = combo.localStartingPosition + (inputValues * combo.modifierStrength*posModifier);
			trans.localPosition = Vector3.Lerp(trans.localPosition, newPos, Time.deltaTime * speed);
		}
	}

	private void DoHead()
	{
		float leftStickX = input.GetAxis(ControllerInput.ControllerAction.LEFT_STICK_X);
		float leftStickY = input.GetAxis(ControllerInput.ControllerAction.LEFT_STICK_Y);
		float posModifier = 0.1f;
		var inputValues = new Vector3(-leftStickX, 0, leftStickY);
		foreach (var combo in head)
		{
			Transform trans = combo.moveableObject.transform;

			//I added - to invert because it looks cooler, makes no other logical sense right now
			Vector3 newPos = combo.localStartingPosition +
							 (inputValues*combo.modifierStrength*posModifier);
			trans.localPosition = Vector3.Lerp(trans.localPosition, newPos, Time.deltaTime * speed);
		}
	}

	private void DoLeftWing()
	{
		bool useRotation = false;

		if (!useRotation)
		{
			//I dont really know what this does, I copied/edited this from above and it works better than rotation
			float posModifier = 0.1f;
			var leftTrigger = input.GetAxis(ControllerInput.ControllerAction.L2);
			Vector3 inputValue = new Vector3(0, 0, -leftTrigger);
			//if (leftTrigger > 0.1f && leftWingCounter < 80)
			{
				leftWingCounter++;
				foreach (var combo in leftWing)
				{
					Transform trans = combo.moveableObject.transform;
					Vector3 newPos = combo.localStartingPosition + (inputValue * combo.modifierStrength * posModifier);
					trans.localPosition = Vector3.Lerp(trans.localPosition, newPos, Time.deltaTime * speed);
					//trans.position(trans.forward, 1*combo.modifierStrength);
				}
			}
		}

		if (useRotation)
		{

			var leftTrigger = input.GetAxis(ControllerInput.ControllerAction.L2);
			if (leftTrigger > 0.1f && leftWingCounter < 80)
			{
				leftWingCounter++;
				foreach (var combo in leftWing)
				{
					Transform trans = combo.moveableObject.transform;
					trans.Rotate(-trans.forward, combo.modifierStrength);
				}
			}
			else if (leftWingCounter > 0)
			{
				leftWingCounter--;
				foreach (var combo in leftWing)
				{
					Transform trans = combo.moveableObject.transform;
					trans.Rotate(-trans.forward, -combo.modifierStrength);
				}
			}
		}
	}

	//Copy dat floppy
	private void DoRightWing()
	{
		var rightTrigger = input.GetAxis(ControllerInput.ControllerAction.R2);
		if (rightTrigger > 0.1f && rightWingCounter < 80)
		{
			rightWingCounter++;
			foreach (var combo in rightWing)
			{
				Transform trans = combo.moveableObject.transform;
				trans.Rotate(trans.forward, -combo.modifierStrength);
			}
		}
		else if (rightWingCounter > 0)
		{
			rightWingCounter--;
			foreach (var combo in rightWing)
			{
				Transform trans = combo.moveableObject.transform;
				trans.Rotate(trans.forward, combo.modifierStrength);
			}
		}
	}
}
