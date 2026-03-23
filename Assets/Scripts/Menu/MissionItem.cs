using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionItem : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descText;
    public TMP_Text progressText;
    public TMP_Text rewardText;

    public Slider progressBar;

    public GameObject claimButton;
    public GameObject goButton;

    public MissionType missionType;

    int target;
    int current;
    int reward;

    bool completed;
    bool claimed;

    // ===============================
    string GetKey(string key)
    {
        return gameObject.name + "_" + key;
    }

    // ===============================
    public void SetupRandom()
    {
        missionType = (MissionType)Random.Range(0, 5);

        switch (missionType)
        {
            case MissionType.Score:
                target = Random.Range(5, 15);
                reward = 25;
                SetText("Score Master", "Get " + target + " score");
                break;

            case MissionType.Cherry:
                target = Random.Range(3, 10);
                reward = 25;
                SetText("Cherry Hunter", "Collect " + target + " cherries");
                break;

            case MissionType.Jump:
                target = Random.Range(10, 25);
                reward = 30;
                SetText("Jump Pro", "Jump " + target + " times");
                break;

            case MissionType.Distance:
                target = Random.Range(50, 100);
                reward = 35;
                SetText("Runner", "Reach " + target + "%");
                break;

            case MissionType.Win:
                target = 1;
                reward = 50;
                SetText("Winner", "Win the level");
                break;
        }

        current = 0;
        completed = false;
        claimed = false;

        SaveData();
        RefreshUI();
    }

    // ===============================
    void SetText(string title, string desc)
    {
        if (titleText != null) titleText.text = title;
        if (descText != null) descText.text = desc;
    }

    // ===============================
    public void AddProgress(int value)
    {
        if (completed) return;

        current += value;

        if (current >= target)
        {
            current = target;
            completed = true;

            if (MissionListManager.instance != null)
                MissionListManager.instance.CheckRedDot();
        }

        SaveData();
        RefreshUI();
    }

    // ===============================
    public void RefreshUI()
    {
        if (rewardText != null)
            rewardText.text = reward.ToString();

        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = target;
            progressBar.value = current;
        }

        if (progressText != null)
            progressText.text = current + "/" + target;

        if (claimButton != null)
            claimButton.SetActive(completed && !claimed);

        if (goButton != null)
            goButton.SetActive(!completed);
    }

    // ===============================
    public void Claim()
    {
        if (!completed || claimed) return;

        claimed = true;

        if (CoinManager.instance != null)
            CoinManager.instance.AddCoins(reward);

        SaveData();

        if (MissionListManager.instance != null)
            MissionListManager.instance.CheckRedDot();

        SetupRandom(); // tạo nhiệm vụ mới
    }

    // ===============================
    public void LoadData()
    {
        if (!PlayerPrefs.HasKey(GetKey("type")))
        {
            SetupRandom();
            return;
        }

        missionType = (MissionType)PlayerPrefs.GetInt(GetKey("type"));
        target = PlayerPrefs.GetInt(GetKey("target"));
        current = PlayerPrefs.GetInt(GetKey("current"));
        reward = PlayerPrefs.GetInt(GetKey("reward"));
        completed = PlayerPrefs.GetInt(GetKey("completed")) == 1;
        claimed = PlayerPrefs.GetInt(GetKey("claimed")) == 1;

        switch (missionType)
        {
            case MissionType.Score:
                SetText("Score Master", "Get " + target + " score");
                break;
            case MissionType.Cherry:
                SetText("Cherry Hunter", "Collect " + target + " cherries");
                break;
            case MissionType.Jump:
                SetText("Jump Pro", "Jump " + target + " times");
                break;
            case MissionType.Distance:
                SetText("Runner", "Reach " + target + "%");
                break;
            case MissionType.Win:
                SetText("Winner", "Win the level");
                break;
        }

        RefreshUI();
    }

    // ===============================
    void SaveData()
    {
        PlayerPrefs.SetInt(GetKey("type"), (int)missionType);
        PlayerPrefs.SetInt(GetKey("target"), target);
        PlayerPrefs.SetInt(GetKey("current"), current);
        PlayerPrefs.SetInt(GetKey("reward"), reward);
        PlayerPrefs.SetInt(GetKey("completed"), completed ? 1 : 0);
        PlayerPrefs.SetInt(GetKey("claimed"), claimed ? 1 : 0);
        PlayerPrefs.Save();
    }

    // ===============================
    public bool IsCompleted()
    {
        return completed;
    }

    public bool IsClaimed()
    {
        return claimed;
    }
}