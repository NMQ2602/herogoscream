using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class LoginGG : MonoBehaviour
{
    public static LoginGG instance;
    public static bool isLogin = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            InitGooglePlayGames(); // init
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Login(); // auto login
    }

    void InitGooglePlayGames()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void Login()
    {
        PlayGamesPlatform.Instance.Authenticate((SignInStatus status) =>
        {
            if (status == SignInStatus.Success)
            {
                isLogin = true;
                Debug.Log("✅ Login thành công");
                Debug.Log("User: " + Social.localUser.userName);
            }
            else
            {
                isLogin = false;
                Debug.LogError("❌ Login thất bại: " + status);
            }
        });
    }
}