using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeroSpawn : MonoBehaviour
{
    public Vector3 initHeroPosition;

    void Start()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play(AudioManager.AudioList.bgmForDict, true);

        if (SpeedrunManager.instance.nameInputed)
        {
            RemoveNameInput();
            SpawnHero();
        }
    }

    public void Skip()
    {
        SpeedrunManager.instance.nameInputed = true;
        SpawnHero();
        RemoveNameInput();
    }

    public void Confirm()
    {
        var heroName = GameObject.Find("/Name Input Convas/Name Input Panel/InputField").GetComponent<TMP_InputField>().text;

        if (!VerifyName(heroName))
        {
            var hint = GameObject.Find("/Name Input Convas/Name Input Panel/Hint").GetComponent<TMP_Text>();
            hint.text = "The name must be less than or equal to 10 characters and can only contains letters and numbers";
            return;
        }

        SpeedrunManager.instance.nameInputed = true;
        SpeedrunManager.instance.player = heroName;
        Debug.Log("Hero name: " + heroName);
        SpawnHero();
        RemoveNameInput();
    }

    // - The name must be less than or equal to 10 characters
    // - It can only contains letters and numbers
    bool VerifyName(string name)
    {
        if (name.Length > 10 || name.Length == 0)
        {
            return false;
        }
        foreach (char c in name)
        {
            if (!char.IsLetterOrDigit(c))
            {
                return false;
            }
        }
        return true;
    }

    void SpawnHero()
    {
        Vector3 heroPosition;
        if (GlobalState.instance != null)
        {
            heroPosition = GlobalState.instance.heroPosition;
        }
        else
        {
            heroPosition = initHeroPosition;
        }
        Instantiate(Resources.Load("Prefabs/Hero"), heroPosition, Quaternion.identity);
    }

    void RemoveNameInput()
    {
        Destroy(GameObject.Find("Name Input Convas"));
    }
}
