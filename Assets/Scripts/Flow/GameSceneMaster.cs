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

	//To be called from BeatGUIBar
	public void HitFullBeat(Pose data)
	{
		for (int i = 0; i < birds.Length; i++)
		{
			var input = birds[i].GetInput();
			PoseData currentPose = Pose.CalculateFromController(input);

			//compare
			PoseDiff poseDiff = data.CompareWithController(input, 0);

			int randomScoreModifier = Random.Range(80, 120);
			ScoreHandler.GetInstance().AddScore(i + 1, Mathf.FloorToInt(poseDiff.totalDiff) * randomScoreModifier);

			lastPose[i] = currentPose;
			Log.Weikie("full beat");
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
			PoseData currentPose = Pose.CalculateFromController(birds[i].GetInput());
			PoseData prevPose = lastPose[i];

			//compare
			PoseDiff poseDiff = Pose.CalculatePoseDiffs(currentPose, prevPose);

			int randomScoreModifier = Random.Range(20, 100);
			ScoreHandler.GetInstance().AddScore(i + 1, Mathf.FloorToInt(poseDiff.totalDiff)*randomScoreModifier);

			lastPose[i] = currentPose;
		}
	}
}
