using UnityEngine;

struct ScoreEntry
{
	public int score;
	public int highscore;
	public int latestEarned;
}

public class ScoreHandler
{
	//should be struct
	private readonly ScoreEntry[] scoreEntryList;
	private int[] comboTracker;

	private static ScoreHandler instance;

	private ScoreHandler()
	{
		scoreEntryList = new ScoreEntry[4];
		comboTracker = new int[4];
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
		entry.score += amount;
		entry.latestEarned = amount;
		scoreEntryList[playerNumber] = entry;
	}

	public int GetScore(int playerNumber)
	{
		return scoreEntryList[playerNumber].score;
	}

	public void SetScore(int i, int amount)
	{
		ScoreEntry entry = scoreEntryList[i];
		entry.score = amount;
		entry.latestEarned = amount;
		scoreEntryList[i] = entry;
	}
}
