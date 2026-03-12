using UnityEngine;
using UnityEngine.UI;

public class MicPlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 7f;

    public Slider sensitivitySlider;

    public Rigidbody2D rb;

    public LayerMask dieLayer;

    string device;
    AudioClip micClip;

    int sampleWindow = 128;

    float sensitivity = 0.2f;
    float micMultiplier = 50f;

    float smoothLoudness = 0;

    bool canJump = true;

    float runThreshold;
    float jumpThreshold;

    string sliderKey = "MicSensitivity";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        // Start microphone
        if (Microphone.devices.Length > 0)
        {
            device = Microphone.devices[0];
            micClip = Microphone.Start(device, true, 10, 44100);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 0f;
            sensitivitySlider.maxValue = 1f;

            // load giá trị đã lưu
            float savedValue = PlayerPrefs.GetFloat(sliderKey, 0.5f);
            sensitivitySlider.value = savedValue;

            sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);

            ChangeSensitivity(savedValue);
        }
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.gameEnded)
            return;

        if (sensitivitySlider != null && sensitivitySlider.value <= 0.01f)
        {
            StopMove();
            return;
        }

        float loudness = GetLoudness();

        smoothLoudness = Mathf.Lerp(smoothLoudness, loudness, 0.15f);

        if (smoothLoudness > runThreshold)
        {
            Move();
        }
        else
        {
            StopMove();
        }

        if (smoothLoudness > jumpThreshold && canJump)
        {
            Jump(smoothLoudness);
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void StopMove()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Jump(float loudness)
    {
        float jumpPower = jumpForce + loudness * 3f;

        jumpPower = Mathf.Clamp(jumpPower, jumpForce, jumpForce * 2.5f);

        rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        canJump = false;
    }

    public void ChangeSensitivity(float value)
    {
        // lưu slider
        PlayerPrefs.SetFloat(sliderKey, value);
        PlayerPrefs.Save();

        if (value <= 0.01f)
        {
            runThreshold = 999f;
            jumpThreshold = 999f;
            return;
        }

        sensitivity = Mathf.Lerp(0.5f, 0.1f, value);

        runThreshold = sensitivity;
        jumpThreshold = sensitivity * 3f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;

        if ((dieLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((dieLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (GameManager.instance != null)
            GameManager.instance.Lose();

        gameObject.SetActive(false);
    }

    float GetLoudness()
    {
        if (micClip == null) return 0;

        int micPosition = Microphone.GetPosition(device);

        if (micPosition < sampleWindow)
            return 0;

        float[] data = new float[sampleWindow];
        micClip.GetData(data, micPosition - sampleWindow);

        float sum = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            sum += data[i] * data[i];
        }

        float rms = Mathf.Sqrt(sum / sampleWindow);

        return rms * micMultiplier;
    }
}