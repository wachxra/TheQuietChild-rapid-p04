using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundEffect
{
    public string keyId;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(-3f, 3f)]
    public float pitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Sound Effects List")]
    public List<SoundEffect> sfxList = new List<SoundEffect>();

    private Dictionary<string, SoundEffect> sfxDict = new Dictionary<string, SoundEffect>();

    [Header("Walking SFX Settings")]
    public string walkingKeyId;
    private bool isWalkingPlaying = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var sfx in sfxList)
        {
            if (!sfxDict.ContainsKey(sfx.keyId))
                sfxDict.Add(sfx.keyId, sfx);
            else
                Debug.LogWarning($"Duplicate SFX keyId: {sfx.keyId}");
        }
    }

    #region BGM
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
            bgmSource.volume = Mathf.Clamp01(volume);
    }
    #endregion

    #region SFX
    public void PlaySFX(string keyId)
    {
        if (sfxSource == null)
        {
            return;
        }

        if (sfxDict.TryGetValue(keyId, out SoundEffect sfx))
        {
            Debug.Log($"Playing SFX: {keyId} | Volume: {sfx.volume} | Pitch: {sfx.pitch}");
            sfxSource.pitch = sfx.pitch;
            sfxSource.PlayOneShot(sfx.clip, sfx.volume);
        }
        else
        {
            Debug.LogWarning($"SFX Key not found: {keyId}");
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = Mathf.Clamp01(volume);
    }
    #endregion

    #region Walking Loop
    public void PlayWalkingLoop()
    {
        if (sfxSource == null || isWalkingPlaying) return;

        if (!sfxDict.TryGetValue(walkingKeyId, out SoundEffect sfx))
        {
            Debug.LogWarning($"Walking SFX key not found: {walkingKeyId}");
            return;
        }

        sfxSource.Stop();
        sfxSource.clip = sfx.clip;
        sfxSource.volume = sfx.volume;
        sfxSource.pitch = sfx.pitch;
        sfxSource.loop = true;
        sfxSource.Play();

        isWalkingPlaying = true;
    }

    public void StopWalkingLoop()
    {
        if (sfxSource == null || !isWalkingPlaying) return;

        sfxSource.Stop();
        sfxSource.loop = false;
        sfxSource.clip = null;

        isWalkingPlaying = false;
    }
    #endregion
}