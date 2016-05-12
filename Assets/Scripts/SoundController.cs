using UnityEngine;

public class LCAudioPlayer
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
