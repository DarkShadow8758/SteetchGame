using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Configurações")]
    [Tooltip("Duração do timer em segundos (ex: 120 = 2 minutos)")]
    public float durationSeconds = 120f;
    public bool autoStart = true;
    public bool callSceneNextOnFinish = true;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    [Tooltip("Se true, mostra mm:ss. Se false, mostra segundos decimais.")]
    public bool showMinutesSeconds = true;

    [Header("Eventos")]
    public UnityEvent onTimerStarted;
    public UnityEvent onTimerStopped;
    public UnityEvent onTimerFinished;

    private float remainingTime;
    private bool running = false;
    private bool finished = false;

    private void Start()
    {
        remainingTime = durationSeconds;
        UpdateTimerUI();
        if (autoStart)
            StartTimer();
    }

    private void Update()
    {
        if (!running || finished) return;
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            finished = true;
            running = false;
            UpdateTimerUI();
            onTimerFinished?.Invoke();
            if (callSceneNextOnFinish)
            {
                var sceneController = FindObjectOfType<SceneController>();
                if (sceneController != null)
                    sceneController.NextScene();
                else
                    Debug.LogWarning("SceneController não encontrado: não é possível ir para a próxima cena.");
            }
            return;
        }
        UpdateTimerUI();
    }

    public void StartTimer()
    {
        if (durationSeconds <= 0f) return;
        running = true;
        finished = false;
        onTimerStarted?.Invoke();
    }

    public void StopTimer()
    {
        running = false;
        onTimerStopped?.Invoke();
    }

    public void ResetTimer()
    {
        remainingTime = durationSeconds;
        finished = false;
        UpdateTimerUI();
    }

    public void SetDuration(float seconds, bool resetNow = true)
    {
        durationSeconds = Mathf.Max(0f, seconds);
        if (resetNow) ResetTimer();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;
        if (showMinutesSeconds)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
            timerText.text = remainingTime.ToString("F1");
    }
}
