using UnityEngine;

public class ScoreAchivementManagerScript : MonoBehaviour
{
    public static ScoreAchivementManagerScript Instance { get; private set; }
    public static int Counter { get; private set; }

    void Start()
    {
        Instance = this;
    }

    public void IncrementCounter()
    {
        Counter++;
        GooglePlayUIScript.Instance.UpdatePointsText();
    }

    public void RestartGame()
    {
        GooglePlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_leaderboardnametest, Counter);
        Counter = 0;
        GooglePlayUIScript.Instance.UpdatePointsText();
    }

}

