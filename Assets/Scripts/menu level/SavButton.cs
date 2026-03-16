using UnityEngine;
using UnityEngine.UI;

public class SaveButtonState : MonoBehaviour
{
    public string saveKey = "";

    Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
    }

    void Start()
    {
        int saved = PlayerPrefs.GetInt(saveKey, 1);
        btn.interactable = saved == 1;
    }

    public void DisableAndSave()
    {
        btn.interactable = false;

        PlayerPrefs.SetInt(saveKey, 0);
        PlayerPrefs.Save();
    }
}