using UnityEngine;

struct ScoreEntry
{
	public int score;
	public int highscore;
	public int latestEarned;
}

public class ScoreHandler
{
	private readonly ScoreEntry[] scoreEntryList;
	private static ScoreHandler instance;

	private ScoreHandler()
	{
		scoreEntryList = new ScoreEntry[4];
	}

	public static ScoreHandler GetInstance()
	{
		if (instance == null) instance = new ScoreHandler();
		return instance;
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
