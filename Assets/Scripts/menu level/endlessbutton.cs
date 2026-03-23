using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelToggle : MonoBehaviour
{
    [Header("UI Elements")]
    public Button toggleButton;
    public RectTransform panel;
    public float slideDuration = 0.5f;
    public float stayDuration = 2f;  

    private Vector2 hiddenPos;
    private Vector2 shownPos;

    void Start()
    {
        if (panel == null || toggleButton == null) return;

  
        shownPos = panel.anchoredPosition;
   
        hiddenPos = shownPos + new Vector2(0, panel.rect.height);

 
        panel.gameObject.SetActive(false);
 
        toggleButton.onClick.AddListener(() =>
        {
            StartCoroutine(ShowAndHidePanel());
        });
    }

    IEnumerator ShowAndHidePanel()
    {
 
        panel.gameObject.SetActive(true);
        yield return StartCoroutine(SlidePanel(panel, hiddenPos, shownPos));

 
        yield return new WaitForSeconds(stayDuration);

 
        yield return StartCoroutine(SlidePanel(panel, shownPos, hiddenPos));
        panel.gameObject.SetActive(false);
    }

    IEnumerator SlidePanel(RectTransform target, Vector2 from, Vector2 to)
    {
        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            target.anchoredPosition = Vector2.Lerp(from, to, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.anchoredPosition = to;
    }
}