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
}
public class WarmUpController : MonoBehaviour , IWarmUp
{
	public MonoBehaviour [] buttons;
	public IEnumerable<MonoBehaviour> Keyboard { get{ return this.buttons as IEnumerable<MonoBehaviour>; } }

	public Color onColor = Color.blue;

	private WarmUpContainer warmUp = null;
	private Image currentImage = null;

	private void Awake()
	{
		this.warmUp= new WarmUpContainer(this as IWarmUp);
	}	
	private void Start()
	{
		this.currentImage = this.buttons[this.warmUp.GetInitialIndex()].GetComponent<Image>();
		this.currentImage.color = onColor;
	}
	private void OnDestroy()
	{
		this.warmUp.Clean();
	}

	public void SetNewKey(IPianoKeyController previous, IPianoKeyController next)
	{
		previous.PianoKeyImage.color = previous.OriginalColor;
		next.PianoKeyImage.color = onColor;
	}
}

[Serializable]
public class WarmUpContainer
{
	private int [] warmUpCollection = null;

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
		this.warmUpCollection = this.currentLesson.warmup.keylist;
		int temp = GetInitialIndex();
		this.currentPianoKey = this.pianoKeys[temp];
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

	public int GetInitialIndex(){ return this.warmUpCollection[0]; }  
	public int GetKeyIndex()
	{
		if(++this.index >= this.warmUpCollection.Length){ this.index = 0; }
		return this.warmUpCollection[this.index];
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
}
