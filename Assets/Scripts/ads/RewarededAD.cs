using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinRewardButton : MonoBehaviour
{
    public Button rewardButton;
    public int coinAmount = 20;   
    public bool oneTimeOnly = true;

    private bool used = false;
    private bool adWatched = false;

    void Start()
    {
        if (rewardButton)
            rewardButton.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        if (used && oneTimeOnly)
        {
            Debug.Log("Đã nhận rồi!");
            return;
        }

        if (!adWatched)
        {
            ShowRewardedAd();
            return;
        }

        GrantCoins();
    }

    // =============================
    // HIỂN THỊ QUẢNG CÁO
    // =============================
    void ShowRewardedAd()
    {
        var ads = FindObjectOfType<GoogleAdsManager>();

        if (ads != null)
        {
            ads.RewardedEndEvent += OnAdFinished;
            ads.ShowRewardedAd();
        }
        else
        {
            // Fake quảng cáo khi test trong Editor
            StartCoroutine(FakeAdRoutine());
        }
    }

    IEnumerator FakeAdRoutine()
    {
        yield return new WaitForSecondsRealtime(2f);
        OnAdFinished();
    }

    // =============================
    // XEM XONG QUẢNG CÁO
    // =============================
    void OnAdFinished()
    {
        adWatched = true;
        GrantCoins();

        var ads = FindObjectOfType<GoogleAdsManager>();
        if (ads != null)
            ads.RewardedEndEvent -= OnAdFinished;
    }

    // =============================
    // NHẬN TIỀN 💰
    // =============================
    void GrantCoins()
    {
        CoinManager.instance.AddCoins(coinAmount);

        Debug.Log($"Nhận {coinAmount} coin!");

        used = true;

        if (oneTimeOnly && rewardButton)
            rewardButton.gameObject.SetActive(false);
    }
}