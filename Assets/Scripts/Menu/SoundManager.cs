using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Image iconImage;

    public Sprite iconOn;  
    public Sprite iconOff;

    const string KEY = "SoundMuted";

    bool isMuted;

    void Start()
    {
        isMuted = PlayerPrefs.GetInt(KEY, 0) == 1;

        ApplyState();
    }

    // ==========================
    public void ToggleSound()
    {
        isMuted = !isMuted;

        PlayerPrefs.SetInt(KEY, isMuted ? 1 : 0);
        PlayerPrefs.Save();

        ApplyState();
    }

    // ==========================
    void ApplyState()
    {
        AudioListener.volume = isMuted ? 0f : 1f;

        if (iconImage != null)
            iconImage.sprite = isMuted ? iconOff : iconOn;
    }
}