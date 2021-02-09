using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

public class Leaderboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool auth) => { });
        }catch(Exception e) { Debug.Log("error in initialising leaderboard class - "+e.StackTrace); }
    }

    public static void addScoreToLeaderboard(int score)
    {
        Debug.Log("[addScoreToLeaderboard] about to report score");
        try
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(score, "CgkI-aCz3OcYEAIQEg", success => { Debug.Log("[addScoreToLeaderboard] leaderboard reporting status -" + success); });
            }
        }catch(Exception e) {
            Debug.Log("[addScoreToLeaderboard] " + e.StackTrace);
        }
    }
    public static void showLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            try
            {
                Social.ShowLeaderboardUI();
            }
            catch (Exception e) {
                Debug.Log("[showLeaderboard]" + e.StackTrace);
            }
        }
    }
    public static void showAchievements()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    //mode - 1  - score
    //mode - 2  - boards purchased
    //mode - 3  - number of levels completed
    //
    public static void unlockAchievement(int mode, double value)
    {
        try
        {
            Debug.Log("[unlockAchievement] achivement before auth");
            if (Social.localUser.authenticated)
            {
                Debug.Log("[unlockAchievement] achivement mode - " + mode +", value - "+value);
                if (mode == 1)
                {
                    if(value>=500)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQCA", success => { });//500
                    else if (value >=300)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQBw", success => { });//300
                    else if (value >=200)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQBg", success => { });//200
                    else if (value >= 100)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQBQ", success => { });//100
                    else if (value >= 75)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQBA", success => { });//75
                    else if (value >= 50)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQAw", success => { });//50
                    else if (value >= 25)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQAA", success => { });//25

                }
                else if (mode == 2)
                {
                    if (value >= 5)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQCg", success => { });//5
                    else if (value >= 3)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQCQ", success => { });//3
                    else if (value >= 1)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQAg", success => { });//1

                }
                else if (mode == 3)
                {
                    if (value > 60)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQEA", success => { });//60
                    else if (value > 40)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQDw", success => { });//40
                    else if (value > 30)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQDg", success => { });//30
                    else if (value > 20)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQDQ", success => { });//20
                    else if (value > 10)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQDA", success => { });//10
                    else if (value > 5)
                        PlayGamesPlatform.Instance.UnlockAchievement("CgkI-aCz3OcYEAIQCw", success => { });//5

                }
            }
        }catch(Exception e) {
            Debug.Log("[unlockAchievement] " + e.StackTrace);
        }
    }
}
