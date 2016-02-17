using System.Collections.Generic;
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
	private readonly List<int> gameScores;
	private static ScoreHandler instance;

	private ScoreHandler()
	{
		gameScores = new List<int>();
		scoreEntryList = new ScoreEntry[4];
	}

	public static ScoreHandler GetInstance()
	{
		if (instance == null) instance = new ScoreHandler();
		return instance;
	}

	public void AddScore(int playerNumber, int amount)
	{
		Floater myfloat = GameObject.Find("Canvas").GetComponent<Floater>();
		if (amount > 100)
		{
			//WEIKIE: Whats this? should probably not even be here
			//myfloat.SpawnFloater(amount + "", playerNumber + 4, Color.cyan);
		}
		ScoreEntry entry = scoreEntryList[playerNumber - 1];
		entry.score += amount;
		entry.latestEarned = amount;
		scoreEntryList[playerNumber - 1] = entry;
	}

	public int GetScore(int playerNumber)
	{
		return scoreEntryList[playerNumber - 1].score;
	}

	public void SetScore(int i, int amount)
	{
		ScoreEntry entry = scoreEntryList[i];
		entry.score = amount;
		entry.latestEarned = amount;
		scoreEntryList[i] = entry;
	}
}
