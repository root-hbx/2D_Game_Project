using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSound : MonoBehaviour
{
    public enum SoundList {
        JUMP,
        SHOOT,
    }
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Play(SoundList name)
    {
        AudioClip clip = GetAudioClip(name);
        audioSource.clip = clip;
        audioSource.Play();
    }

    AudioClip GetAudioClip(SoundList name)
    {
        switch (name)
        {
            case SoundList.JUMP:
                return Resources.Load<AudioClip>("Audio/Jump");
            case SoundList.SHOOT:
                return Resources.Load<AudioClip>("Audio/Shoot");
        }
        return null;
    }

    // Update is called once per frame

}
