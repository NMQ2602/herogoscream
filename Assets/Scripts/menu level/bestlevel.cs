using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuLevelBest : MonoBehaviour
{
    public string levelName;

    public TMP_Text bestText;
    public Slider progressSlider;

    public GameObject[] listsToHide;
    public GameObject[] listsToShow;

    public Button rewardButton; // button chỉ bấm 1 lần

    string claimKey;

    void Start()
    {
        claimKey = "RewardClaimed_" + levelName;

        float bestWin = PlayerPrefs.GetFloat("BestWin_" + levelName, 0f);
        float bestLose = PlayerPrefs.GetFloat("BestLose_" + levelName, 0f);

        float bestPercent = Mathf.Max(bestWin, bestLose);

        // hiển thị %
        if (bestText != null)
            bestText.text = Mathf.RoundToInt(bestPercent) + "%";

        // slider
        if (progressSlider != null)
            progressSlider.value = bestPercent / 100f;

        // chỉ mở khi level đạt 100%
        if (bestPercent >= 100f)
        {
            foreach (GameObject obj in listsToHide)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

            foreach (GameObject obj in listsToShow)
            {
                if (obj != null)
                    obj.SetActive(true);
            }

            // nếu đã bấm rồi thì khóa button
            if (rewardButton != null)
            {
                if (PlayerPrefs.GetInt(claimKey, 0) == 1)
                {
                    rewardButton.interactable = false;
                }
                else
                {
                    rewardButton.interactable = true;
                }
            }
        }
    }

    // gọi hàm này khi bấm button
    public void OnRewardButtonClick()
    {
        PlayerPrefs.SetInt(claimKey, 1);
        PlayerPrefs.Save();

        if (rewardButton != null)
            rewardButton.interactable = false;
    }
}