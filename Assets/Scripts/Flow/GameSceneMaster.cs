using UnityEngine;
using System.Collections;

public class GameSceneMaster : MonoBehaviour
{

	public BeatGUIBar ui;
	public BirdControl[] birds;
	private PoseData[]	lastPose;

	void Start ()
	{
		lastPose = new PoseData[birds.Length];
	}

	void Update ()
	{

	}

	public void InitBirdControls()
	{
		int count = ControllerInput.GetConnectedControllersCount();
		for (int i = 0; i < count; i++)
		{
			birds[i]._initializeController();
		}
		for (int i = count; i < birds.Length; i++)
		{
			birds[i].transform.position += Vector3.up*200;
		}
	}

	//To be called from BeatGUIBar
	public void HitFullBeat(Pose data)
	{
		for (int i = 0; i < birds.Length; i++)
		{
			if (!birds[i].IsInitialized()) continue;

			ControllerInput input = birds[i].GetInput();
			PoseData currentPose = Pose.CalculateFromController(input);

			//compare
			PoseDiff poseDiff = data.CompareWithController(input, 0);

			int randomScoreModifier = Random.Range(30, 50);
			ScoreHandler.GetInstance().AddScore(i + 1, Mathf.FloorToInt(poseDiff.totalDiff) * randomScoreModifier);

			lastPose[i] = currentPose;
		}
	}

	//To be called from BeatGUIBar
	public void HitSubBeat()
	{
		DoScore();
	}

	private void DoScore()
	{
		for (int i = 0; i < birds.Length; i++)
		{
			if (!birds[i].IsInitialized()) continue;

			PoseData currentPose = Pose.CalculateFromController(birds[i].GetInput());
			PoseData prevPose = lastPose[i];

			//compare
			PoseDiff poseDiff = Pose.CalculatePoseDiffs(currentPose, prevPose);

			int randomScoreModifier = Random.Range(10, 40);
			ScoreHandler.GetInstance().AddScore(i + 1, Mathf.FloorToInt(poseDiff.totalDiff)*randomScoreModifier);

			lastPose[i] = currentPose;
		}
	}
}
