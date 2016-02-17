using UnityEngine;
using System.Collections;

public class GameSceneMaster : MonoBehaviour
{

	public BeatGUIBar ui;
	public BirdControl[] birds;
	private PoseData[]	lastPose;
	public bool end;
	public BirdControl mistress;

	void Start ()
	{
		lastPose = new PoseData[birds.Length];
		mistress = GameObject.Find("BalconyBird").GetComponent<BirdControl>();
		mistress._initializeController();
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

			int randomScoreModifier = Random.Range(10, 15);
			ScoreHandler.GetInstance().AddScore(i, Mathf.FloorToInt(poseDiff.totalDiff) * randomScoreModifier);

			lastPose[i] = currentPose;
		}
		ScoreChanged();
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

			int randomScoreModifier = Random.Range(5, 10);
			ScoreHandler.GetInstance().AddScore(i, Mathf.FloorToInt(poseDiff.totalDiff)*randomScoreModifier);

			lastPose[i] = currentPose;
		}
		ScoreChanged();
	}

	public void ExitStage()
	{
		for (int i = 0; i < birds.Length; i++)
		{
			ScoreHandler.GetInstance().SetScore(i, 0);
		}
	}

	public void ScoreChanged()
	{
		int highestScore = 0;
		int player = 0;

		for (int i = 0; i < birds.Length; i++)
		{
			int score = ScoreHandler.GetInstance().GetScore(i);
			if (score > highestScore)
			{
				highestScore = score;
				player = i;
			}
		}

		mistress.input.SetHackPortNumberModifier(player + 1);
		mistress.input.RedoBindings();
	}

	public void End()
	{
		end = true;
	}
}
