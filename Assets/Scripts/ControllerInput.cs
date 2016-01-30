using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ControllerInput : MonoBehaviour
{
	public enum CurrentPlatform
	{
		Unknown,
		Windows,
		Mac
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
		DPAD_Y
	}

	private Dictionary<ControllerAction, List<KeyCode>> buttonKeymap = new Dictionary<ControllerAction, List<KeyCode>>();
	private Dictionary<ControllerAction, List<string>> axisKeymap = new Dictionary<ControllerAction, List<string>>();
	private CurrentPlatform currentPlatform = CurrentPlatform.Unknown;

	#region Public accessors

	public bool GetKeyDown(ControllerAction action)
	{
		if (!IsActionMapped(action)) return false;

		List<KeyCode> keycodeList = buttonKeymap[action];
		return keycodeList.Any(Input.GetKeyDown);
	}

	public bool GetKey(ControllerAction action)
	{
		if (!IsActionMapped(action)) return false;

		List<KeyCode> keycodeList = buttonKeymap[action];
		return keycodeList.Any(Input.GetKey);
	}

	public bool GetKeyUp(ControllerAction action)
	{
		if (!IsActionMapped(action)) return false;

		List<KeyCode> keycodeList = buttonKeymap[action];
		return keycodeList.Any(Input.GetKeyUp);
	}

	public float GetAxis(ControllerAction action)
	{
		//if (!IsActionMapped(action)) return 0;

		List<string> keycodeList = axisKeymap[action];
		foreach (var s in keycodeList)
		{
			float f = Input.GetAxis(s);
			if (Mathf.Abs(Input.GetAxis(s)) > Mathf.Epsilon)
			{
				return f;
			}
		}
		return 0;
	}

	#endregion

	#region Private stuff

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
			buttonKeymap.TryGetValue(action, out list);
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
			axisKeymap.TryGetValue(action, out list);
		}

		list.Add(axisName);
		axisKeymap[action] = list;
	}

	private void PlaystationBinding()
	{
		AddButtonBinding(ControllerAction.SQUARE, KeyCode.Joystick1Button0);
		AddButtonBinding(ControllerAction.CROSS, KeyCode.Joystick1Button1);
		AddButtonBinding(ControllerAction.CIRCLE, KeyCode.Joystick1Button2);
		AddButtonBinding(ControllerAction.TRIANGLE, KeyCode.Joystick1Button3);
		AddButtonBinding(ControllerAction.L1, KeyCode.Joystick1Button4);
		AddButtonBinding(ControllerAction.R1, KeyCode.Joystick1Button5);
		AddButtonBinding(ControllerAction.L2, KeyCode.Joystick1Button6);
		AddButtonBinding(ControllerAction.R2, KeyCode.Joystick1Button7);
		AddButtonBinding(ControllerAction.SELECT, KeyCode.Joystick1Button8);
		AddButtonBinding(ControllerAction.START, KeyCode.Joystick1Button9);
		AddButtonBinding(ControllerAction.L3, KeyCode.Joystick1Button10);
		AddButtonBinding(ControllerAction.R3, KeyCode.Joystick1Button11);

		//if (currentPlatform == CurrentPlatform.Windows)
		{
			AddAxisBinding(ControllerAction.LEFT_STICK_X,	"Controller1Axis1");
			AddAxisBinding(ControllerAction.LEFT_STICK_Y,	"Controller1Axis2");
			AddAxisBinding(ControllerAction.RIGHT_STICK_X,	"Controller1Axis3");
			AddAxisBinding(ControllerAction.RIGHT_STICK_Y,	"Controller1Axis4");
			AddAxisBinding(ControllerAction.L2,				"Controller1Axis5");
			AddAxisBinding(ControllerAction.R2,				"Controller1Axis6");
			AddAxisBinding(ControllerAction.DPAD_X,			"Controller1Axis7");
			AddAxisBinding(ControllerAction.DPAD_Y,			"Controller1Axis8");
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
		AddButtonBinding(ControllerAction.A,		KeyCode.Joystick1Button0);
		AddButtonBinding(ControllerAction.B,		KeyCode.Joystick1Button1);
		AddButtonBinding(ControllerAction.X,		KeyCode.Joystick1Button2);
		AddButtonBinding(ControllerAction.Y,		KeyCode.Joystick1Button3);
		AddButtonBinding(ControllerAction.L1,		KeyCode.Joystick1Button4);
		AddButtonBinding(ControllerAction.R1,		KeyCode.Joystick1Button5);
		AddButtonBinding(ControllerAction.SELECT,	KeyCode.Joystick1Button6);
		AddButtonBinding(ControllerAction.START,	KeyCode.Joystick1Button7);
		AddButtonBinding(ControllerAction.L3,		KeyCode.Joystick1Button8);
		AddButtonBinding(ControllerAction.R3,		KeyCode.Joystick1Button9);

		//if (currentPlatform == CurrentPlatform.Windows)
		{
			AddAxisBinding(ControllerAction.LEFT_STICK_X,	"Controller1Axis1");
			AddAxisBinding(ControllerAction.LEFT_STICK_Y,	"Controller1Axis2");
			AddAxisBinding(ControllerAction.RIGHT_STICK_X,	"Controller1Axis4");
			AddAxisBinding(ControllerAction.RIGHT_STICK_Y,	"Controller1Axis5");
			AddAxisBinding(ControllerAction.DPAD_X,			"Controller1Axis6");
			AddAxisBinding(ControllerAction.DPAD_Y,			"Controller1Axis7");
			AddAxisBinding(ControllerAction.L2,				"Controller1Axis9");
			AddAxisBinding(ControllerAction.R2,				"Controller1Axis10");
		}
	}

	private void Start()
	{

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		currentPlatform = CurrentPlatform.Windows;
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
		currentPlatform = CurrentPlatform.Mac;
#endif

		//PlaystationBinding();
		XboxBindings();
	}

	private void Update()
	{
		ButtonTestPrints();
		AxisTestPrints();
		//float a = Input.GetAxis("X-Axis");
		//float b = Input.GetAxis("Y-Axis");
		float a = Input.GetAxis("Controller1Axis3");
		float b = Input.GetAxis("Controller1Axis2"); //should probably invert

		//Log.Weikie("X axis " + a);
		//Log.Weikie("Y axis " + b);
		//UNITY_STANDALONE_OSX WIN LINUX
		//UNITY_EDITOR_OSX WIN LINUX

	}

	private void AxisTestPrints()
	{
		float f;

		if (Mathf.Abs(f = GetAxis(ControllerAction.LEFT_STICK_X)) > 0.1f)	Log.Weikie("LEFT_STICK_X " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.LEFT_STICK_Y)) > 0.1f)	Log.Weikie("LEFT_STICK_Y " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.RIGHT_STICK_X)) > 0.1f)	Log.Weikie("RIGHT_STICK_X " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.RIGHT_STICK_Y)) > 0.1f)	Log.Weikie("RIGHT_STICK_Y " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.DPAD_X)) > 0.1f)			Log.Weikie("DPAD_X " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.DPAD_Y)) > 0.1f)			Log.Weikie("DPAD_Y " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.L2)) > 0.1f)				Log.Weikie("L2 " + f);
		if (Mathf.Abs(f = GetAxis(ControllerAction.R2)) > 0.1f)				Log.Weikie("R2 " + f);
	}

	private void ButtonTestPrints()
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
	}

	#endregion
}