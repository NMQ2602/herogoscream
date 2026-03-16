using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public TMP_Text coinText;

    int coins;

    // =================================
    void Awake()
    {
        // Singleton + DontDestroy
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 💎 không bị hủy khi đổi scene
        }
        else
        {
            Destroy(gameObject); // tránh tạo trùng
            return;
        }
    }

    // =================================
    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateUI();
    }

    // =================================
    public void AddCoins(int amount)
    {
        coins += amount;

        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();

        UpdateUI();
    }

    // =================================
    public bool SpendCoins(int amount)
    {
        if (coins < amount)
            return false;

        coins -= amount;

        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();

        UpdateUI();

        return true;
    }

    // =================================
    public int GetCoins()
    {
        return coins;
    }

    // =================================
    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coins.ToString();
    }

    // =================================
    // Gọi khi scene mới load xong để gán text mới
    public void SetCoinText(TMP_Text newText)
    {
        coinText = newText;
        UpdateUI();
    }
}