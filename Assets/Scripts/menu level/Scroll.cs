using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollAmount = 0.2f;

    public void ScrollUp()
    {
        scrollRect.verticalNormalizedPosition += scrollAmount;
    }

    public void ScrollDown()
    {
        scrollRect.verticalNormalizedPosition -= scrollAmount;
    }
}