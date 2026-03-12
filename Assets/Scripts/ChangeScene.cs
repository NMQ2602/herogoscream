using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeScene : MonoBehaviour
{
    public GameObject fadeObject;
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    public void LoadScene(string sceneName)
    {
        fadeObject.SetActive(true);

        StartCoroutine(Fade(sceneName));
    }

    IEnumerator Fade(string sceneName)
    {
        float alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.unscaledDeltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}