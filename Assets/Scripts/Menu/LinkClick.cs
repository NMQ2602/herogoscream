using UnityEngine;

public class OpenLinkButton : MonoBehaviour
{
    [SerializeField] private string url;

    public void OpenLink()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("URL chưa được nhập!");
        }
    }
}