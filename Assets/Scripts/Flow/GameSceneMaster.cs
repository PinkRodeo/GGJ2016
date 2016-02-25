using UnityEngine;

public class GameSceneMaster : MonoBehaviour
{

	public BeatGUIBar ui;
	public BirdControl[] birds;
	private PoseData[]	lastPose;
	public bool end;
	public BirdControl mistress;
	public Floater floater;

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
			float diff = poseDiff.totalDiff;


			//Log.Weikie("pose difference:" + diff);
			PlayerScoreFeedback(diff, i);
			
			lastPose[i] = currentPose;
		}
		ScoreChanged();
	}

	private void PlayerScoreFeedback(float difference, int playerNumber)
	{
		if (difference < 0.15f)
		{
			floater.SetPulse(playerNumber, Grade.Perfect);
			ScoreHandler.GetInstance().IncrementCombo(playerNumber);

			int score = Mathf.FloorToInt(10 * ScoreHandler.GetInstance().GetComboMultiplier(playerNumber));
			ScoreHandler.GetInstance().AddScore(playerNumber, score * 10);
		}
		else if (difference < 0.3f)
		{
			floater.SetPulse(playerNumber, Grade.Great);
			ScoreHandler.GetInstance().IncrementCombo(playerNumber);

			int score = Mathf.FloorToInt(9 * ScoreHandler.GetInstance().GetComboMultiplier(playerNumber));
			ScoreHandler.GetInstance().AddScore(playerNumber, score * 10);

		}
		else if (difference < .8f)
		{
			int score = Mathf.FloorToInt(8 * ScoreHandler.GetInstance().GetComboMultiplier(playerNumber));
			ScoreHandler.GetInstance().AddScore(playerNumber, score * 10);


			floater.SetPulse(playerNumber, Grade.Good);
			ScoreHandler.GetInstance().IncrementCombo(playerNumber);
		}
		else
		{
			floater.SetPulse(playerNumber, Grade.Bad);
			ScoreHandler.GetInstance().SetComboCount(playerNumber, 0);
		}
	}

	//To be called from BeatGUIBar
	public void HitSubBeat()
	{
		//DoScore();
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
			int amount = Mathf.FloorToInt(poseDiff.totalDiff)*randomScoreModifier;
			ScoreHandler.GetInstance().AddScore(i, amount);

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

	private void ScoreChanged()
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

		if (mistress.input.GetControllerPort() != player + 1)
		{
			mistress.input.SetHackPortNumberModifier(player + 1);
			mistress.input.RedoBindings();
		}
	}

	public void End()
	{
		end = true;
	}
}
