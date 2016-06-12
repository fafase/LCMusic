using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using LCHelper;

public interface IWarmUp
{
	IEnumerable<MonoBehaviour> Keyboard { get; }
	void SetNewKey(IPianoKeyController previous, IPianoKeyController next);
	IMusicalScore ScoreLeft { get; }
	IMusicalScore ScoreRight { get; }
}
public class WarmUpController : MonoBehaviour , IWarmUp
{
	[SerializeField] private ScoreController scoreCtrlLeft = null;  
	[SerializeField] private ScoreController scoreCtrlRight = null;
	public MonoBehaviour [] buttons;
	public IEnumerable<MonoBehaviour> Keyboard { get{ return this.buttons as IEnumerable<MonoBehaviour>; } }

	public IMusicalScore ScoreLeft 
	{ 
		get
		{
			return this.scoreCtrlLeft as IMusicalScore;
		}
	}
	public IMusicalScore ScoreRight 
	{
		get
		{
			return this.scoreCtrlRight as IMusicalScore;
		}
	}

	private WarmUpContainer warmUp = null;
	private Image currentImage = null;

	private void Awake()
	{
		this.warmUp = new WarmUpContainer(this as IWarmUp);
	}	
	private void Start()
	{
		Note firstNote = this.warmUp.GetCurrentNote();
		this.currentImage = this.buttons[firstNote.key].GetComponent<Image>();
		this.currentImage.color = firstNote.KeyColor;
		this.warmUp.SetMusicalScore();
	}
	private void OnDestroy()
	{
		if(this.warmUp != null)
		{
			this.warmUp.Clean();
		}
	}

	public void SetNewKey(IPianoKeyController previous, IPianoKeyController next)
	{
		previous.PianoKeyImage.color = previous.OriginalColor;
		Note currentNote = this.warmUp.GetCurrentNote();
		next.PianoKeyImage.color = currentNote.KeyColor;
	}
}

[Serializable]
public class WarmUpContainer
{
	private Note [] notes = null;

	private IPianoKeyController[] pianoKeys = null;
	private int index = 0;
	private IWarmUp warmUp = null;

	private IPianoKeyController currentPianoKey = null;
	private Lesson currentLesson = null;

	public WarmUpContainer(IWarmUp warmUp)
	{
		this.warmUp = warmUp;
		this.pianoKeys = GetAllPianoKeyController(this.warmUp.Keyboard);

		foreach(IPianoKeyController pkc in this.pianoKeys)
		{
			pkc.RaiseKeyDown += IPianoKeyController_RaiseKeyDown;
		}

		this.currentLesson = Save.DeserializeFromPlayerPrefs<Lesson> (ConstString.CurrentData);
		this.notes = this.currentLesson.warmup.note;
		int temp = GetInitialIndex();
		this.currentPianoKey = this.pianoKeys[temp];
	}

	public void SetMusicalScore()
	{
		SetMusicalScore(this.warmUp.ScoreLeft);
		SetMusicalScore(this.warmUp.ScoreRight);
	}

	public void Clean()
	{
		foreach(IPianoKeyController pkc in this.pianoKeys)
		{
			if(pkc == null){ continue; }
			pkc.RaiseKeyDown += IPianoKeyController_RaiseKeyDown;
		}
	}

	private void IPianoKeyController_RaiseKeyDown (object sender, KeyDownEventArg e)
	{
		IPianoKeyController pkc = e.pianoKeyCtrl;
		if(pkc == this.currentPianoKey)
		{
			IPianoKeyController previous = this.currentPianoKey;
			int temp = GetKeyIndex();
			this.currentPianoKey = this.pianoKeys[temp];
			this.warmUp.SetNewKey(previous, this.currentPianoKey);
		}
	}

	public int GetInitialIndex(){ return this.notes[0].key; }  
	public int GetKeyIndex()
	{
		if(++this.index >= this.notes.Length){ this.index = 0; }
		return this.notes[this.index].key;
	}

	public IPianoKeyController[] GetAllPianoKeyController(IEnumerable<MonoBehaviour> btns)
	{
		List<IPianoKeyController> list = new List<IPianoKeyController>();
		foreach(MonoBehaviour  btn in btns)
		{
			IPianoKeyController pkc = btn.GetComponent<IPianoKeyController>();
			list.Add(pkc);
		}
		return list.ToArray();
	}

	public Note GetCurrentNote()
	{
		return this.notes[this.index];
	}

	private void SetMusicalScore(IMusicalScore score)
	{
		if(score != null){ score.InitWithLesson(this.currentLesson); }
	}
}
