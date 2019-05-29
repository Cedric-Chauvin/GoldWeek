using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GooglePlayGamesScript : MonoBehaviour
{
    [SerializeField]
    private  bool _setup = false;
    [SerializeField]
    public GameObject canvas;
    [SerializeField]
    public  Image _iconGooglePlay1;
    [SerializeField]
    public Image _iconGooglePlay2;
    private  Color defaultColor1;
    private Color defaultColor2;
    public static GooglePlayGamesScript Instance { get; private set; }

    void Start()
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            _iconGooglePlay1 = canvas.transform.GetChild(0).GetComponent<Image>();
            _iconGooglePlay2 = canvas.transform.GetChild(1).GetComponent<Image>();
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            defaultColor1 = _iconGooglePlay1.color;
            _iconGooglePlay1.color = Color.grey;
            defaultColor2 = _iconGooglePlay2.color;
            _iconGooglePlay2.color = Color.grey;
            //SignIn();
        }
        if(PlayGamesPlatform.Instance.IsAuthenticated())
        {
            _iconGooglePlay1 = canvas.transform.GetChild(0).GetComponent<Image>();
            _iconGooglePlay2 = canvas.transform.GetChild(1).GetComponent<Image>();
            _iconGooglePlay1.color = defaultColor1;
            _iconGooglePlay2.color = defaultColor2;
        }
    }


    void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut(); ;
    }

    public void SignInOutButton()
    {
        _setup = !_setup;
        if (!_setup && !PlayGamesPlatform.Instance.IsAuthenticated())
        {
            SignIn();
            _iconGooglePlay1.color = defaultColor1;
            _iconGooglePlay2.color = defaultColor2;
            Debug.LogWarning("log");
        }
        else if (_setup && PlayGamesPlatform.Instance.IsAuthenticated())
        {
            SignOut();
            _iconGooglePlay1.color = Color.grey;
            _iconGooglePlay2.color = Color.grey;
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
