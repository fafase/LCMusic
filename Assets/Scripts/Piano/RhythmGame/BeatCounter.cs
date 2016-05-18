using UnityEngine;
using System.Collections;
using System;

public interface IBeatCounter
{
	float BarLength { get; } 
	float Bpm { get; }
}

public class BeatCounter : MonoBehaviour , IBeatCounter
{
	private BeatCounterContainer beatCounter = null;
	private float barLength = 0.0f;
	public float BarLength { get { return this.barLength; } }

	private float bpm = 0.0f;
	public float Bpm { get {  return this.bpm; } }

	public void Init(IPadController padCtrl, float newBpm)
	{
		this.barLength = padCtrl.BarBeat;
		this.bpm = newBpm;
		this.beatCounter = new BeatCounterContainer(this as IBeatCounter);
	}

	private void Update () 
	{
		float elapsedTime = this.beatCounter.GetElapsedTime();
		this.beatCounter.UpdateCurrentCounter(elapsedTime);
		Debug.Log(this.beatCounter.CurrentCounter);
	}
}

[Serializable]
public class BeatCounterContainer
{
	private IBeatCounter beatCounter = null;
	private float currentCounter = 0.0f;
	public float CurrentCounter { get { return this.currentCounter; } }
	private DateTime previousDateTime;

	public BeatCounterContainer (IBeatCounter beatCounter)
	{
		this.beatCounter = beatCounter;	
		this.previousDateTime = DateTime.Now;
	}

	public void UpdateCurrentCounter(float elapsedTime)
	{
		this.currentCounter += elapsedTime;
		if(this.currentCounter >= GetPeriodBarInSec())
		{
			this.currentCounter -= GetPeriodBarInSec();
		}
	}

	public void ResetCurrentCounter()
	{
		this.currentCounter = 0.0f;
	}

	public float GetPeriodBarInSec()
	{
		float periodBar = 60f / this.beatCounter.Bpm * this.beatCounter.BarLength;
		return periodBar;
	}

	public float GetElapsedTime()
	{
		DateTime now = DateTime.Now;
		TimeSpan ts = now.Subtract(this.previousDateTime);
		this.previousDateTime = now;
		float elapsedTime = ts.Milliseconds / 1000f;
		return elapsedTime;
	}
}

