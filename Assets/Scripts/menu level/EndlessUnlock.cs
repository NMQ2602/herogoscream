using UnityEngine;
using UnityEngine.UI;

public class UnlockByLevel : MonoBehaviour
{
    [Header("Level to unlock")]
    public string unlockLevel = "Level2";

    [Header("Mission Buttons")]
    public Button missionButton;     

    [Header("Endless Buttons")]
    public Button endlessButton;     
    public Button endlessButtonlock; 

    void Start()
    {
        float bestWin = PlayerPrefs.GetFloat("BestWin_" + unlockLevel, 0f);
        float bestLose = PlayerPrefs.GetFloat("BestLose_" + unlockLevel, 0f);
        float best = Mathf.Max(bestWin, bestLose);


        bool unlocked = best >= 100f;

        if (missionButton != null)
            missionButton.gameObject.SetActive(unlocked);

        if (endlessButton != null)
            endlessButton.gameObject.SetActive(unlocked);

        if (endlessButtonlock != null)
            endlessButtonlock.gameObject.SetActive(!unlocked);
    }
}