using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
namespace LCHelper
{
    public static class LCAudioPlayer
    {
        public static void PlaySound(AudioSource audioSource, float pitch)
        {
            audioSource.pitch = Mathf.Pow(2f, pitch / 12.0f);
            audioSource.Play();
        }
        public static void SetPitch(AudioSource audioSource, float pitch)
        {
            audioSource.Stop();
            audioSource.pitch = Mathf.Pow(2f, pitch / 12.0f);
        }
    }

    public static class Save
    {
        public static void SerializeInPlayerPrefs(string key, object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            PlayerPrefs.SetString(key, Convert.ToBase64String(ms.GetBuffer()));
        }
        public static T DeserializeFromPlayerPrefs<T>(string key) where T : class
        {
            string str = PlayerPrefs.GetString(AppController.CurrentData, null);
            if (str == null) { return null; }
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(str));
            return bf.Deserialize(ms) as T;
        }
    }
}