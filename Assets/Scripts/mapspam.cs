using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTileSpawner : MonoBehaviour
{
    [Header("Tile Settings")]
    public GameObject[] tilePrefabs;
    public float minGap = 1f;
    public float maxGap = 3f;
    public float minY = -0.5f;
    public float maxY = 1f;
    public int maxActiveTiles = 20;

    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public float coinSpawnChance = 0.6f;
    public float coinHeightOffset = 1f;

    [Header("Player Settings")]
    public Transform player;
    public float spawnAheadDistance = 10f;
    public float dieY = -5f;

    private List<GameObject> activeTiles = new List<GameObject>();
    private float lastX;
    private float lastY;

    void Start()
    {
        if (tilePrefabs.Length == 0 || player == null) return;

        lastX = transform.position.x;
        lastY = transform.position.y;

        GameObject firstTile = SpawnTile(lastX, lastY);
        activeTiles.Add(firstTile);

        for (int i = 1; i < 5; i++)
            SpawnNextTile();
    }

    void Update()
    {
        if (player == null || GameManager_Endless.instance == null) return;

        // Spawn tile
        if (activeTiles.Count > 0)
        {
            GameObject lastTile = activeTiles[activeTiles.Count - 1];

            if (player.position.x + spawnAheadDistance > lastTile.transform.position.x)
                SpawnNextTile();
        }

        // Check chết
        if (player.position.y < dieY)
        {
            GameManager_Endless.instance.Lose();
        }
    }

    void SpawnNextTile()
    {
        float newX = lastX + Random.Range(minGap, maxGap);

        // ⭐ Random Y nhưng không tụt dốc liên tục
        float deltaY = Random.Range(minY, maxY);

        // Không cho tụt liên tục xuống
        if (lastY <= 0 && deltaY < 0)
            deltaY = Mathf.Abs(deltaY);

        float newY = lastY + deltaY;

        // ⭐ Chặn không xuống vùng chết
        newY = Mathf.Max(newY, dieY + 2f);

        // ⭐ Giới hạn map cho đẹp
        newY = Mathf.Clamp(newY, -1f, 2f);

        GameObject newTile = SpawnTile(newX, newY);

        activeTiles.Add(newTile);
        lastX = newX;
        lastY = newY;

        // Xóa tile cũ
        if (activeTiles.Count > maxActiveTiles)
        {
            if (activeTiles[0] != null)
                Destroy(activeTiles[0]);

            activeTiles.RemoveAt(0);
        }
    }

    GameObject SpawnTile(float x, float y)
    {
        GameObject tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
        Vector3 pos = new Vector3(x, y, 0f);

        GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);

        SpawnCoin(pos);

        // ⭐ Gắn trigger tính điểm
        TileTrigger trigger = tile.AddComponent<TileTrigger>();
        trigger.spawner = this;

        return tile;
    }

    void SpawnCoin(Vector3 tilePos)
    {
        if (coinPrefab != null && Random.value < coinSpawnChance)
        {
            Vector3 coinPos = tilePos;
            coinPos.y += coinHeightOffset;

            Instantiate(coinPrefab, coinPos, Quaternion.identity, transform);
        }
    }

    // ⭐ CỘNG ĐIỂM + MISSION
    public void AddScore(int amount)
    {
        if (GameManager_Endless.instance != null)
            GameManager_Endless.instance.AddScore(amount);

        if (MissionListManager.instance != null)
            MissionListManager.instance.AddScore(amount);
    }
}

public class TileTrigger : MonoBehaviour
{
    public EndlessTileSpawner spawner;
    private bool scored = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!scored && other.CompareTag("Player"))
        {
            if (spawner != null)
                spawner.AddScore(1);

            scored = true;
        }
    }
}