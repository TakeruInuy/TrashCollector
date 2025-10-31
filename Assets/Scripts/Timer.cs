using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{

    [SerializeField] private float startTime = 60f;
    [SerializeField] private TextMeshProUGUI timerText;

    
    private float currentTime;
    private bool isRunning = true;
    public UnityEvent onTimerEnd;
    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(currentTime, 0f); 

        ScoreManager.Instance.timerMultiplier = currentTime/startTime;

        if (timerText != null)
            timerText.text = FormatTime(currentTime);

        if (currentTime <= 0f)
        {
            isRunning = false;
            onTimerEnd.Invoke();
        }
    }


    private string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60);
        int seconds = Mathf.FloorToInt(t % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public void SetTimer(bool timerSet)
    {
        isRunning = timerSet;
    }

    public void ResetTimer()
    {
        SetTimer(true);
        currentTime = startTime;
    }
}

