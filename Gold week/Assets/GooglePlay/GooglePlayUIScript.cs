using UnityEngine;
using UnityEngine.UI;

public class GooglePlayUIScript : MonoBehaviour
{
    public static GooglePlayUIScript Instance { get; private set; }

    void Start()
    {
        Instance = this;
    }

    [SerializeField]
    private Text pointsTxt;

    public void GetPoint()
    {
        ScoreAchivementManagerScript.Instance.IncrementCounter();
    }

    public void Restart()
    {
        ScoreAchivementManagerScript.Instance.RestartGame();
    }

    public void Increment()
    {
        GooglePlayGamesScript.IncrementAchievement(GPGSIds.achievement_master, 10);
        GetPoint();

    }

    public void Unlock()
    {
        GooglePlayGamesScript.UnlockAchievement(GPGSIds.achievement_hello_world__1st_achivement);
    }

    public void ShowAchievements()
    {
        GooglePlayGamesScript.ShowAchievementsUI();
    }

    public void ShowLeaderboards()
    {
        GooglePlayGamesScript.ShowLeaderboardsUI();
    }

    public void UpdatePointsText()
    {
        pointsTxt.text = ScoreAchivementManagerScript.Counter.ToString();
    }
}
