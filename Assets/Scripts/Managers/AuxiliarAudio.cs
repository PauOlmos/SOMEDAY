using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxiliarAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<AudioSource>() != null) gameObject.GetComponent<AudioSource>().volume = Settings.volume;

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<AudioSource>() != null) gameObject.GetComponent<AudioSource>().volume = Settings.volume;

    }
}
