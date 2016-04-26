using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class StringController : MonoBehaviour 
{
	private AudioSource audioSource = null;
    [SerializeField] private EmptyString emptyString = null;
	public uint currentFret = 0;

    private void Awake()
	{
		this.audioSource = this.gameObject.GetComponent<AudioSource> ();
        if (this.emptyString == null)
        {
            Debug.LogError("[LCMusic] Missing EmptyString " + this.name);
        }
	}
	public void SetCurrentFret(uint newFret)
	{
		this.currentFret = newFret;
        LCAudioPlayer.SetPitch(this.audioSource, this.currentFret);
    }

	public void PlayCurrentFret()
	{
        if (this.currentFret == 0)
        {
            // Check for Empty String
            if (this.emptyString.IsStringSetOn() == false)
            {
                return;
            }
        }
        LCAudioPlayer.PlaySound(this.audioSource, this.currentFret);
	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayCurrentFret();
        }
    }
}

