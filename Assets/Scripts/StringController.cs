using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class StringController : MonoBehaviour 
{
	private AudioSource audioSource = null;
	public uint currentFret = 0;
	private void Awake()
	{
		this.audioSource = this.gameObject.GetComponent<AudioSource> ();

	}
	public void SetCurrentFret(uint newFret)
	{
		this.currentFret = newFret;
	}

	public void PlayCurrentFret()
	{
        LCAudioPlayer.PlaySound(this.audioSource, this.currentFret);
	}
}

