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
	public void HitFullBeat()
	{

	}

	//To be called from BeatGUIBar
	public void HitSubBeat()
	{
		for (int i = 0; i < birds.Length; i++)
		{
			PoseData currentPose = Pose.CalculateFromController(birds[i].GetInput());
			PoseData prevPose = lastPose[i];

			//compare
			PoseDiff poseDiff = Pose.CalculatePoseDiffs(currentPose, prevPose);

			int randomScoreModifier = Random.Range(100, 200);
			ScoreHandler.GetInstance().AddScore(i + 1, Mathf.RoundToInt(poseDiff.totalDiff * randomScoreModifier));

			lastPose[i] = currentPose;
		}
	}
}
