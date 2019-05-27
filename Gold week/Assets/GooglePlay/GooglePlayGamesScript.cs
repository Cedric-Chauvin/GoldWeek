using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;

public class GooglePlayGamesScript : MonoBehaviour
{

    private bool _setup = false;
    [SerializeField]
    public Image _iconGooglePlay;
    private Color defaultColor;

    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        defaultColor = _iconGooglePlay.color;
        _iconGooglePlay.color = Color.grey;
        SignIn();
    }


    void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut(); ;
    }

    public void SignInOutButton()
    {
        _setup = !_setup;
        if(!_setup && !PlayGamesPlatform.Instance.IsAuthenticated())
        {
            SignIn();
            _iconGooglePlay.color = defaultColor;
            Debug.LogWarning("log");
        }
        else if(_setup && PlayGamesPlatform.Instance.IsAuthenticated())
        {
            SignOut();
            _iconGooglePlay.color = Color.grey;
            Debug.LogWarning("Disc");
        }
    }



    void SignIn()
    {
        Social.localUser.Authenticate(success => { });
    }

    #region Achievements
    public static void UnlockAchievement(string id)
    {
        Social.ReportProgress(id, 100, success => { });
    }

    public static void IncrementAchievement(string id, int stepsToIncrement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
    }

    public static void ShowAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }
    #endregion /Achievements

    #region Leaderboards
    public static void AddScoreToLeaderboard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, success => { });
    }

    public static void ShowLeaderboardsUI()
    {
        Social.ShowLeaderboardUI();
    }
    #endregion /Leaderboards

}
