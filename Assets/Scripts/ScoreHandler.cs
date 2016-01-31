using System.Collections.Generic;

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
		ScoreEntry entry = scoreEntryList[playerNumber - 1];
		entry.score += amount;
		entry.latestEarned = amount;
		scoreEntryList[playerNumber - 1] = entry;
	}

	public int GetScore(int playerNumber)
	{
		return scoreEntryList[playerNumber - 1].score;
	}

	public void generateTotalGameScore()
	{
		int score = 0;
		score += scoreEntryList[0].score;
		score += scoreEntryList[1].score;
		score += scoreEntryList[2].score;
		score += scoreEntryList[3].score;
		gameScores.Add(score);
	}

}
