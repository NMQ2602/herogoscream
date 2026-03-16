using UnityEngine;

public class AppOpenOnResume : MonoBehaviour
{
    private GoogleAdsManager ads;

    private void Start()
    {
        ads = GoogleAdsManager.Instance;

        if (ads == null)
            Debug.LogWarning("❗ GoogleAdsManager.Instance = NULL (script chưa spawn hoặc bị disable)");
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            if (ads != null && ads.IsAppOpenReady())
            {
                ads.ShowAppOpenAd();   // ⬅️ Dùng đúng hàm
            }
            else
            {
                Debug.Log("AppOpen không sẵn sàng lúc resume.");
            }
        }
    }
}
