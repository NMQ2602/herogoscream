using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public TMP_Text coinText;

    int coins;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateUI();
    }

    // thêm coin
    public void AddCoins(int amount)
    {
        coins += amount;

        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();

        UpdateUI();
    }

    // trừ coin (dùng trong shop)
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

    // lấy số coin hiện tại
    public int GetCoins()
    {
        return coins;
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coins.ToString();
    }
}