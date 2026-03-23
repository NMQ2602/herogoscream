using UnityEngine;
using TMPro;

public class SubmitScoreUI : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void Submit()
    {
        int score = GameManager_Endless.instance.GetScore();

        string playerName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            int id = PlayerPrefs.GetInt("PlayerID", 1);

            playerName = "Player_" + id;

            PlayerPrefs.SetInt("PlayerID", id + 1);
            PlayerPrefs.Save();
        }

        LeaderboardManager.instance.SubmitScore(playerName, score);

        gameObject.SetActive(false);
    }
}