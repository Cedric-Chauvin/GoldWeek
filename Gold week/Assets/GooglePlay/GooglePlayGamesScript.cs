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
    public  Image _iconGooglePlay;
    private  Color defaultColor;
    public static GooglePlayGamesScript Instance { get; private set; }

    void Start()
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            _iconGooglePlay = canvas.transform.GetChild(0).GetComponent<Image>();
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            defaultColor = _iconGooglePlay.color;
            _iconGooglePlay.color = Color.grey;
            //SignIn();
        }
        if(PlayGamesPlatform.Instance.IsAuthenticated())
        {
            _iconGooglePlay = canvas.transform.GetChild(0).GetComponent<Image>();
            defaultColor = _iconGooglePlay.color;
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
            _iconGooglePlay.color = defaultColor;
            Debug.LogWarning("log");
        }
        else if (_setup && PlayGamesPlatform.Instance.IsAuthenticated())
        {
            SignOut();
            _iconGooglePlay.color = Color.grey;
            Debug.LogWarning("Disc");
        }
    }

    public void Update()
    {
        if (_iconGooglePlay == null && SceneManager.GetActiveScene().name == "Menu")
        {
            _iconGooglePlay = canvas.transform.GetChild(0).GetComponent<Image>();
        }
        if (Input.GetKey(KeyCode.Keypad1))
        {
            SceneManager.LoadScene("Menu");
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            SceneManager.LoadScene("Cedric");
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
