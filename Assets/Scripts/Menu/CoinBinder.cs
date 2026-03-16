using UnityEngine;
using TMPro;

public class CoinTextBinder : MonoBehaviour
{
    public TMP_Text text;

    void Start()
    {
        if (CoinManager.instance != null)
        {
            CoinManager.instance.SetCoinText(text);
        }
    }
}