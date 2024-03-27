using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public struct SpeedrunData
{
    public string player;
    public int time;
}

public class SpeedrunManager : MonoBehaviour
{
    public static SpeedrunManager instance;

    static string KEY = "906dafba3cc72263ffb443d8c15fdd73f96d3917aa6ed549454a8f79fe590b35";
    static byte[] key = StringToByteArray(KEY);

    public bool nameInputed;
    public string player;
    public int level;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Record(int time)
    {
        if (player == null || player == "")
        {
            return;
        }
        Upload(new SpeedrunData { player = player, time = time }, level);
    }

    void Upload(SpeedrunData data, int level)
    {
        string json = JsonUtility.ToJson(data);
        byte[] encrypted = Encrypt(json);
        StartCoroutine(UploadCoroutine(encrypted, level));
    }

    IEnumerator UploadCoroutine(byte[] payload, int level)
    {
        var request = new UnityWebRequest("https://game-leaderboard.azurewebsites.net/leaderboard/" + level, "POST", new DownloadHandlerBuffer(), new UploadHandlerRaw(payload));
        request.SetRequestHeader("Content-Type", "application/octet-stream");
        yield return request.SendWebRequest();
    }

    byte[] Encrypt(string data)
    {
        byte[] encrypted;
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var encrypted_data = encryptor.TransformFinalBlock(System.Text.Encoding.UTF8.GetBytes(data), 0, data.Length);
            encrypted = new byte[aes.IV.Length + encrypted_data.Length];
            Array.Copy(aes.IV, 0, encrypted, 0, aes.IV.Length);
            Array.Copy(encrypted_data, 0, encrypted, aes.IV.Length, encrypted_data.Length);
        }
        return encrypted;
    }

    static byte[] StringToByteArray(string hex)
    {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }
}
