using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Diagnostics;
using System.Collections.Generic;

public interface IBeatCounter
{
	float BarLength { get; } 
	float Bpm { get; }
	void SetBeatCounterRunning(bool value);
	void Init(float newBarLength);
	void SetBpm(int value);
}

public interface IMetronome
{
	event EventHandler <BpmBeatArg> RaiseBpm;
	event EventHandler <BpmArg> RaiseChangeBpm;
}

[RequireComponent(typeof(AudioSource))]
public class BeatCounter : MonoBehaviour , IBeatCounter, IMetronome
{
	IEnumerable <IPadController> padCtrls = null;
	private AudioSource audioSource = null;
	private Metronome metronome = null; 
	private bool isGameRunning = false;
	private float barLength = 0.0f;
	public float BarLength { get { return this.barLength; } }

	private Action updateBeat = null;

	public float Bpm { get { return this.metronome.Bpm; } }
	public event EventHandler<BpmArg> RaiseChangeBpm;
	protected void OnChangeBpm(BpmArg args)
	{
		if (RaiseChangeBpm != null) {
			RaiseChangeBpm (this, args);
		}
	}
	public event EventHandler<BpmBeatArg> RaiseBpm;
	protected void OnBpm(BpmBeatArg args)
	{
		if (RaiseBpm != null) 
		{
			RaiseBpm (this, args);
		}
	}

	private int barBeat = 0;

	private void Awake()
	{
		this.audioSource = this.gameObject.GetComponent<AudioSource> ();
	}

	public void Init(float newBarLength)
	{
		this.barLength = newBarLength;
		this.metronome = new Metronome (this.barLength);
	}

	private void SetUpdateBeat(object sender, EventArgs arg)
	{
		this.updateBeat = UpdateBeat;
		this.metronome.SetBarTimerOnStart();
		RaiseBpm -= SetUpdateBeat;
	}
		
	private void Update()
	{
		if(updateBeat != null) 
		{
			updateBeat();
		}
		UpdateMetronome();
	}

	public void SetBpm(int value)
	{
		this.metronome.SetBpm (value);
		OnChangeBpm (new BpmArg(this.Bpm));
	}

	private void UpdateMetronome()
	{
		if(this.metronome.SetBeatTimer(Time.deltaTime)== false) { return; }
		this.audioSource.Play ();
		OnBpm (new BpmBeatArg(this.barBeat++));			// Reset movement speed
	}

	private void UpdateBeat()
	{
		if(this.metronome.SetBarTimer(Time.deltaTime) == true)
		{
			this.metronome.SetBeatCounter();
			foreach(IPadController pc in this.padCtrls)
			{
				pc.ResetBeat();
			}
		}
		// Run all PadController
		foreach(IPadController pc in this.padCtrls)
		{
			pc.CheckTimeForBeat(this.metronome.BarTimer);
		}
	}

	public void GetPadControllers(IEnumerable<IPadController> pcs) { this.padCtrls = pcs; }
	public void SetBeatCounterRunning(bool value)
	{
		if(value == true)
		{
			RaiseBpm += SetUpdateBeat;
			return;
		}
		RaiseBpm -= SetUpdateBeat;
	}
}

[Serializable]
public class Metronome
{
	public float Bpm { get; private set; }
	private float barLength = 0f;

	private float beatTimer = 0f;
	private float beatCounter = 0f;
	public float BeatCounter { get { return this.beatCounter; } }

	/// <summary>
	/// Provide the elapsed time in the current bar (from 0 to bar length)
	/// </summary>
	/// <value>The bar timer.</value>
	public float BarTimer { get; private set;}
	private float barCounter = 0f;

	public Metronome (float barLength)
	{
		this.barLength = barLength;
		SetBeatCounter ();
		SetBarCounter();
	}

	public float SetBpm(int value)
	{
		this.Bpm = Mathf.Clamp (value, 20f, 200f);
		SetBeatCounter();
		SetBarCounter();
		return this.Bpm;
	}

	public bool UpdateMetronome(float deltaTime)
	{
		SetBarTimer(deltaTime);
		return SetBeatTimer(deltaTime);
	}
	public bool SetBeatTimer(float deltaTime)
	{
		this.beatTimer += deltaTime;
		if (this.beatTimer < (this.BeatCounter)) { return false; }
		this.beatTimer -= this.BeatCounter; 
		return true;
	}
	public bool SetBarTimer(float deltaTime)
	{
		this.BarTimer += deltaTime;
		if(this.BarTimer < this.barCounter){ return false; }
		this.BarTimer -= this.barCounter;
		return true;
	}

	public void SetBeatCounter() { this.beatCounter = 60f / this.Bpm; }
	public void SetBarCounter()
	{
		this.barCounter = 60f / this.Bpm * this.barLength;
	}

	public void SetBarTimerOnStart()
	{
		float temp = this.beatTimer - (float)Math.Truncate(this.beatTimer);
		this.BarTimer = temp;
	}
}

public class BpmArg : EventArgs
{
	public readonly float bpm;

	public BpmArg ( float newBpm)
	{
		this.bpm = newBpm;
	}
}
public class BpmBeatArg : EventArgs
{
	public readonly int count;

	public BpmBeatArg (int newCount)
	{
		this.count = newCount;
	}
}

