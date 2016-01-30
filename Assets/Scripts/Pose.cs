using UnityEngine;
using System.Collections;

public struct PoseData {
	public Texture2D uiTexture;

	public int phases;

	public float[] leftWing;		//0 to 1
	public float[] rightWing;		//0 to 1

	public Vector2[] head;
	public Vector2[] tail;
}

public class Pose {

	public PoseData data;
	public float phaseLength;

	public float minDiff = 0;
	public float maxDiff = 0;
	public float totalDiff = 0;

	public Pose(PoseData data) {
		this.data = data;
		if (data.phases > 1) {
			phaseLength = 1 / (data.phases - 1);
		} else {
			phaseLength = 1;
		}
	}

	public void CompareWithController(ControllerInput controller, float progress) {
		int firstPhase = (int)Mathf.Floor(progress/phaseLength);
		int secondPhase = firstPhase + 1;

		if (data.phases == 1) {
			secondPhase = 0;
		}

		float subProgress = progress % phaseLength;
		subProgress /= phaseLength;	//normalize it;

		float desiredLeftWing = data.leftWing [firstPhase] + (data.leftWing [secondPhase] - data.leftWing[firstPhase]) * subProgress;
		float desiredRightWing = data.rightWing [firstPhase] + (data.rightWing [secondPhase] - data.rightWing[firstPhase]) * subProgress;

		float desiredHeadX = data.head [firstPhase].x + (data.head [secondPhase].x - data.head[firstPhase].x) * subProgress;
		float desiredHeadY = data.head [firstPhase].y + (data.head [secondPhase].y - data.head[firstPhase].y) * subProgress;

		float desiredTailX = data.tail [firstPhase].x + (data.tail [secondPhase].x - data.tail[firstPhase].x) * subProgress;
		float desiredTailY = data.tail [firstPhase].y + (data.tail [secondPhase].y - data.tail[firstPhase].y) * subProgress;

		totalDiff = 0.0f;
		maxDiff = 0.0f;
		minDiff = 99.0f;

		calculateDiffs(desiredLeftWing, controller.GetAxis(ControllerInput.ControllerAction.L2));
		calculateDiffs(desiredRightWing, controller.GetAxis(ControllerInput.ControllerAction.R2));

		calculateDiffs(desiredHeadX, controller.GetAxis(ControllerInput.ControllerAction.RIGHT_STICK_X));
		calculateDiffs(desiredHeadY, controller.GetAxis(ControllerInput.ControllerAction.RIGHT_STICK_Y));

		calculateDiffs(desiredTailX, controller.GetAxis(ControllerInput.ControllerAction.LEFT_STICK_X));
		calculateDiffs(desiredTailY, controller.GetAxis(ControllerInput.ControllerAction.LEFT_STICK_Y));
	}

	private void calculateDiffs(float a, float b) {
		float diff = Mathf.Abs (a - b);
		minDiff = Mathf.Min (minDiff, diff);
		maxDiff = Mathf.Max (maxDiff, diff);
		totalDiff += diff; 
	}
}