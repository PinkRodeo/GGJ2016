using UnityEngine;

struct ScoreEntry
{
	public int score;
	public int prevScore;

	public int latestEarned;

	public int highscore;
}

public class ScoreHandler
{
	//should be struct
	private readonly ScoreEntry[] scoreEntryList;
	private readonly int[] comboTracker;

	private static ScoreHandler instance;

	private const int NumberOfPlayers = 4;

	private ScoreHandler()
	{
		scoreEntryList = new ScoreEntry[NumberOfPlayers];
		comboTracker = new int[NumberOfPlayers];
	}

	public void Reset()
	{
		for (int i = 0; i < NumberOfPlayers; i++)
		{
			scoreEntryList[i].score = 0;
			scoreEntryList[i].prevScore = 0;

			scoreEntryList[i].latestEarned = 0;

			comboTracker[i] = 0;
		}
	}

	public static ScoreHandler GetInstance()
	{
		if (instance == null) instance = new ScoreHandler();
		return instance;
	}

	public void SetComboCount(int playerNumber, int comboCount)
	{
		comboTracker[playerNumber] = comboCount;
	}

	public void IncrementCombo(int playerNumber)
	{
		comboTracker[playerNumber]++;
	}

	public float GetComboMultiplier(int playerNumber)
	{
		float comboMultiplier = 1f;
		int comboCount = GetComboCount(playerNumber);
		float prevComboModAddition = 0.5f;
		for (int n = 0; n < comboCount; ++n)
		{
			prevComboModAddition *= 0.5f;
			comboMultiplier += prevComboModAddition;
		}

		return comboMultiplier;
	}

	public int GetComboCount(int playerNumber)
	{
		var comboCount = Mathf.Min(comboTracker[playerNumber], 4);
		return comboCount;
	}

	public void AddScore(int playerNumber, int amount)
	{
		ScoreEntry entry = scoreEntryList[playerNumber];
		
		entry.prevScore = entry.score;

		entry.score += amount;
		entry.latestEarned = amount;
		
		entry.highscore = Mathf.Max(entry.score, entry.highscore);

		scoreEntryList[playerNumber] = entry;
	}

	public int GetScore(int playerNumber)
	{
		return scoreEntryList[playerNumber].score;
	}

	public void SetScore(int i, int amount)
	{
		ScoreEntry entry = scoreEntryList[i];

		entry.prevScore = entry.score;

		entry.score = amount;
		entry.latestEarned = amount;

		entry.highscore = Mathf.Max(entry.score, entry.highscore);

		scoreEntryList[i] = entry;
	}
}
