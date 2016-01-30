using System.Collections.Generic;

class players
{
    public int id;
    public int score;
    public int highscore;
    public int latestEarned;
}

public class ScoreHandler {
    private players[] pList;
    private List<int> gameScores;
    private static ScoreHandler instance;
    private ScoreHandler()
    {
        gameScores = new List<int>();
        pList = new players[4];
        pList[0] = new players();
        pList[0].id = 1;
        pList[1] = new players();
        pList[1].id = 2;
        pList[2] = new players();
        pList[2].id = 3;
        pList[3] = new players();
        pList[3].id = 4;
    }

    public static ScoreHandler getInstance()
    {
        if (instance == null) instance = new ScoreHandler();
        return instance;
    }

    public void addScore(int playerID, int amount)
    {
        pList[playerID-1].score += amount;
        pList[playerID-1].latestEarned = amount;
    }

    public int getScore(int playerID)
    {
        return pList[playerID - 1].score;
    }

    public void generateTotalGameScore()
    {
        int score = 0;
        score += pList[0].score;
        score += pList[1].score;
        score += pList[2].score;
        score += pList[3].score;
        gameScores.Add(score);
    }

}
