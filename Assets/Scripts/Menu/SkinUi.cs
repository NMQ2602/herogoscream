using UnityEngine;
using UnityEngine.UI;

public class PlayerSkinUI : MonoBehaviour
{
    public Image playerImage;
    public Sprite[] skins;

    void Start()
    {
        UpdateSkin();
    }

    public void UpdateSkin()
    {
        int index = PlayerPrefs.GetInt("SelectedSkin", 0);

        if (index >= 0 && index < skins.Length)
        {
            playerImage.sprite = skins[index];
        }
    }
}