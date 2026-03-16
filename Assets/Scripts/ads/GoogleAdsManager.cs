using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;

public class GoogleAdsManager : MonoBehaviour
{
    public static GoogleAdsManager Instance;

    // ====================
    // APP OPEN ADS
    // ====================
    [Header("App Ads")]
    public string appOpenId; // Test ID
    private AppOpenAd appOpenAd;
    private bool isShowingAppOpen = false;
    /// <summary>
    /// /////////////////////////
    /// </summary>

    [SerializeField]
    private string bannerId;
    [SerializeField]
    private string interId;
    [SerializeField]
    private string rewardedId;

    [Header("Banner Settings")]
    public AdPosition bannerPosition = AdPosition.Bottom; // ✅ chỉ giữ lại vị trí banner

    BannerView _bannerView;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;

    [Header("Ads Events For Game")]
    public UnityAction interRewardEvent;
    public UnityAction RewardedEndEvent;

    //private bool hasEarnedReward = false;
    // ============================
    // MREC
    // ============================
    [SerializeField] private string mrecId;
    private BannerView _mrecView;

    [Header("MREC Background (optional)")]
    public GameObject mrecBg;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        if (IsNoAdsEnabled())
        {
            Debug.Log("No Ads active — không load quảng cáo.");
            return;
        }

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            LoadBannerAd();
            LoadInterstitialAd();
            interRewardEvent += () => GiveInterReward();

            LoadRewardedAd();
            RewardedEndEvent += () => GiveRewarededReward();
        });
        LoadAppOpenAd();////////////////////
        //LoadMRECAd();

    }

    // Kiểm tra trạng thái No Ads
    private bool IsNoAdsEnabled()
    {
        return PlayerPrefs.GetInt("noads", 0) == 1;
    }










    /// <summary>
    #region APP OPEN ADS
    public void LoadAppOpenAd()
    {
        if (IsNoAdsEnabled()) return;

        Debug.Log("Loading App Open Ad...");
        var request = new AdRequest();

        AppOpenAd.Load(appOpenId, request, (AppOpenAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("App Open failed to load: " + error);
                return;
            }

            appOpenAd = ad;
            RegisterAppOpenEvents();
            Debug.Log("App Open Loaded");
        });
    }

    public void ShowAppOpenAd()
    {
        if (IsNoAdsEnabled()) return;

        if (appOpenAd == null || isShowingAppOpen)
        {
            Debug.Log("App Open chưa sẵn sàng.");
            return;
        }

        Debug.Log("Showing App Open Ad...");
        isShowingAppOpen = true;

        appOpenAd.Show();

    }

    // Event
    private void RegisterAppOpenEvents()
    {
        appOpenAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App Open Closed");
            isShowingAppOpen = false;
            LoadAppOpenAd(); // reload lại
        };

        appOpenAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App Open Failed: " + error);
            isShowingAppOpen = false;
            LoadAppOpenAd();
        };
    }
    #endregion

    /// </summary>











    #region Banner Ads

    public void LoadBannerAd()
    {
        if (IsNoAdsEnabled()) return;

        if (_bannerView == null)
        {
            CreateBannerView();
        }

        ListenToBannerAdEvents();

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    private void CreateBannerView()
    {
        if (_bannerView != null)
        {
            DestroyBannerView();
        }

        // ✅ Banner adaptive co theo màn hình (ổn định nhất)
        int screenWidth = Screen.width;
        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(screenWidth);

        _bannerView = new BannerView(bannerId, adaptiveSize, bannerPosition);
        Debug.Log($"[AdsManager] Banner tạo với kích thước {adaptiveSize.Width}x{adaptiveSize.Height} tại {bannerPosition}");
    }

    public void DestroyBannerView()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    private void ListenToBannerAdEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : " + _bannerView.GetResponseInfo());
        };
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : " + error);
        };
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    #endregion

    #region Interstitial Ads
    private void LoadInterstitialAd()
    {
        if (IsNoAdsEnabled()) return;

        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");
        var adRequest = new AdRequest();

        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("interstitial ad failed to load an ad with error : " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
            _interstitialAd = ad;
            InterstitialEvent(_interstitialAd);
            InterstitialReloadHandler(_interstitialAd);
        });
    }

    public void ShowInterstitialAd()
    {
        if (IsNoAdsEnabled()) return;

        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            LoadInterstitialAd();
        }
    }

    private void GiveInterReward()
    {
        Debug.Log("Inter Ads Reward Given");
    }

    public bool IsInterstitialAdReady()
    {
        return _interstitialAd != null && _interstitialAd.CanShowAd();
    }

    private void InterstitialEvent(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
            interRewardEvent?.Invoke();
        };
        interstitialAd.OnAdImpressionRecorded += () => Debug.Log("Interstitial ad recorded an impression.");
        interstitialAd.OnAdClicked += () => Debug.Log("Interstitial ad was clicked.");
        interstitialAd.OnAdFullScreenContentOpened += () => Debug.Log("Interstitial ad full screen content opened.");
        interstitialAd.OnAdFullScreenContentClosed += () => Debug.Log("Interstitial ad full screen content closed.");
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error : " + error);
        };
    }

    private void InterstitialReloadHandler(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");
            LoadInterstitialAd();
            interRewardEvent?.Invoke();
        };
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error : " + error);
            LoadInterstitialAd();
        };
    }
    #endregion

    #region Rewareded Ads
    private void LoadRewardedAd()
    {
        if (IsNoAdsEnabled()) return;

        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");
        var adRequest = new AdRequest();

        RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
            _rewardedAd = ad;
            RewaredEvent(_rewardedAd);
            RewardedReloadHandler(_rewardedAd);
        });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (IsNoAdsEnabled()) return;

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            bool hasEarnedReward = false;

            // Khi người chơi xem hết video
            _rewardedAd.Show((Reward reward) =>
            {
                hasEarnedReward = true;
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });

            // Khi người chơi đóng quảng cáo (ấn nút X)
            _rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("📴 Người chơi đã đóng quảng cáo.");
                if (hasEarnedReward)
                {
                    RewardedEndEvent?.Invoke();
                    Debug.Log("✅ Đã xem xong và đóng quảng cáo → kích hoạt sự kiện thưởng.");
                }
                else
                {
                    Debug.Log("❌ Người chơi tắt quảng cáo trước khi xem xong — không thưởng.");
                }
            };
        }
        else
        {
            LoadRewardedAd();
        }
    }


    public void GiveRewarededReward()
    {
        Debug.Log("Rewared Ads Reward Given");
    }

    private void RewaredEvent(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };
        ad.OnAdImpressionRecorded += () => Debug.Log("Rewarded ad recorded an impression.");
        ad.OnAdClicked += () => Debug.Log("Rewarded ad was clicked.");
        ad.OnAdFullScreenContentOpened += () => Debug.Log("Rewarded ad full screen content opened.");
        ad.OnAdFullScreenContentClosed += () => Debug.Log("Rewarded ad full screen content closed.");
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error : " + error);
        };
    }

    private void RewardedReloadHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");
            LoadRewardedAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error : " + error);
            LoadRewardedAd();
        };
    }
    #endregion

    // ============================
    //         MREC 300x250
    // ============================
    public void LoadMRECAd()
    {
        if (IsNoAdsEnabled()) return;

        // Xoá MREC cũ nếu có
        if (_mrecView != null)
        {
            _mrecView.Destroy();
            _mrecView = null;
        }

        Debug.Log("Loading MREC Ad...");

        // 300x250
        AdSize size = AdSize.MediumRectangle;

        _mrecView = new BannerView(mrecId, size, AdPosition.Bottom);

        var request = new AdRequest();
        _mrecView.LoadAd(request);

        // CALLBACK
        _mrecView.OnBannerAdLoaded += () =>
        {
            Debug.Log("MREC loaded!");
            if (mrecBg != null) mrecBg.SetActive(true);
        };

        _mrecView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("MREC failed: " + error);
            if (mrecBg != null) mrecBg.SetActive(false);
        };

        _mrecView.OnAdClicked += () =>
        {
            Debug.Log("MREC clicked.");
        };

        _mrecView.OnAdPaid += (AdValue value) =>
        {
            Debug.Log($"MREC paid {value.Value} {value.CurrencyCode}");
        };
    }


    public void DestroyMREC()
    {
        if (_mrecView != null)
        {
            _mrecView.Destroy();
            _mrecView = null;
        }
        if (mrecBg != null) mrecBg.SetActive(false);
    }
    public bool IsAppOpenReady()
    {
        return appOpenAd != null;
    }
    public bool IsRewardedReady()
    {
        return _rewardedAd != null && _rewardedAd.CanShowAd();
    }
    public bool IsInterstitialReady()
    {
        return _interstitialAd != null && _interstitialAd.CanShowAd();
    }
    public void PlayGoogleRewarded()
    {
        ShowRewardedAd();
    }

}
