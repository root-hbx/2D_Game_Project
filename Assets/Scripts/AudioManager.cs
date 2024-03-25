using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;
    public AudioClip bgmWin;
    public AudioClip bgmFail;
    public AudioClip bgmForPlaying;
    public AudioClip bgmForDict;

    public AudioClip bgmShoot;

    List<AudioSource> audios = new List<AudioSource>();

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
            return;
        }

        for(int i=0; i<5; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        

    }

    public void Play(int index, string name, bool isLoop)
    {
        var clip = GetAudioClip(name);
        if (clip != null)
        {
            var audio = audios[index];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
        }
    }

    AudioClip GetAudioClip(string name)
    {
        switch(name)
        {
            case "bgmWin":
                return bgmWin;
            case "bgmFail":
                return bgmFail;
            case "bgmForPlaying":
                return bgmForPlaying;
            case "bgmForDict":
                return bgmForDict;
            case "bgmShoot":
                return bgmShoot;
        }
        return null;
    }

}