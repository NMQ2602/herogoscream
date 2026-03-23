using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    private DatabaseReference db;
    private bool isReady = false;

    void Awake()
    {
        Debug.Log("🔥 Awake LeaderboardManager");

        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitFirebase();
    }

    void InitFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("🔥 Firebase init...");

                db = FirebaseDatabase
                     .GetInstance("https://screamgohero-17537-default-rtdb.firebaseio.com/")
                     .RootReference;

                isReady = true;

                Debug.Log("✅ Firebase READY");
            }
            else
            {
                Debug.LogError("❌ Firebase FAIL: " + task.Result);
            }
        });
    }

    public void SubmitScore(string name, int score)
    {
        Debug.Log("🚀 Gọi SubmitScore");

        StartCoroutine(WaitAndSubmit(name, score));
    }

    IEnumerator WaitAndSubmit(string name, int score)
    {
        while (!isReady)
        {
            Debug.Log("⏳ Đợi Firebase...");
            yield return null;
        }

        Debug.Log("🔥 Firebase OK → gửi");

        string key = db.Child("leaderboard").Push().Key;

        LeaderboardEntry entry = new LeaderboardEntry(name, score);

        string json = JsonUtility.ToJson(entry);

        var task = db.Child("leaderboard").Child(key).SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("❌ Lỗi gửi: " + task.Exception);
        }
        else
        {
            Debug.Log("✅ GỬI THÀNH CÔNG: " + name + " - " + score);
        }
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;

    public LeaderboardEntry(string n, int s)
    {
        name = n;
        score = s;
    }
}