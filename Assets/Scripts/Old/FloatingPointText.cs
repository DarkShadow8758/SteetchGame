using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Controla a animação e destruição do texto flutuante de pontos.
/// Deve ser colocado no prefab que é instanciado pelo ScoreManager.
/// </summary>
public class FloatingPointText : MonoBehaviour
{
    public float moveSpeed = 30f; // units in canvas space per second
    public float lifetime = 1.25f;
    public Vector2 moveDirection = new Vector2(0, 1);
    public float fadeDuration = 0.5f;

    private TextMeshProUGUI tmp;
    private CanvasGroup cg;
    private float timer = 0f;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        cg = gameObject.AddComponent<CanvasGroup>();
    }

    public void Show(string text)
    {
        if (tmp != null) tmp.text = text;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        timer = 0f;
        Vector3 startPos = transform.localPosition;
        while (timer < lifetime)
        {
            float dt = Time.deltaTime;
            timer += dt;
            transform.localPosition += (Vector3)(moveDirection.normalized * moveSpeed * dt);
            if (timer > lifetime - fadeDuration)
            {
                float fadeT = (timer - (lifetime - fadeDuration)) / fadeDuration;
                if (cg != null) cg.alpha = Mathf.Lerp(1f, 0f, fadeT);
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
