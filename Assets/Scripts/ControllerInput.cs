using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ControllerInput
{
	public enum CurrentPlatform
	{
		Unknown,
		Windows,
		Mac
	}

	public enum ControllerType
	{
		Unknown,
		Playstation,
		Xbox
	}

	private readonly Dictionary<ControllerAction, List<KeyCode>> buttonKeymap = new Dictionary<ControllerAction, List<KeyCode>>();
	private readonly Dictionary<ControllerAction, List<string>> axisKeymap = new Dictionary<ControllerAction, List<string>>();
	private readonly CurrentPlatform currentPlatform;
	private readonly ControllerType controllerType = ControllerType.Unknown;
	private readonly int controllerPort;

	#region Public accessors

	/// <summary>
	/// Weikie's controller input wrapper
	/// </summary>
	/// <param name="controllerPort">1-based index of what controller port to use</param>
	public ControllerInput(int controllerPort)
	{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		currentPlatform = CurrentPlatform.Windows;
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
		currentPlatform = CurrentPlatform.Mac;
#else
		currentPlatform = CurrentPlatform.Unknown;
#endif

		this.controllerPort = controllerPort;
		string[] controllerNames = Input.GetJoystickNames();
		if (controllerNames.Length < controllerPort)
		{
			Log.Weikie(string.Format("Controller {0} not assigned, only {1} controllers detected", controllerPort, controllerNames.Length));
			Log.Weikie("Assigned to port 1 as default.");
			this.controllerPort = 1;
			if (controllerNames.Length == 0)
				return;
		}
		//else
		{
			string name = controllerNames[controllerPort - 1];

			if (name.ToLower().Contains("xbox"))
			{
				controllerType = ControllerType.Xbox;
				Log.Weikie("Assigned xbox controller");
				XboxBindings();
			}
			else if (name == "Wireless Controller")
			{
				controllerType = ControllerType.Playstation;
				//commented because seems to work pretty good after 5 hours of testing
				//Log.Weikie("Assigned playstation controller");
				PlaystationBinding();
			}
			else
			{
				Log.Weikie("Unknown controller, go fix. Name is " + name);
			}
		}
	}

	public bool GetKeyDown(ControllerAction action)
	{
		TransformCrossControllerButton(ref action);
		if (!IsActionMapped(action)) return false;

		List<KeyCode> keycodeList = buttonKeymap[action];
		return keycodeList.Any(Input.GetKeyDown);
	}

	public bool GetKey(ControllerAction action)
	{
		TransformCrossControllerButton(ref action);
		if (!IsActionMapped(action)) return false;

		List<KeyCode> keycodeList = buttonKeymap[action];
		return keycodeList.Any(Input.GetKey);
	}

	public bool GetKeyUp(ControllerAction action)
	{
		TransformCrossControllerButton(ref action);
		if (!IsActionMapped(action)) return false;

		List<KeyCode> keycodeList = buttonKeymap[action];
		return keycodeList.Any(Input.GetKeyUp);
	}

	public float GetAxis(ControllerAction action)
	{
		//im too lazy to add error checks here even though its copy paste and edit
		//if (!IsActionMapped(action)) return 0;

		List<string> keycodeList = axisKeymap[action];
		foreach (var axisName in keycodeList)
		{
			float value = Input.GetAxisRaw(axisName);
			if (Mathf.Abs(value) > Mathf.Epsilon)
			{
				//hack to make triggers go from -1 to 1
				if (controllerType == ControllerType.Playstation)
				{
					if (action == ControllerAction.L2 || action == ControllerAction.R2)
					{
						return (value + 1) / 2.0f;
					}
				}
				return value;
			}
		}
		return 0;
	}

	public bool AnyButtonPressed()
	{
		var values = Enum.GetValues(typeof(ControllerAction)).Cast<ControllerAction>();

		return values.Any(GetKeyDown);
	}

	/// <summary>
	/// Gets the combined axis values in a single vector
	/// </summary>
	/// <returns>A 0 to 1 normalized value, where 0.5 is idle</returns>
	public Vector2 GetLeftStick()
	{
		return new Vector2((GetAxis(ControllerAction.LEFT_STICK_X)),
						   (GetAxis(ControllerAction.LEFT_STICK_Y)));
	}

	/// <summary>
	/// Gets the combined axis values in a single vector
	/// </summary>
	/// <returns>A 0 to 1 normalized value, where 0.5 is idle</returns>
	public Vector2 GetRightStick()
	{
		return new Vector2((GetAxis(ControllerAction.RIGHT_STICK_X)),
						   (GetAxis(ControllerAction.RIGHT_STICK_Y)));
	}

	/// <summary>
	/// Return left trigger value
	/// </summary>
	/// <returns>A 0 to 1 normalized value</returns>
	public float GetLeftTrigger()
	{
		return GetAxis(ControllerAction.L2);
	}

	/// <summary>
	/// Return right trigger value
	/// </summary>
	/// <returns>A 0 to 1 normalized value</returns>
	public float GetRightTrigger()
	{
		return GetAxis(ControllerAction.R2);
	}

	public int GetControllerPort()
	{
		return controllerPort;
	}

	#endregion

	#region Private stuff

	private void TransformCrossControllerButton(ref ControllerAction action)
	{
		if (controllerType == ControllerType.Playstation)
		{
			switch (action)
			{
			case ControllerAction.A:
				action = ControllerAction.CROSS;
				break;
			case ControllerAction.B:
				action = ControllerAction.CIRCLE;
				break;
			case ControllerAction.X:
				action = ControllerAction.SQUARE;
				break;
			case ControllerAction.Y:
				action = ControllerAction.TRIANGLE;
				break;

			}
		}
		else if (controllerType == ControllerType.Playstation)
		{
			switch (action)
			{
			case ControllerAction.CIRCLE:
				action = ControllerAction.B;
				break;
			case ControllerAction.CROSS:
				action = ControllerAction.A;
				break;
			case ControllerAction.SQUARE:
				action = ControllerAction.X;
				break;
			case ControllerAction.TRIANGLE:
				action = ControllerAction.Y;
				break;
			}
		}
	}

	private bool IsActionMapped(ControllerAction action)
	{
		List<KeyCode> keycodeList;

		if (buttonKeymap.TryGetValue(action, out keycodeList) && keycodeList != null)
		{
			return true;
		}
		return false;
	}

	private void AddButtonBinding(ControllerAction action, KeyCode keyCode)
	{
		List<KeyCode> list;
		if (!buttonKeymap.ContainsKey(action))
		{
			list = new List<KeyCode>();
			buttonKeymap.Add(action, list);
		}
		else
		{
			list = buttonKeymap[action];
		}

		list.Add(keyCode);
		buttonKeymap[action] = list;
	}

	private void AddAxisBinding(ControllerAction action, string axisName)
	{
		List<string> list;
		if (!axisKeymap.ContainsKey(action))
		{
			list = new List<string>();
			axisKeymap.Add(action, list);
		}
		else
		{
			list = axisKeymap[action];
		}

		list.Add(axisName);
		axisKeymap[action] = list;
	}

	private void PlaystationBinding()
	{
		//TERRARIA STYLE HACKS HELL YEAH
		int joystickNumber = 20 * (controllerPort - 1);

		AddButtonBinding(ControllerAction.SQUARE,	KeyCode.Joystick1Button0  + joystickNumber);
		AddButtonBinding(ControllerAction.CROSS,	KeyCode.Joystick1Button1  + joystickNumber);
		AddButtonBinding(ControllerAction.CIRCLE,	KeyCode.Joystick1Button2  + joystickNumber);
		AddButtonBinding(ControllerAction.TRIANGLE, KeyCode.Joystick1Button3  + joystickNumber);
		AddButtonBinding(ControllerAction.L1,		KeyCode.Joystick1Button4  + joystickNumber);
		AddButtonBinding(ControllerAction.R1,		KeyCode.Joystick1Button5  + joystickNumber);
		AddButtonBinding(ControllerAction.L2,		KeyCode.Joystick1Button6  + joystickNumber);
		AddButtonBinding(ControllerAction.R2,		KeyCode.Joystick1Button7  + joystickNumber);
		AddButtonBinding(ControllerAction.SELECT,	KeyCode.Joystick1Button8  + joystickNumber);
		AddButtonBinding(ControllerAction.START,	KeyCode.Joystick1Button9  + joystickNumber);
		AddButtonBinding(ControllerAction.L3,		KeyCode.Joystick1Button10 + joystickNumber);
		AddButtonBinding(ControllerAction.R3,		KeyCode.Joystick1Button11 + joystickNumber);
		AddButtonBinding(ControllerAction.PLAYSTATION,		KeyCode.Joystick1Button12 + joystickNumber);
		AddButtonBinding(ControllerAction.TOUCHPAD_PRESS,	KeyCode.Joystick1Button13 + joystickNumber);

		//if (currentPlatform == CurrentPlatform.Windows)
		{
			AddAxisBinding(ControllerAction.LEFT_STICK_X,	"Controller" + controllerPort + "Axis1"); //correct
			AddAxisBinding(ControllerAction.LEFT_STICK_Y,	"Controller" + controllerPort + "Axis2"); //correct
			AddAxisBinding(ControllerAction.RIGHT_STICK_X,	"Controller" + controllerPort + "Axis3"); //correct
			AddAxisBinding(ControllerAction.RIGHT_STICK_Y,	"Controller" + controllerPort + "Axis6"); //wrong,kinda does work but shares with DPAD_X for some reason
			AddAxisBinding(ControllerAction.L2,				"Controller" + controllerPort + "Axis4"); //correct
			AddAxisBinding(ControllerAction.R2,				"Controller" + controllerPort + "Axis5"); //correct
			AddAxisBinding(ControllerAction.DPAD_X,			"Controller" + controllerPort + "Axis7"); //wrong, shares with DPAD_Y for some reason
			AddAxisBinding(ControllerAction.DPAD_Y,			"Controller" + controllerPort + "Axis8"); //correct
		}
		//Square  = joystick button 0
		//X       = joystick button 1
		//Circle  = joystick button 2
		//Triangle= joystick button 3
		//L1      = joystick button 4
		//R1      = joystick button 5
		//L2      = joystick button 6
		//R2      = joystick button 7
		//Share   = joystick button 8
		//Options = joystick button 9
		//L3      = joystick button 10
		//R3      = joystick button 11
		//PS      = joystick button 12
		//PadPress= joystick button 13
	}

	private void XboxBindings()
	{
		int joystickNumber = 20 * (controllerPort - 1);

		if (currentPlatform == CurrentPlatform.Windows)
		{
			//TERRARIA STYLE HACKS HELL YEAH
			AddButtonBinding(ControllerAction.A,		KeyCode.Joystick1Button0 + joystickNumber);
			AddButtonBinding(ControllerAction.B,		KeyCode.Joystick1Button1 + joystickNumber);
			AddButtonBinding(ControllerAction.X,		KeyCode.Joystick1Button2 + joystickNumber);
			AddButtonBinding(ControllerAction.Y,		KeyCode.Joystick1Button3 + joystickNumber);
			AddButtonBinding(ControllerAction.L1,		KeyCode.Joystick1Button4 + joystickNumber);
			AddButtonBinding(ControllerAction.R1,		KeyCode.Joystick1Button5 + joystickNumber);
			AddButtonBinding(ControllerAction.SELECT,	KeyCode.Joystick1Button6 + joystickNumber);
			AddButtonBinding(ControllerAction.START,	KeyCode.Joystick1Button7 + joystickNumber);
			AddButtonBinding(ControllerAction.L3,		KeyCode.Joystick1Button8 + joystickNumber);
			AddButtonBinding(ControllerAction.R3,		KeyCode.Joystick1Button9 + joystickNumber);

			AddAxisBinding(ControllerAction.LEFT_STICK_X,	"Controller" + controllerPort + "Axis1");
			AddAxisBinding(ControllerAction.LEFT_STICK_Y,	"Controller" + controllerPort + "Axis2");
			AddAxisBinding(ControllerAction.RIGHT_STICK_X,	"Controller" + controllerPort + "Axis4");
			AddAxisBinding(ControllerAction.RIGHT_STICK_Y,	"Controller" + controllerPort + "Axis5");
			AddAxisBinding(ControllerAction.DPAD_X,			"Controller" + controllerPort + "Axis6");
			AddAxisBinding(ControllerAction.DPAD_Y,			"Controller" + controllerPort + "Axis7");
			AddAxisBinding(ControllerAction.L2,				"Controller" + controllerPort + "Axis9");
			AddAxisBinding(ControllerAction.R2,				"Controller" + controllerPort + "Axis10");
		}
		else if (currentPlatform == CurrentPlatform.Mac)
		{
			//TERRARIA STYLE HACKS HELL YEAH
			AddButtonBinding(ControllerAction.A,		KeyCode.Joystick1Button16 + joystickNumber);
			AddButtonBinding(ControllerAction.B,		KeyCode.Joystick1Button17 + joystickNumber);
			AddButtonBinding(ControllerAction.X,		KeyCode.Joystick1Button18 + joystickNumber);
			AddButtonBinding(ControllerAction.Y,		KeyCode.Joystick1Button19 + joystickNumber);
			AddButtonBinding(ControllerAction.L1,		KeyCode.Joystick1Button13 + joystickNumber);
			AddButtonBinding(ControllerAction.R1,		KeyCode.Joystick1Button14 + joystickNumber);
			AddButtonBinding(ControllerAction.SELECT,	KeyCode.Joystick1Button10 + joystickNumber);
			AddButtonBinding(ControllerAction.START,	KeyCode.Joystick1Button9  + joystickNumber);
			AddButtonBinding(ControllerAction.L3,		KeyCode.Joystick1Button11 + joystickNumber);
			AddButtonBinding(ControllerAction.R3,		KeyCode.Joystick1Button12 + joystickNumber);

			AddAxisBinding(ControllerAction.LEFT_STICK_X,	"Controller" + controllerPort + "Axis1");
			AddAxisBinding(ControllerAction.LEFT_STICK_Y,	"Controller" + controllerPort + "Axis2");
			AddAxisBinding(ControllerAction.RIGHT_STICK_X,	"Controller" + controllerPort + "Axis3");
			AddAxisBinding(ControllerAction.RIGHT_STICK_Y,	"Controller" + controllerPort + "Axis4");
			AddAxisBinding(ControllerAction.L2,				"Controller" + controllerPort + "Axis5");
			AddAxisBinding(ControllerAction.R2,				"Controller" + controllerPort + "Axis6");
		}
		else
		{
			Log.Weikie("Unknown platform, xbox controller not assigned for player " + controllerPort);
		}
	}

	public void AxisTestPrints()
	{
		float f;

		float deadzonePrint = 0.3f;
		if (Mathf.Abs(f = GetAxis(ControllerAction.LEFT_STICK_X)) > deadzonePrint)		Log.Weikie(controllerPort + " LEFT_STICK_X " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.LEFT_STICK_Y)) > deadzonePrint)		Log.Weikie(controllerPort + " LEFT_STICK_Y " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.RIGHT_STICK_X)) > deadzonePrint)		Log.Weikie(controllerPort + " RIGHT_STICK_X " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.RIGHT_STICK_Y)) > deadzonePrint)		Log.Weikie(controllerPort + " RIGHT_STICK_Y " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.DPAD_X)) > deadzonePrint)			Log.Weikie(controllerPort + " DPAD_X " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.DPAD_Y)) > deadzonePrint)			Log.Weikie(controllerPort + " DPAD_Y " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.L2)) > deadzonePrint)				Log.Weikie(controllerPort + " L2 " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.R2)) > deadzonePrint)				Log.Weikie(controllerPort + " R2 " + f);
	}

	public void ButtonTestPrints()
	{
		if (GetKeyDown(ControllerAction.SQUARE)) Log.Weikie("Key pressed SQUARE"); //
		if (GetKeyDown(ControllerAction.CROSS)) Log.Weikie("Key pressed CROSS"); //
		if (GetKeyDown(ControllerAction.CIRCLE)) Log.Weikie("Key pressed CIRCLE"); //
		if (GetKeyDown(ControllerAction.TRIANGLE)) Log.Weikie("Key pressed TRIANGLE"); //
		if (GetKeyDown(ControllerAction.L1)) Log.Weikie("Key pressed L1"); //
		if (GetKeyDown(ControllerAction.R1)) Log.Weikie("Key pressed R1"); //
		if (GetKeyDown(ControllerAction.L2)) Log.Weikie("Key pressed L2"); //
		if (GetKeyDown(ControllerAction.R2)) Log.Weikie("Key pressed R2"); //
		if (GetKeyDown(ControllerAction.SELECT)) Log.Weikie("Key pressed SELECT"); //
		if (GetKeyDown(ControllerAction.START)) Log.Weikie("Key pressed START"); //
		if (GetKeyDown(ControllerAction.L3)) Log.Weikie("Key pressed L3"); //
		if (GetKeyDown(ControllerAction.R3)) Log.Weikie("Key pressed R3"); //
		if (GetKeyDown(ControllerAction.A)) Log.Weikie("Key pressed A"); //
		if (GetKeyDown(ControllerAction.B)) Log.Weikie("Key pressed B"); //
		if (GetKeyDown(ControllerAction.X)) Log.Weikie("Key pressed X"); //
		if (GetKeyDown(ControllerAction.Y)) Log.Weikie("Key pressed Y"); //
		if (GetKeyDown(ControllerAction.PLAYSTATION)) Log.Weikie("Key pressed PLAYSTATION"); //
		if (GetKeyDown(ControllerAction.TOUCHPAD_PRESS)) Log.Weikie("Key pressed TOUCHPAD_PRESS"); //
	}

	#endregion
}

public enum ControllerAction
{
	A,
	B,
	X,
	Y,
	START,
	SELECT,
	LEFT_STICK_X,
	LEFT_STICK_Y,
	RIGHT_STICK_X,
	RIGHT_STICK_Y,
	L1,
	L2,
	L3,
	R1,
	R2,
	R3,
	CROSS,
	CIRCLE,
	SQUARE,
	TRIANGLE,
	DPAD_X,
	DPAD_Y,
	PLAYSTATION,
	TOUCHPAD_PRESS
}