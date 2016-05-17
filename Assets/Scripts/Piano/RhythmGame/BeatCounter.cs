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
		
	}
}

[Serializable]
public class BeatCounterContainer
{
	private IBeatCounter beatCounter = null;
	private float currentCounter = 0.0f;

	public BeatCounterContainer (IBeatCounter beatCounter)
	{
		this.beatCounter = beatCounter;	
	}

	public float SetCurrentCounter(float deltaTime)
	{
		this.currentCounter += deltaTime;
		if(this.currentCounter >= this.beatCounter.BarLength)
		{
			this.currentCounter -= this.beatCounter.BarLength;
		}
		return this.currentCounter;
	}

	public void ResetCurrentCounter()
	{
		this.currentCounter = 0.0f;
	}

	public float GetPeriodBar()
	{
		float periodBar = 60f / this.beatCounter.Bpm * this.beatCounter.BarLength;
		return periodBar;
	}
}

