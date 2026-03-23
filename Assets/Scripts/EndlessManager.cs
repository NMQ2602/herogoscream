using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager_Endless : MonoBehaviour
{
    public static GameManager_Endless instance;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip loseSound;

    [Header("Player & Tiles")]
    public Transform player;

    [Header("UI")]
    public TMP_Text progressText;     
    public TMP_Text loseProgressText;  
    public TMP_Text loseBestText;   
    public GameObject loseUI;
    public GameObject pauseUI;
    public GameObject submitUI;

    [HideInInspector] public bool gameEnded = false;

    private int score = 0;
    private int highScore = 0;
    private string highScoreKey = "HighScore_Endless";

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        UpdateScoreUI();
    }

    public void AddScore(int value = 1)
    {
        if (gameEnded) return;

        score += value;
        if (score > highScore) highScore = score;

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (progressText != null)
            progressText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public void Lose()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (sfxSource != null && loseSound != null)
            sfxSource.PlayOneShot(loseSound);

        PlayerPrefs.SetInt(highScoreKey, highScore);
        PlayerPrefs.Save();

        if (loseUI != null) loseUI.SetActive(true);
        if (loseProgressText != null) loseProgressText.text = score.ToString();
        if (loseBestText != null) loseBestText.text = highScore.ToString();

        if (submitUI != null)
            submitUI.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Pause()
    {
        if (gameEnded) return;
        Time.timeScale = 0f;
        if (pauseUI != null) pauseUI.SetActive(true);
    }

    public void Resume()
    {
        if (gameEnded) return;
        Time.timeScale = 1f;
        if (pauseUI != null) pauseUI.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}