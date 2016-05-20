using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIRhythmController : MonoBehaviour 
{
	[SerializeField] private Text styleNameText = null; 
	[SerializeField] private Text streakText = null;

	private void Awake()
	{
		if(this.styleNameText == null){ throw new NullReferenceException("StyleNameText null"); }
		if(this.streakText == null){ throw new NullReferenceException("StreakText null"); }
	}

	public void SetStyleNameText(string text)
	{
		this.styleNameText.text = text;
	}

	public void SetStreakText(string text)
	{
		this.streakText.text = text;
	}
}
