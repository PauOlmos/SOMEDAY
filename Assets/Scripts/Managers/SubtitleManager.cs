using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI subtitles;
    public AudioClip currentAudioClip;
    public float audioTimer = 0.0f;
    public AudioSource bossDialogAudioSource;
    public string subtitleText;
    public bool canReproduceAudio = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAudioClip != null)
        {
            audioTimer += Time.deltaTime;
            if(audioTimer < currentAudioClip.length && canReproduceAudio == true)
            {
                bossDialogAudioSource.clip = currentAudioClip;
                if (Settings.subtitles == true)
                {
                    subtitles.gameObject.SetActive(true);
                    subtitles.text = subtitleText;
                    subtitles.fontSize = 8 + Settings.subtitlesSize * 14;
                }
               
                if(!bossDialogAudioSource.isPlaying) bossDialogAudioSource.Play();
            }
            else
            {
                canReproduceAudio = false;
                audioTimer = 0.0f;
                subtitles.gameObject.SetActive(false);
                currentAudioClip = null;
            }
        }
    }

}
