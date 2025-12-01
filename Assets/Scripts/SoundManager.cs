using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gerencia a reprodução de músicas (background music) para o jogo.
/// - Mantenha este objeto com `DontDestroyOnLoad` para manter a música entre cenas
/// - Coloque as faixas em `musicTracks` no Inspector (AudioClips)
/// - Chame os métodos públicos via UI (Button -> OnClick) ou de outros scripts.
/// </summary>
public class SoundManager : MonoBehaviour
{
    [Header("Músicas")]
    [Tooltip("Lista de AudioClips que podem ser reproduzidos.")]
    public List<AudioClip> musicTracks = new List<AudioClip>();

    [Header("Configuração")]
    [Tooltip("Se true, começa a reproduzir automaticamente na cena inicial.")]
    public bool playOnAwake = true;
    [Tooltip("Índice inicial a ser reproduzido na lista musicTracks (se playOnAwake estiver ativo).")]
    public int startIndex = 0;
    [Tooltip("Volume padrão (0..1)")]
    [Range(0f, 1f)]
    public float volume = 1f;
    [Tooltip("Se true, o mesmo áudio ficará em loop. Caso false, NextTrack() será chamado quando terminar (se houver).")]
    public bool loopTrack = true;

    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;
    public int CurrentTrackIndex { get; private set; } = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = loopTrack;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    private void Start()
    {
        if (playOnAwake && musicTracks != null && musicTracks.Count > 0)
        {
            if (startIndex < 0 || startIndex >= musicTracks.Count)
                startIndex = 0;
            PlayTrack(startIndex);
        }
    }

    public void PlayTrack(int index)
    {
        if (musicTracks == null || musicTracks.Count == 0)
            return;
        if (index < 0 || index >= musicTracks.Count)
            return;

        if (audioSource.isPlaying && CurrentTrackIndex == index)
            return;//ja ta tocano

        CurrentTrackIndex = index;
        audioSource.clip = musicTracks[index];
        audioSource.loop = loopTrack;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void PlayTrackByName(string name)
    {
        if (string.IsNullOrEmpty(name) || musicTracks == null)
            return;
        for (int i = 0; i < musicTracks.Count; i++)
        {
            if (musicTracks[i] != null && musicTracks[i].name == name)
            {
                PlayTrack(i);
                return;
            }
        }
    }

    public void NextTrack()
    {
        if (musicTracks == null || musicTracks.Count == 0) return;
        int next = CurrentTrackIndex + 1;
        if (next >= musicTracks.Count) next = 0;
        PlayTrack(next);
    }

    public void PreviousTrack()
    {
        if (musicTracks == null || musicTracks.Count == 0) return;
        int prev = CurrentTrackIndex - 1;
        if (prev < 0) prev = musicTracks.Count - 1;
        PlayTrack(prev);
    }

    public void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
    public void TogglePause()
    {
        if (audioSource == null) return;
        if (audioSource.isPlaying) audioSource.Pause(); else audioSource.UnPause();
    }
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource != null)
            audioSource.volume = volume;
    }

    public void CrossfadeToTrack(int index, float fadeDuration = 1f)
    {
        if (musicTracks == null || index < 0 || index >= musicTracks.Count) return;
        StartCoroutine(CrossfadeCoroutine(index, fadeDuration));
    }

    private IEnumerator CrossfadeCoroutine(int index, float duration)
    {
        if (audioSource == null) yield break;

        float startVolume = audioSource.volume;
        float t = 0f;
        if (!audioSource.isPlaying)
        {
            PlayTrack(index);
            yield break;
        }

        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
        CurrentTrackIndex = index;
        audioSource.clip = musicTracks[index];
        audioSource.loop = loopTrack;
        audioSource.Play();
    }
}
