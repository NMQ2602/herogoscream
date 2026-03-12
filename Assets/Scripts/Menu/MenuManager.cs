using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public bool levelPanelOpened = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}