using UnityEngine;
using System.Collections;
using TMPro;

public class HintBlink : MonoBehaviour
{
    public float blinkSpeed = 0.5f; 
    public float showTime = 4f;   

    CanvasGroup cg;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();

        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();

        StartCoroutine(BlinkThenHide());
    }

    IEnumerator BlinkThenHide()
    {
        float timer = 0;

        while (timer < showTime)
        {
            // Fade out
            yield return Fade(1f, 0f, blinkSpeed);

            // Fade in
            yield return Fade(0f, 1f, blinkSpeed);

            timer += blinkSpeed * 2;
        }

        gameObject.SetActive(false);
    }

    IEnumerator Fade(float from, float to, float time)
    {
        float t = 0;

        while (t < time)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }

        cg.alpha = to;
    }
}