using UnityEngine;
using TMPro;

public class SkinShopItem : MonoBehaviour
{
    public int skinIndex;
    public int price;

    public TMP_Text priceText;
    public GameObject lockIcon;

    public GameObject shopPanel;

    void Start()
    {
        // skin mặc định
        if (skinIndex == 0)
        {
            PlayerPrefs.SetInt("Skin_0", 1);

            if (lockIcon != null)
                lockIcon.SetActive(false);

            if (priceText != null)
                priceText.text = "SELECT";

            return;
        }

        bool unlocked = IsUnlocked();

        if (unlocked)
        {
            if (lockIcon != null)
                lockIcon.SetActive(false);

            if (priceText != null)
                priceText.text = "SELECT";
        }
        else
        {
            if (priceText != null)
                priceText.text = price.ToString();
        }
    }

    bool IsUnlocked()
    {
        return PlayerPrefs.GetInt("Skin_" + skinIndex, 0) == 1;
    }

    void UnlockSkin()
    {
        PlayerPrefs.SetInt("Skin_" + skinIndex, 1);

        if (lockIcon != null)
            lockIcon.SetActive(false);

        if (priceText != null)
            priceText.text = "SELECT";
    }

    void SelectSkin()
    {
        PlayerPrefs.SetInt("SelectedSkin", skinIndex);

        UpdateSkinUI();

        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    public void ClickSkin()
    {
        if (IsUnlocked())
        {
            SelectSkin();
        }
        else
        {
            if (CoinManager.instance.SpendCoins(price))
            {
                UnlockSkin();
                SelectSkin();
            }
        }
    }

    public void UnlockByFacebook()
    {
        if (!IsUnlocked())
        {
            Application.OpenURL("https://facebook.com/yourpage");
            UnlockSkin();
        }

        SelectSkin();
    }

    public void UnlockByTwitter()
    {
        if (!IsUnlocked())
        {
            Application.OpenURL("https://x.com/yourpage");
            UnlockSkin();
        }

        SelectSkin();
    }

    void UpdateSkinUI()
    {
        PlayerSkinUI ui = FindObjectOfType<PlayerSkinUI>();

        if (ui != null)
            ui.UpdateSkin();
    }
}