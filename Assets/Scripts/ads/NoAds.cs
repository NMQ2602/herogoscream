using UnityEngine;
using UnityEngine.Purchasing;

public class RemoveAdsIAP : MonoBehaviour, IStoreListener
{
    private IStoreController controller;

    public const string REMOVE_ADS = "remove_ads";

    void Start()
    {
        var builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance()
        );

        builder.AddProduct(REMOVE_ADS, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    // =========================
    public void BuyRemoveAds()
    {
        if (controller != null)
            controller.InitiatePurchase(REMOVE_ADS);
    }

    // =========================
    public void OnInitialized(IStoreController c, IExtensionProvider e)
    {
        controller = c;
        Debug.Log("IAP Ready");
    }

    // ⭐ Hàm cũ
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("IAP Init Failed: " + error);
    }

    // ⭐ Hàm mới
    public void OnInitializeFailed(
        InitializationFailureReason error,
        string message)
    {
        Debug.LogError("IAP Init Failed: " + error + " | " + message);
    }

    // =========================
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == REMOVE_ADS)
        {
            ActivateNoAds();
        }

        return PurchaseProcessingResult.Complete;
    }

    // =========================
    void ActivateNoAds()
    {
        PlayerPrefs.SetInt("noads", 1);
        PlayerPrefs.Save();

        Debug.Log("🎉 Remove Ads Activated");

        // Google Ads
        if (GoogleAdsManager.Instance != null)
        {
            GoogleAdsManager.Instance.DestroyBannerView();
            GoogleAdsManager.Instance.DestroyMREC();
        }

        // Unity Ads
        if (UnityAdsManager.Instance != null)
        {
            UnityAdsManager.Instance.HideBanner();
        }
    }

    // =========================
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogError("Purchase Failed: " + reason);
    }
}