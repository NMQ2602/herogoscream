using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform levelStart;
    public Transform levelEnd;
    public Transform player;

    public TMP_Text progressText;

    public TMP_Text loseProgressText;
    public TMP_Text loseBestText;

    public TMP_Text winProgressText;
    public TMP_Text winBestText;

    public GameObject winUI;
    public GameObject loseUI;
    public GameObject pauseUI;

    public bool gameEnded = false;

    float currentPercent = 0;

    float bestLose = 0;
    float bestWin = 0;

    string loseKey;
    string winKey;

    void Awake()
    {
        instance = this;

        string levelName = SceneManager.GetActiveScene().name;

        loseKey = "BestLose_" + levelName;
        winKey = "BestWin_" + levelName;

        bestLose = PlayerPrefs.GetFloat(loseKey, 0);
        bestWin = PlayerPrefs.GetFloat(winKey, 0);
    }

    void Update()
    {
        if (!gameEnded)
            UpdateProgress();
    }

    void UpdateProgress()
    {
        if (levelStart == null || levelEnd == null || player == null) return;

        float totalDistance = levelEnd.position.x - levelStart.position.x;
        float playerDistance = player.position.x - levelStart.position.x;

        float percent = (playerDistance / totalDistance) * 100f;
        percent = Mathf.Clamp(percent, 0, 100);

        currentPercent = percent;

        if (progressText != null)
            progressText.text = Mathf.RoundToInt(percent) + "%";

        if (percent >= 100)
            Win();
    }

    float GetBestPercent()
    {
        return Mathf.Max(bestWin, bestLose);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Lose()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (currentPercent > bestLose)
        {
            bestLose = currentPercent;
            PlayerPrefs.SetFloat(loseKey, bestLose);
        }

        PlayerPrefs.Save();

        Time.timeScale = 0;

        if (loseUI != null)
            loseUI.SetActive(true);

        if (loseProgressText != null)
            loseProgressText.text = Mathf.RoundToInt(currentPercent) + "%";

        if (loseBestText != null)
            loseBestText.text = Mathf.RoundToInt(GetBestPercent()) + "%";
    }

    public void Win()
    {
        if (gameEnded) return;

        gameEnded = true;

        currentPercent = 100;

        if (currentPercent > bestWin)
        {
            bestWin = currentPercent;
            PlayerPrefs.SetFloat(winKey, bestWin);
        }

        PlayerPrefs.Save();

        Time.timeScale = 0;

        if (winUI != null)
            winUI.SetActive(true);

        if (winProgressText != null)
            winProgressText.text = "100%";

        if (winBestText != null)
            winBestText.text = Mathf.RoundToInt(GetBestPercent()) + "%";
    }
}