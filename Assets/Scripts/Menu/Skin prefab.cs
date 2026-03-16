using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public SpriteRenderer playerRenderer;
    public Sprite[] skins;

    void Start()
    {
        int index = PlayerPrefs.GetInt("SelectedSkin", 0);

        if (index >= 0 && index < skins.Length)
        {
            playerRenderer.sprite = skins[index];
        }
    }
}