using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ShowAchievementUI : MonoBehaviour
{
    public void ShowUI()
    {
        if (Social.localUser.authenticated)
        {
            Debug.Log("Mở Achievement UI");
            Social.ShowAchievementsUI();
        }
        else
        {
            Debug.LogError("❌ Chưa login");
        }
    }
}