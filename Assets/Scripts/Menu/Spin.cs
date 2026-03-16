using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SpinWheel : MonoBehaviour
{
    public Transform wheel;

    public Button spinButton;
    public Button claimButton;
    public Button bonusButton;

    public GameObject spinPanel;

    public TMP_Text rewardText;

    public float minSpinTime = 3f;
    public float maxSpinTime = 5f;

    public int rewardCount = 8;
    public float wheelOffset = 0f;

    public int[] rewards = { 100, 200, 300, 500, 1000, 50, 400, 800 };
    public int[] bonusMultipliers = { 2, 3, 2, 10, 5, 4, 3, 6 };

    bool spinning = false;

    int pendingReward = 0;
    int pendingMultiplier = 1;

    bool bonusMode = false;

    void Start()
    {
        claimButton.gameObject.SetActive(false);
        bonusButton.gameObject.SetActive(false);
        rewardText.text = "";

        Debug.Log("SpinWheel Start");
    }

    public void Spin()
    {
        if (spinning) return;

        Debug.Log("SPIN START");

        spinButton.gameObject.SetActive(false);
        claimButton.gameObject.SetActive(false);
        bonusButton.gameObject.SetActive(false);

        rewardText.text = "";

        bonusMode = false;

        StartCoroutine(SpinWheelRoutine());
    }

    IEnumerator SpinWheelRoutine()
    {
        spinning = true;

        float spinTime = Random.Range(minSpinTime, maxSpinTime);
        float timer = 0;

        float startSpeed = Random.Range(800f, 1200f);

        float totalRotation = 0f; // tổng độ quay

        Debug.Log("Spin Time: " + spinTime);
        Debug.Log("Start Speed: " + startSpeed);

        while (timer < spinTime)
        {
            float speed = Mathf.Lerp(startSpeed, 0, timer / spinTime);

            float rotationThisFrame = speed * Time.deltaTime;

            wheel.Rotate(0, 0, rotationThisFrame);

            totalRotation += rotationThisFrame; // cộng tổng độ quay

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("TOTAL ROTATION: " + totalRotation + " deg");
        Debug.Log("TOTAL ROTATION (loops): " + totalRotation / 360f);

        float finalAngle = wheel.eulerAngles.z;
        Debug.Log("FINAL ANGLE: " + finalAngle);

        int index = GetWheelIndex();

        Debug.Log("FINAL INDEX: " + index);

        if (!bonusMode)
            ShowReward(index);
        else
            ShowBonus(index);

        spinning = false;
    }

    int GetWheelIndex()
    {
        float angle = wheel.eulerAngles.z;

        float sector = 360f / rewardCount;

        float adjustedAngle = (360f - angle + wheelOffset) % 360f;

        int index = Mathf.FloorToInt(adjustedAngle / sector);

        Debug.Log("Wheel Angle: " + angle);
        Debug.Log("Adjusted Angle: " + adjustedAngle);
        Debug.Log("Index: " + index);

        return index;
    }

    void ShowReward(int index)
    {
        if (index >= rewards.Length) return;

        pendingReward = rewards[index];

        Debug.Log("REWARD INDEX: " + index);
        Debug.Log("REWARD COIN: " + pendingReward);

        rewardText.text = "+" + pendingReward;

        claimButton.gameObject.SetActive(true);
        bonusButton.gameObject.SetActive(true);
    }

    public void BonusSpin()
    {
        if (spinning) return;

        Debug.Log("BONUS SPIN START");

        bonusMode = true;

        claimButton.gameObject.SetActive(false);
        bonusButton.gameObject.SetActive(false);

        StartCoroutine(SpinWheelRoutine());
    }

    void ShowBonus(int index)
    {
        if (index >= bonusMultipliers.Length) return;

        pendingMultiplier = bonusMultipliers[index];

        int total = pendingReward * pendingMultiplier;

        Debug.Log("BONUS INDEX: " + index);
        Debug.Log("MULTIPLIER: x" + pendingMultiplier);
        Debug.Log("TOTAL REWARD: " + total);

        rewardText.text = total.ToString();

        claimButton.gameObject.SetActive(true);
    }

    public void ClaimReward()
    {
        int total = pendingReward * pendingMultiplier;

        Debug.Log("CLAIM REWARD");
        Debug.Log("BASE REWARD: " + pendingReward);
        Debug.Log("MULTIPLIER: " + pendingMultiplier);
        Debug.Log("TOTAL COIN ADD: " + total);

        CoinManager.instance.AddCoins(total);

        pendingReward = 0;
        pendingMultiplier = 1;
        bonusMode = false;

        spinPanel.SetActive(false);
    }
}