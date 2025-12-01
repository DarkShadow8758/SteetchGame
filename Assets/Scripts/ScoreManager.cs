using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI totalScoreText;
    [Tooltip("Prefab de texto flutuante (TextMeshProUGUI) para exibir +x pontos; nn esquece do script FloatingPointText")]
    public GameObject floatingTextPrefab;
    public Canvas hudCanvas;

    [Header("Final Score Panel, caso de tempo a gente bota isso aqui")]
    public GameObject finalScorePanel;
    public TextMeshProUGUI finalScoreText;

    private int totalPoints = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        UpdateTotalScoreUI();
    }

    public void AddPoints(int points, Vector3 worldPosition)
    {
        if (points == 0) return;
        totalPoints += points;
        UpdateTotalScoreUI();

        if (floatingTextPrefab != null && hudCanvas != null)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
            RectTransform canvasRect = hudCanvas.GetComponent<RectTransform>();
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, hudCanvas.worldCamera, out localPoint);
            GameObject go = Instantiate(floatingTextPrefab, hudCanvas.transform);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = localPoint;
            FloatingPointText ft = go.GetComponent<FloatingPointText>();
            if (ft != null)
            {
                ft.Show($"+{points} pontos");
            }
            else
            {
                var tmp = go.GetComponent<TextMeshProUGUI>();
                if (tmp != null) tmp.text = $"+{points} pontos";
                Destroy(go, 1.5f);
            }
        }
    }

    private void UpdateTotalScoreUI()
    {
        if (totalScoreText != null)
            totalScoreText.text = $"Pontos: {totalPoints}";
    }

    public void ShowFinalScore()
    {
        if (finalScorePanel != null)
        {
            finalScorePanel.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = $"Pontos finais: {totalPoints}";
        }
        else
        {
            Debug.Log($"Pontos finais: {totalPoints}");
        }
    }

    public int GetTotalPoints() => totalPoints;
}
