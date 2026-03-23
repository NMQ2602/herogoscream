using UnityEngine;

public class MissionBinder : MonoBehaviour
{
    public MissionItem[] missionItems;

    void Start()
    {
        foreach (var m in missionItems)
        {
            if (m != null)
                m.LoadData();
        }

        if (MissionListManager.instance != null)
        {
            MissionListManager.instance.SetMissions(missionItems);
        }
    }
}