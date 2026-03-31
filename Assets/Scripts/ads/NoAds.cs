using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;

public class RemoveAdsIAP : MonoBehaviour, IStoreListener
{
    public static RemoveAdsIAP Instance;

    private IStoreController controller;
    private IExtensionProvider extensions;

    public const string REMOVE_ADS = "remove_ads";

    [Header("UI")]
    public GameObject removeAdsButton;
    public TextMeshProUGUI statusText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        if (IsNoAds())
        {
            ActivateNoAds();
            HideRemoveAdsButton();
        }

        InitIAP();
    }

    void InitIAP()
    {
        if (controller != null) return;

        var builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance()
        );

        builder.AddProduct(REMOVE_ADS, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyRemoveAds()
    {
        if (controller == null)
        {
            Debug.LogError("IAP chưa sẵn sàng");
            return;
        }

        if (IsNoAds())
        {
            Debug.Log("Đã mua rồi!");
            return;
        }

        statusText?.SetText("Đang xử lý...");
        controller.InitiatePurchase(REMOVE_ADS);
    }

    public void OnInitialized(IStoreController c, IExtensionProvider e)
    {
        controller = c;
        extensions = e;

        Debug.Log("IAP Ready");

        var product = controller.products.WithID(REMOVE_ADS);

        if (product != null && product.hasReceipt)
        {
            Debug.Log("Restore thành công");
            ActivateNoAds();
            HideRemoveAdsButton();
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("IAP Init Failed: " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("IAP Init Failed: " + error + " | " + message);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == REMOVE_ADS)
        {
            Debug.Log("Mua No Ads thành công");

            ActivateNoAds();
            HideRemoveAdsButton();
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogError("Purchase Failed: " + reason);

        if (statusText != null)
            statusText.text = "Mua thất bại!";
    }

    public void ActivateNoAds()
    {
        PlayerPrefs.SetInt("noads", 1);
        PlayerPrefs.Save();

        Debug.Log("🎉 Remove Ads Activated");

        if (statusText != null)
            statusText.text = "Đã mua No Ads ✔";

        if (GoogleAdsManager.Instance != null)
        {
            GoogleAdsManager.Instance.DestroyBannerView();
            GoogleAdsManager.Instance.DestroyMREC();
        }

        if (UnityAdsManager.Instance != null)
        {
            UnityAdsManager.Instance.HideBanner();
        }
        if (GoogleAdsManager.Instance != null)
        {
            GoogleAdsManager.Instance.DisableAllAds();
        }
        if (UnityAdsManager.Instance != null)
        {
            UnityAdsManager.Instance.DisableAllAds();
        }
    }

    void HideRemoveAdsButton()
    {
        if (removeAdsButton != null)
        {
            removeAdsButton.SetActive(false);

            Button btn = removeAdsButton.GetComponent<Button>();
            if (btn != null)
                btn.interactable = false;
        }
    }

    public bool IsNoAds()
    {
        return PlayerPrefs.GetInt("noads", 0) == 1;
    }
    public void RestorePurchase()
    {
        Debug.Log("Check lại purchase (Android)");

        if (controller == null)
        {
            Debug.LogError("IAP chưa init");
            return;
        }

        statusText?.SetText("Đang kiểm tra...");

        var product = controller.products.WithID(REMOVE_ADS);

        if (product != null && product.hasReceipt)
        {
            Debug.Log("Đã mua trước đó");

            ActivateNoAds();
            HideRemoveAdsButton();

            statusText?.SetText("Đã khôi phục ✔");
        }
        else
        {
            Debug.Log("Chưa mua");

            statusText?.SetText("Chưa có mua hàng");
        }
    }
}