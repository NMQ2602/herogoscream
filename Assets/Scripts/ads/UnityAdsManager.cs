using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class UnityAdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static UnityAdsManager Instance;

    [Header("Unity Ads Placement IDs")]
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

    IEnumerator InitRoutine()
    {
        while (!Advertisement.isInitialized) yield return null;

        LoadBanner();
        LoadInterstitial();
        LoadRewarded();
    }

    // ======================================================
    // APP OPEN
    // ======================================================
    public void ShowAppOpen()
    {
        Advertisement.Show(appOpenId);
    }

    // ======================================================
    // BANNER
    // ======================================================
    public void LoadBanner()
    {
        Advertisement.Banner.Load(bannerId);
    }

    public void ShowBanner()
    {
        Advertisement.Banner.Show(bannerId);
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    // ======================================================
    // INTERSTITIAL
    // ======================================================
    public void LoadInterstitial()
    {
        Advertisement.Load(interId, this);
    }

    public bool IsInterReady()
    {
        return interReady;
    }

    public void ShowInterstitial()
    {
        Advertisement.Show(interId, this);
    }

    // ======================================================
    // REWARDED
    // ======================================================
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
        Advertisement.Show(rewardedId, this);
    }

    // ======================================================
    // CALLBACKS
    // ======================================================
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
}
