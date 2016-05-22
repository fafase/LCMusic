using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(RhythmController))]
public class UIRhythmController : MonoBehaviour 
{
	[SerializeField] private Text styleNameText = null; 
	[SerializeField] private Text streakText = null;
	[SerializeField] private Text bpmText = null;
	[SerializeField] private GameObject startUIObj = null;

	private RhythmController rhythmController = null;

	private void Awake()
	{
		if(this.styleNameText == null){ throw new NullReferenceException("StyleNameText null"); }
		if(this.streakText == null){ throw new NullReferenceException("StreakText null"); }
		if(this.bpmText == null){ throw new NullReferenceException("BPMText null"); }
		this.rhythmController = this.gameObject.GetComponent<RhythmController>();
	}

	public void SetStyleNameText(string text)
	{
		this.styleNameText.text = text;
	}

	public void SetStreakText(string text)
	{
		this.streakText.text = text;
	}

	public void StartRhythmGame()
	{
		this.startUIObj.SetActive(false);
		this.rhythmController.StartRhythmGame();
	}
	public void SetUI(string uiText, int bpm)
	{
		this.startUIObj.SetActive(true);
		this.startUIObj.GetComponentInChildren<Text>().text = uiText;
		SetBpmText(bpm.ToString());
	}

	public void SetBpmText(string text)
	{
		string temp = "Bpm : ";
		this.bpmText.text = temp + text;
	}
}
