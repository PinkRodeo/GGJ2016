using System;
using UnityEngine;
using System.Collections.Generic;

public class DanceScript : MonoBehaviour
{
	private enum BodyPart
	{
		None,
		Head,
		LeftWing,
		RightWing,
		Tail
	}

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

	public List<ObjectStrengthCombo> leftWing;
	public List<ObjectStrengthCombo> rightWing;
	public List<ObjectStrengthCombo> head;
	public List<ObjectStrengthCombo> tail;

	private ControllerInput input;
	private BodyPart currentBodyPart = BodyPart.None;


	// Use this for initialization
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

	// Update is called once per frame
	void Update ()
	{
		SwitchSelectedPart();
		Dance();
	}

	private void Dance()
	{
		var leftTrigger = input.GetAxis(ControllerInput.ControllerAction.L2);
		var rightTrigger = input.GetAxis(ControllerInput.ControllerAction.R2);


		DoHead();
		DoAss();


		switch (currentBodyPart)
		{
		case BodyPart.LeftWing:
			DoWing(leftTrigger, rightTrigger, leftWing);
			break;
		case BodyPart.RightWing:
			DoWing(leftTrigger, rightTrigger, rightWing);
			break;
		}
	}

	private void DoAss()
	{
		float rightStickX = input.GetAxis(ControllerInput.ControllerAction.RIGHT_STICK_X);
		float rightStickY = input.GetAxis(ControllerInput.ControllerAction.RIGHT_STICK_Y);
		float posModifier = 0.1f;

		foreach (var combo in tail)
		{
			Transform trans = combo.moveableObject.transform;
			Vector3 newPos = combo.localStartingPosition +
							 (new Vector3(rightStickX, 0, rightStickY)*combo.modifierStrength*posModifier);
			trans.localPosition = Vector3.Lerp(trans.localPosition, newPos, Time.deltaTime);
		}
	}

	private void DoHead()
	{
		float leftStickX = input.GetAxis(ControllerInput.ControllerAction.LEFT_STICK_X);
		float leftStickY = input.GetAxis(ControllerInput.ControllerAction.LEFT_STICK_Y);
		float posModifier = 0.1f;

		foreach (var combo in head)
		{
			Transform trans = combo.moveableObject.transform;

			//I added - to invert because it looks cooler, makes no other logical sense right now
			Vector3 newPos = combo.localStartingPosition +
							 -(new Vector3(leftStickX, 0, leftStickY)*combo.modifierStrength*posModifier);
			trans.localPosition = Vector3.Lerp(trans.localPosition, newPos, Time.deltaTime);
		}
	}

	private void DoWing(float leftTrigger, float rightTrigger, List<ObjectStrengthCombo> wing)
	{
		if (leftTrigger > -0.7f)
		{
			foreach (var combo in wing)
			{
				Transform trans = combo.moveableObject.transform;
				trans.Rotate(trans.forward, 1*combo.modifierStrength);
			}
		}
		if (rightTrigger > -0.7f)
		{
			foreach (var combo in wing)
			{
				Transform trans = combo.moveableObject.transform;
				trans.Rotate(trans.forward, -1*combo.modifierStrength);
			}
		}
	}

	private void SwitchSelectedPart()
	{
		if (input.GetKeyDown(ControllerInput.ControllerAction.CROSS))
		{
			currentBodyPart = BodyPart.Tail;
		}
		if (input.GetKeyDown(ControllerInput.ControllerAction.SQUARE))
		{
			currentBodyPart = BodyPart.LeftWing;
		}
		if (input.GetKeyDown(ControllerInput.ControllerAction.CIRCLE))
		{
			currentBodyPart = BodyPart.RightWing;
		}
		if (input.GetKeyDown(ControllerInput.ControllerAction.TRIANGLE))
		{
			currentBodyPart = BodyPart.Head;
		}
	}
}
