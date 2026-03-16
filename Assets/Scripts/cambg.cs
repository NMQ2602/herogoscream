using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class BgCam : MonoBehaviour
{
    WebCamTexture tex;

    [Header("UI")]
    public RawImage display;

    [Header("Button Icon")]
    public Image buttonImage;
    public Sprite iconOff;   // hình khi camera tắt
    public Sprite iconOn;    // hình khi camera bật

    void Start()
    {
        if (display != null)
            display.gameObject.SetActive(false);

        SetIcon(false);
    }

    // ============================
    public void StartStopCam_Clicked()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            StartCoroutine(WaitForCameraPermission());
            return;
        }
#endif

        if (tex != null)
            StopWebCam();
        else
            StartWebCam();
    }

    // ============================
    void StartWebCam()
    {
        var devices = WebCamTexture.devices;

        if (devices.Length == 0) return;

        // ưu tiên camera trước
        WebCamDevice device = devices[0];
        foreach (var d in devices)
        {
            if (d.isFrontFacing)
            {
                device = d;
                break;
            }
        }

        tex = new WebCamTexture(device.name);
        display.texture = tex;

        tex.Play();
        display.gameObject.SetActive(true);

        // lật camera trước
        if (device.isFrontFacing)
            display.rectTransform.localScale = new Vector3(-1, 1, 1);
        else
            display.rectTransform.localScale = Vector3.one;

        // xoay đúng hướng
        float antiRotate = -(360 - tex.videoRotationAngle);
        display.transform.rotation = Quaternion.Euler(0, 0, antiRotate);

        SetIcon(true);
    }

    // ============================
    void StopWebCam()
    {
        if (tex != null)
        {
            tex.Stop();
            tex = null;
        }

        if (display != null)
            display.gameObject.SetActive(false);

        SetIcon(false);
    }

    // ============================
    void SetIcon(bool isOn)
    {
        if (buttonImage == null) return;

        buttonImage.sprite = isOn ? iconOn : iconOff;
    }

    // ============================
    IEnumerator WaitForCameraPermission()
    {
        yield return new WaitForSeconds(1f);

#if UNITY_ANDROID
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            StartStopCam_Clicked();
        }
#endif
    }
}