using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CoinManager.instance != null)
                CoinManager.instance.AddCoins(coinValue);

            if (MissionListManager.instance != null)
                MissionListManager.instance.AddCherry(1);

            Destroy(gameObject);
        }
    }
}