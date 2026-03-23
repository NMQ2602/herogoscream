using UnityEngine;

public class MissionListManager : MonoBehaviour
{
    public static MissionListManager instance;

    public MissionItem[] missions;

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

    public void SetMissions(MissionItem[] newMissions)
    {
        missions = newMissions;

        foreach (var m in missions)
        {
            if (m != null)
                m.RefreshUI();
        }
    }

    void UpdateMission(MissionType type, int value)
    {
        for (int i = 1; i <= 3; i++)
        {
            string prefix = "Mission" + i;

            if (!PlayerPrefs.HasKey(prefix + "_type")) continue;

            MissionType mType = (MissionType)PlayerPrefs.GetInt(prefix + "_type");

            if (mType != type) continue;

            int current = PlayerPrefs.GetInt(prefix + "_current");
            int target = PlayerPrefs.GetInt(prefix + "_target");
            int completed = PlayerPrefs.GetInt(prefix + "_completed");

            if (completed == 1) continue;

            current += value;

            if (current >= target)
            {
                current = target;
                PlayerPrefs.SetInt(prefix + "_completed", 1);
            }

            PlayerPrefs.SetInt(prefix + "_current", current);
        }

        PlayerPrefs.Save();

        // ⭐ nếu đang ở menu thì update UI luôn
        if (missions != null)
        {
            foreach (var m in missions)
            {
                if (m != null)
                    m.RefreshUI();
            }
        }
    }

    public void AddScore(int value)
    {
        UpdateMission(MissionType.Score, value);
    }

    public void AddCherry(int value)
    {
        UpdateMission(MissionType.Cherry, value);
    }

    public void AddJump(int value)
    {
        UpdateMission(MissionType.Jump, value);
    }

    public void AddDistance(int value)
    {
        UpdateMission(MissionType.Distance, value);
    }

    public void WinLevel()
    {
        UpdateMission(MissionType.Win, 1);
    }

    public void CheckRedDot()
    {
        if (missions == null) return;

        foreach (var m in missions)
        {
            if (m == null) continue;

            if (m.IsCompleted() && !m.IsClaimed())
            {
                if (MissionUI.instance != null)
                    MissionUI.instance.ShowRedDot();
                return;
            }
        }

        if (MissionUI.instance != null)
            MissionUI.instance.HideRedDot();
    }
}