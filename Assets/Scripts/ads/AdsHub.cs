using UnityEngine;

public class AdsHub : MonoBehaviour
{
    public static AdsHub Instance;

    public GoogleAdsManager google;
    public UnityAdsManager unity;

    void Awake()
    {
        Instance = this;
    }

    // ======================================================
    // APP OPEN
    // ======================================================
    public void ShowAppOpen()
    {
        if (google != null && google.IsAppOpenReady())
        {
            google.ShowAppOpenAd();
        }
        else
        {
            unity.ShowAppOpen();
        }
    }


    // ======================================================
    // BANNER
    // ======================================================
    public void ShowBanner()
    {
        if (google != null)
        {
            google.LoadBannerAd();
        }
        else
        {
            unity.ShowBanner();
        }
    }

    public void HideBanner()
    {
        if (google != null)
        {
            google.DestroyBannerView();
        }
        else
        {
            unity.HideBanner();
        }
    }

    // ======================================================
    // INTERSTITIAL
    // ======================================================
    public void ShowInterstitial()
    {
        if (google != null && google.IsInterstitialAdReady())
        {
            google.ShowInterstitialAd();
            return;
        }

        if (unity != null && unity.IsInterReady())
        {
            unity.ShowInterstitial();
            return;
        }

        Debug.LogWarning("Không có interstitial nào sẵn sàng!");
    }


    // ======================================================
    // REWARDED
    // ======================================================
    public void ShowRewarded()
    {
        if (google != null && google.IsRewardedReady())
        {
            google.ShowRewardedAd();
            return;
        }

        if (unity != null && unity.IsRewardedReady())
        {
            unity.ShowRewarded();
            return;
        }

        Debug.LogWarning("Không có rewarded nào sẵn sàng!");
    }

}
