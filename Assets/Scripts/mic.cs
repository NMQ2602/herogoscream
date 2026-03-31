using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class MicPermissionManager : MonoBehaviour
{
    void Start()
    {
        RequestMicPermission();
    }

    void RequestMicPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        else
        {
            Debug.Log("✅ Đã có quyền Microphone");
        }
#endif
    }
}