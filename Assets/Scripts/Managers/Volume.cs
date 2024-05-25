using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetGlobalVolume(Settings.volume);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0)
        {
            SetGlobalVolume(Settings.volume);
        }
        SetGlobalTimeVolume();
    }
    public static void SetGlobalVolume(float volume)
    {
        // Get all audio sources in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        // Iterate through each audio source and set its volume
        foreach (AudioSource audioSource in allAudioSources)
        {
            // Clamp the volume between 0 and 1
            audioSource.volume = Mathf.Clamp01(volume);
        }
    }

    void SetGlobalTimeVolume()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            if(audioSource.name != "AmbienceAudioSource") audioSource.pitch = Time.timeScale;
            // Clamp the volume between 0 and 1

        }
    }

}
