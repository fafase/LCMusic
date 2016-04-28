using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class PianoKeyController : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private float pitch = 1.0f;

    private PianoKey pianoKey = null;

    private void Start()
    {
        this.pianoKey = new PianoKey(this.gameObject.AddComponent<AudioSource>(), this.clip, this.pitch);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.pianoKey.PlayPianoKey();
    }
}
[Serializable]
public class PianoKey
{
    private float pitch = 1.0f;
    private AudioSource audioSource = null;

    public PianoKey(AudioSource audioSource, AudioClip clip, float pitch)
    {
        this.audioSource = audioSource;
        this.audioSource.clip = clip;
        this.pitch = pitch;
    }

    public void PlayPianoKey()
    {
        this.audioSource.Stop();
        this.audioSource.pitch = this.pitch;
        this.audioSource.Play();
    }
}
