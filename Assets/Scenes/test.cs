using UnityEngine;

public class ResetGameData : MonoBehaviour
{
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Đã reset toàn bộ dữ liệu game");
    }
}