using UnityEngine;
using UnityEngine.UI;

public class GooglePlayUIScript : MonoBehaviour
{
    public static GooglePlayUIScript Instance { get; private set; }

    void Start()
    {
        Instance = this;
    }

    private Text pointsTxt;

    public void GetPoint()
    {
        ScoreAchivementManagerScript.Instance.IncrementCounter();
    }

    public void Restart()
    {
        ScoreAchivementManagerScript.Instance.RestartGame();
    }


    public void UnlockHelloWorld()
    {
        GooglePlayGamesScript.UnlockAchievement(GPGSIds.achievement_hello_world__1st_achivement);
    }

    public void IncrementMasterAchivement()
    {
        GooglePlayGamesScript.IncrementAchievement(GPGSIds.achievement_master, 10);
    }

    public void IncrementGodBlessAmerica()
    {
        GooglePlayGamesScript.IncrementAchievement(GPGSIds.achievement_god_bless_america, 10);
    }

    public void UnlockYoureMyStar()
    {
        GooglePlayGamesScript.UnlockAchievement(GPGSIds.achievement_youre_my_star);
    }

    public void UnlockZephyr()
    {
        GooglePlayGamesScript.UnlockAchievement(GPGSIds.achievement_zephyr);
    }

    public void UnlockIndecision()
    {
        GooglePlayGamesScript.UnlockAchievement(GPGSIds.achievement_indecision);
    }

    public void UnlockChampion()
    {
        GooglePlayGamesScript.UnlockAchievement(GPGSIds.achievement_champion);
    }



    public void UnlockExtinction()
    {
        GooglePlayGamesScript.UnlockAchievement(GPGSIds.achievement_extinction);
    }

    public void IncrementLetItBurn()
    {
        GooglePlayGamesScript.IncrementAchievement(GPGSIds.achievement_let_it_burn, 10);
    }

    public void UnlockSupernova()
    {
        GooglePlayGamesScript.UnlockAchievement(GPGSIds.achievement_supernova);
    }

    public void IncrementStepForMan()
    {
        GooglePlayGamesScript.IncrementAchievement(GPGSIds.achievement_little_step_for_man_big_for_humanity, 20);
    }

    public void UnlockSymbiosis()
    {
        GooglePlayGamesScript.UnlockAchievement(GPGSIds.achievement_symbiosis);
    }

    public void IncrementLifeGoodWay()
    {
        GooglePlayGamesScript.IncrementAchievement(GPGSIds.achievement_life_find_always_the_good_way, 10);
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
