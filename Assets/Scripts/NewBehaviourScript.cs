using UnityEngine;
using System.Collections;

public class ButtonScale : MonoBehaviour
{
    public float speed = 6f;

    public void ShowButton()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ScaleShow());
    }

    public void HideButton()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleHide());
    }

    IEnumerator ScaleShow()
    {
        transform.localScale = Vector3.zero;

        while (transform.localScale.x < 1f)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                Vector3.one,
                Time.deltaTime * speed
            );

            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    IEnumerator ScaleHide()
    {
        while (transform.localScale.x > 0.05f)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                Vector3.zero,
                Time.deltaTime * speed
            );

            yield return null;
        }

        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}