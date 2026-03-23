using UnityEngine;

public class MissionUI : MonoBehaviour
{
    public static MissionUI instance;

    public GameObject redDot;
    public GameObject missionPanel;

    void Awake()
    {
        instance = this;
    }

    public void ShowRedDot()
    {
        redDot.SetActive(true);
    }

    public void HideRedDot()
    {
        redDot.SetActive(false);
    }

    public void OpenMission()
    {
        missionPanel.SetActive(true);
    }

    public void CloseMission()
    {
        missionPanel.SetActive(false);
    }
}