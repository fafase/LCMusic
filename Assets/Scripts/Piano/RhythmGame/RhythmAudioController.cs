using UnityEngine;
using System.Collections;
using System;

public class RhythmAudioController : MonoBehaviour {

	// Contains 7 audio clips
	public AudioClip[] clips;
	private AudioSource [] audioSources = null;
	private int amountClip= -1;

	[SerializeField] private AudioClip buzzClip = null;
	private AudioSource buzzSource = null;

	private void Awake()
	{
		if(this.buzzClip == null){ throw new NullReferenceException("Missing buzz sound"); }
	}

	public void Init(int amount)
	{
		this.amountClip = CheckAmountGreaterThan0LessThan7(amount);
		this.audioSources = new AudioSource[amount];
		GameObject audioObject = new GameObject("AudioObject");
		for(int i = 0; i < amount; i++)
		{
			this.audioSources[i] = audioObject.AddComponent<AudioSource>();
			this.audioSources[i].playOnAwake = false;
		}
		this.buzzSource = audioObject.AddComponent<AudioSource>();
		this.buzzSource.playOnAwake = false;
		this.buzzSource.clip = this.buzzClip;

		switch(amount)
		{
		case 1:
			SetOnePad();
			break;
		case 2:
			SetTwoPads();
			break;
		case 3:
			SetThreePads();
			break;
		case 4:
			SetFourPads();
			break;
		case 5:
			SetFivePads();
			break;
		case 6:
			SetSixPads();
			break;
		case 7:
			SetSevenPads();
			break;
		}
	}

	public void PlayClipSuccess(int index)
	{
		CheckAmountGreaterThan0LessThan7(index, this.amountClip);
		this.audioSources[index].Play();
	}

	public void PlayBuzz()
	{
		this.buzzSource.Play();
	}

	private int CheckAmountGreaterThan0LessThan7(int amount , int range = 7)
	{
		if (amount < 0 || amount > range)
		{
			throw new Exception("Wrong amount in RhythmAudioController"); 
		}
		return amount;
	}

	private void SetOnePad(){}

	private void SetTwoPads()
	{
		this.audioSources[0].clip = this.clips[0];
		this.audioSources[1].clip = this.clips[2];
	}

	private void SetThreePads()
	{
		this.audioSources[0].clip = this.clips[0];
		this.audioSources[1].clip = this.clips[2];
		this.audioSources[2].clip = this.clips[4];
	}
	private void SetFourPads(){}
	private void SetFivePads(){}
	private void SetSixPads(){}
	private void SetSevenPads(){}
}
