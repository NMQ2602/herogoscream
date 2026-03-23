using TMPro;
using UnityEngine;

public class LeaderboardItemUI : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text nameText;
    public TMP_Text scoreText;

    public void SetData(int rank, string name, int score)
    {
        rankText.text = "#" + rank;
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}