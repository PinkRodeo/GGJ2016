using UnityEngine;
using System.Collections;

public struct PoseData
{
	public float leftWing;		//0 to 1
	public float rightWing;		//0 to 1

	public Vector2 head;
	public Vector2 tail;
};

public struct PoseList
{
	public PoseData[] poses;
	public int count;
	public Sprite uiTexture;
};

public struct PoseDiff
{
	public float minDiff;
	public float maxDiff;
	public float totalDiff;
};

public class Pose
{

	public PoseList data;
	public float phaseLength;

	public Pose(PoseList data)
	{
		this.data = data;
		if (data.count > 1)
		{
			phaseLength = 1 / (data.count - 1);
		}
		else
		{
			phaseLength = 1;
		}
	}

	public PoseDiff CompareWithController(ControllerInput controller, float progress)
	{
		int firstPhase = (int)Mathf.Floor(progress/phaseLength);
		int secondPhase = firstPhase + 1;

		if (data.count == 1)
		{
			secondPhase = 0;
		}

		PoseData firstPose = data.poses [firstPhase];
		PoseData secondPose = data.poses [secondPhase];

		PoseData desiredPose = new PoseData();

		float subProgress = progress % phaseLength;
		subProgress /= phaseLength;	//normalize it;

		desiredPose.leftWing = firstPose.leftWing + (secondPose.leftWing - firstPose.leftWing) * subProgress;
		desiredPose.rightWing = firstPose.rightWing + (secondPose.rightWing - firstPose.rightWing) * subProgress;

		desiredPose.head.x = firstPose.head.x + (secondPose.head.x - firstPose.head.x) * subProgress;
		desiredPose.head.y = firstPose.head.y + (secondPose.head.y - firstPose.head.y) * subProgress;

		desiredPose.tail.x = firstPose.tail.x + (secondPose.tail.x - firstPose.tail.x) * subProgress;
		desiredPose.head.y = firstPose.tail.y + (secondPose.tail.y - firstPose.tail.y) * subProgress;

		PoseData controllerPose = CalculateFromController(controller);

		return CalculatePoseDiffs (desiredPose, controllerPose);
	}

	private static PoseDiff CalculateDiffs(float a, float b, PoseDiff diff)
	{
		float dd = Mathf.Abs (a - b);
		PoseDiff result;
		result.minDiff = Mathf.Min (diff.minDiff, dd);
		result.maxDiff = Mathf.Max (diff.maxDiff, dd);
		result.totalDiff = diff.totalDiff + dd;

		return result;
	}

	public static PoseDiff CalculatePoseDiffs(PoseData a, PoseData b)
	{
		PoseDiff result = new PoseDiff ();

		result = CalculateDiffs (a.leftWing, b.leftWing, result);
		result = CalculateDiffs (a.rightWing, b.rightWing, result);
		result = CalculateDiffs (a.head.x, b.head.x, result);
		result = CalculateDiffs (a.head.y, b.head.y, result);
		result = CalculateDiffs (a.tail.x, b.tail.x, result);
		result = CalculateDiffs (a.tail.y, b.tail.y, result);

		return result;
	}

	public static PoseData CalculateFromController(ControllerInput controller)
	{
		PoseData result = new PoseData ();

		result.leftWing = controller.GetAxis (ControllerAction.L2);
		result.rightWing = controller.GetAxis (ControllerAction.R2);
		result.head.x = controller.GetAxis (ControllerAction.LEFT_STICK_X);
		result.head.y = controller.GetAxis (ControllerAction.LEFT_STICK_X);
		result.tail.x = controller.GetAxis (ControllerAction.RIGHT_STICK_X);
		result.tail.y = controller.GetAxis (ControllerAction.RIGHT_STICK_Y);

		return result;
	}

}