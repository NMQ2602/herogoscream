using GooglePlayGames;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem instance;
    public long score = 0;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        boardManager.Instance.ReportScore(score);
    }
    public void FirstPlay()
    {
        if (PlayerPrefs.GetInt("first_play_done", 0) == 0)
        {
            PlayerPrefs.SetInt("first_play_done", 1);
            Unlock(GPGSIds.achievement_first_play);
        }
    }

    public void AddScream()
    {
        int count = PlayerPrefs.GetInt("scream_count", 0);
        count++;

        PlayerPrefs.SetInt("scream_count", count);

        Debug.Log("Scream count: " + count);

        Increment(GPGSIds.achievement_scream_10, 1);
    }

    // =========================
    public void Unlock(string id)
    {
        if (!IsLogin())
        {
            Debug.Log("❌ Chưa login → không unlock");
            return;
        }

        Social.ReportProgress(id, 100.0f, (success) =>
        {
            Debug.Log("Unlock " + id + ": " + success);
        });
    }

    // =========================
    public void Increment(string id, int step)
    {
        if (!IsLogin())
        {
            Debug.Log("❌ Chưa login → không increment");
            return;
        }

        PlayGamesPlatform.Instance.IncrementAchievement(id, step, (success) =>
        {
            Debug.Log("Increment " + id + ": " + success);
        });
    }

    // =========================
    public void ShowAchievements()
    {
        if (!IsLogin())
        {
            Debug.Log("❌ Chưa login → không mở UI");
            return;
        }

        Social.ShowAchievementsUI();
    }

    // =========================
    bool IsLogin()
    {
        return Social.localUser.authenticated;
    }
}