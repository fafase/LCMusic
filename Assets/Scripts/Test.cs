using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Test : MonoBehaviour 
{
	private AudioSource audioSource = null;
	public KeyCode[] keys;

	private void Start()
	{
		this.audioSource = this.gameObject.GetComponent<AudioSource> ();
	}
		

	private void CheckAudio(KeyCode key, float pitch)
	{
		if (Input.GetKeyDown (key)) 
		{
			this.audioSource.pitch = Mathf.Pow(2f, pitch / 12.0f);
			this.audioSource.Play ();
		}
	}
}
