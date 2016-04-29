﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public interface IPianoKeyController
{
    void PlayPianoKey(float pitch);
    float Pitch { get; }
}
public class PianoKeyController : MonoBehaviour, IPointerEnterHandler ,IPianoKeyController
{
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    private float pitch = 1.0f;
    public float Pitch { get { return this.pitch; } }

    private AudioSource audioSource = null;
    private PianoKey pianoKey = null;

    private void Start()
    {
        this.audioSource = this.gameObject.AddComponent<AudioSource>();
        this.audioSource.clip = this.clip;
        this.pianoKey = new PianoKey(this as IPianoKeyController);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.pianoKey.PlayPianoKey();
    }
    public void PlayPianoKey(float pitch)
    {
        this.audioSource.Stop();
        this.audioSource.pitch = pitch;
        this.audioSource.Play();
    }
}

[Serializable]
public class PianoKey
{
    private float pitch = 1.0f;
    private IPianoKeyController pianoKey = null;

    public PianoKey(IPianoKeyController pianoKey)
    {
        this.pianoKey = pianoKey;
        this.pitch = this.pianoKey.Pitch;
    }

    public void PlayPianoKey()
    {
        this.pianoKey.PlayPianoKey(this.pitch);
    }
}
