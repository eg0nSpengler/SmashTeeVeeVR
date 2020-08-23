using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunAudioHandler : MonoBehaviour
{
    [Header("Audio References")]
    public AudioClip FireSound;
    public AudioClip ReloadSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (!FireSound || !ReloadSound)
        {
            Debug.LogWarning(gameObject.name.ToString() + " is missing an Audio reference!");
        }

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }
}
