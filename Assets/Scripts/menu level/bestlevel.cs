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

    public Button rewardButton;

    string claimKey;
    float bestPercent;

    void Start()
    {
        claimKey = "RewardClaimed_" + levelName;

        float bestWin = PlayerPrefs.GetFloat("BestWin_" + levelName, 0f);
        float bestLose = PlayerPrefs.GetFloat("BestLose_" + levelName, 0f);

        bestPercent = Mathf.Max(bestWin, bestLose);

        if (bestText != null)
            bestText.text = Mathf.RoundToInt(bestPercent) + "%";

        if (progressSlider != null)
            progressSlider.value = bestPercent / 100f;

        if (bestPercent < 100f)
        {
            if (rewardButton != null)
            {
                rewardButton.interactable = false;
                rewardButton.gameObject.SetActive(false);

                CanvasGroup cg = rewardButton.GetComponent<CanvasGroup>();
                if (cg != null)
                    cg.blocksRaycasts = false;
            }

            return;
        }

        foreach (GameObject obj in listsToHide)
            if (obj != null) obj.SetActive(false);

        foreach (GameObject obj in listsToShow)
            if (obj != null) obj.SetActive(true);

        if (rewardButton != null)
        {
            rewardButton.gameObject.SetActive(true);

            CanvasGroup cg = rewardButton.GetComponent<CanvasGroup>();
            if (cg != null)
                cg.blocksRaycasts = true;

            bool claimed = PlayerPrefs.GetInt(claimKey, 0) == 1;

            rewardButton.interactable = !claimed;
        }
    }

    public void OnRewardButtonClick()
    {
        if (bestPercent < 100f)
            return;

        if (PlayerPrefs.GetInt(claimKey, 0) == 1)
            return;

        PlayerPrefs.SetInt(claimKey, 1);
        PlayerPrefs.Save();

        if (rewardButton != null)
            rewardButton.interactable = false;
    }
}