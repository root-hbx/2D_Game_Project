using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioList
    {
        bgmWin,
        bgmFail,
        bgmForPlaying,
        bgmForDict,
        bgmShoot
    }

    private AudioSource audioSource;

    private void Awake()
    {
        
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {

    }

    void Update()
    {


    }

    public void Play(AudioList name, bool isLoop)
    {
        Debug.Log(audioSource);
        var clip = GetAudioClip(name);
        audioSource.clip = clip;
        audioSource.loop = isLoop;
        audioSource.Play();
    }

    AudioClip GetAudioClip(AudioList name)
    {
        switch (name)
        {
            case AudioList.bgmWin:
                return Resources.Load<AudioClip>("Audio/bgmWin");
            case AudioList.bgmFail:
                return Resources.Load<AudioClip>("Audio/bgmFail");
            case AudioList.bgmForPlaying:
                return Resources.Load<AudioClip>("Audio/bgmForPlaying");
            case AudioList.bgmForDict:
                return Resources.Load<AudioClip>("Audio/bgmForDict");
            case AudioList.bgmShoot:
                return Resources.Load<AudioClip>("Audio/bgmShoot");
        }
        return null;
    }

}