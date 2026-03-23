using UnityEngine;
using Firebase.Database;
using Firebase.Extensions; // 🔥 QUAN TRỌNG
using System.Collections.Generic;
using System.Linq;

public class LeaderboardLoader : MonoBehaviour
{
    public Transform content;
    public GameObject itemPrefab;

    void Start()
    {
        FirebaseDatabase
            .GetInstance("https://screamgohero-17537-default-rtdb.firebaseio.com/")
            .GetReference("leaderboard")
            .OrderByChild("score")
            .LimitToLast(10)
            .GetValueAsync()
            .ContinueWithOnMainThread(task => // 🔥 FIX
            {
                if (task.IsCompleted)
                {
                    Debug.Log("🔥 Load OK");

                    List<(string name, int score)> list = new List<(string, int)>();

                    foreach (var child in task.Result.Children)
                    {
                        if (child.Child("name").Value == null) continue;
                        if (child.Child("score").Value == null) continue;

                        string name = child.Child("name").Value.ToString();
                        int score = int.Parse(child.Child("score").Value.ToString());

                        list.Add((name, score));
                    }

                    list = list.OrderByDescending(x => x.score).ToList();

                    for (int i = 0; i < list.Count; i++)
                    {
                        GameObject obj = Instantiate(itemPrefab, content);
                        obj.SetActive(true);

                        obj.GetComponent<LeaderboardItemUI>()
                           .SetData(i + 1, list[i].name, list[i].score);
                    }

                    Debug.Log("✅ Tổng item: " + list.Count);
                }
                else
                {
                    Debug.LogError("❌ Load FAIL: " + task.Exception);
                }
            });
    }
}