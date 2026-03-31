using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class UnityAdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static UnityAdsManager Instance;

    public string appOpenId = "AppOpen_Android";
    public string bannerId = "Banner_Android";
    public string interId = "Interstitial_Android";
    public string rewardedId = "Rewarded_Android";

    private bool interReady = false;
    private bool rewardedReady = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(InitRoutine());
    }

    bool IsNoAds()
    {
        return PlayerPrefs.GetInt("noads", 0) == 1;
    }

    IEnumerator InitRoutine()
    {
        while (!Advertisement.isInitialized) yield return null;

        if (!IsNoAds())
        {
            LoadBanner();
            LoadInterstitial();
        }

        LoadRewarded();
    }

    public void ShowAppOpen()
    {
        if (IsNoAds()) return;
        Advertisement.Show(appOpenId);
    }

    public void LoadBanner()
    {
        if (IsNoAds()) return;
        Advertisement.Banner.Load(bannerId);
    }

    public void ShowBanner()
    {
        if (IsNoAds()) return;
        Advertisement.Banner.Show(bannerId);
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    public void LoadInterstitial()
    {
        if (IsNoAds()) return;
        Advertisement.Load(interId, this);
    }

    public bool IsInterReady()
    {
        return interReady;
    }

    public void ShowInterstitial()
    {
        if (IsNoAds()) return;

        if (interReady)
            Advertisement.Show(interId, this);
    }

    public void LoadRewarded()
    {
        Advertisement.Load(rewardedId, this);
    }

    public bool IsRewardedReady()
    {
        return rewardedReady;
    }

    public void ShowRewarded()
    {
        if (rewardedReady)
            Advertisement.Show(rewardedId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == interId) interReady = true;
        if (placementId == rewardedId) rewardedReady = true;
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state)
    {
        if (placementId == interId)
        {
            interReady = false;

            if (!IsNoAds())
                LoadInterstitial();
        }

        if (placementId == rewardedId)
        {
            rewardedReady = false;
            LoadRewarded();
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }

    public void DisableAllAds()
    {
        Advertisement.Banner.Hide();
        StopAllCoroutines();
        interReady = false;
    }
}