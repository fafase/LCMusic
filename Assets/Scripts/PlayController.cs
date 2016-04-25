using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayController : MonoBehaviour 
{
	private IEnumerable<ISound> sounds = null;

	private void Start()
	{
		var s = FindObjectsOfType<ISound> ();
		Debug.Log (s.Length);
		this.sounds = s as IEnumerable<ISound>;
	}

	private void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			PlayAllSound ();
		}
	}

	public void PlayAllSound()
	{
		foreach (ISound sound in this.sounds) 
		{
			sound.PlaySound ();
		}
	}
}

public abstract class ISound :MonoBehaviour 
{
	public abstract void PlaySound();
}
