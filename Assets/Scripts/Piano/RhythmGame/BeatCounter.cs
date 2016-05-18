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
}

public interface IMetronome
{
	event EventHandler <BpmBeatArg> RaiseBpm;
	event EventHandler <BpmArg> RaiseChangeBpm;

	float ElapsedTime { get; }
}

[RequireComponent(typeof(AudioSource))]
public class BeatCounter : MonoBehaviour , IBeatCounter, IMetronome
{
	[SerializeField] private Text bpmText = null;
	IEnumerable <IPadController> padCtrls = null;
	private AudioSource audioSource = null;
	private MetronomeContainer metronome = null; 
	private BeatCounterContainer beatCounter = null;

	private float barLength = 0.0f;
	public float BarLength { get { return this.barLength; } }

	public float Bpm { get { return this.metronome.Bpm; } }
	public float ElapsedTime { get{ return this.metronome.ElapsedTime; } }
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

	private void Start()
	{
		if (this.bpmText == null) 
		{
			throw new NullReferenceException ("Missing bpmText");
		}
		this.metronome = new MetronomeContainer ();
		this.audioSource = this.gameObject.GetComponent<AudioSource> ();
		this.bpmText.text = ((int)this.metronome.Bpm).ToString ();
	}

	public void Init(float newBarLength)
	{
		this.barLength = newBarLength;
		this.beatCounter = new BeatCounterContainer(this as IBeatCounter);
	}

	private void Update()
	{
		UpdateMetronome();
		UpdateBeat();
	}

	public void SetBpm(int value)
	{
		this.bpmText.text = ((int)this.metronome.SetBpm (value)).ToString ();
		OnChangeBpm (new BpmArg(this.Bpm));
	}

	public float GetElapsedTime()
	{
		return this.metronome.ElapsedTime;
	}

	private void UpdateMetronome()
	{
		if(this.metronome.UpdateMetronome(Time.deltaTime)== false) { return; }
		this.audioSource.Play ();
		OnBpm (new BpmBeatArg(this.barBeat++));			// Reset movement speed
	}

	private void UpdateBeat()
	{
		if(this.beatCounter.UpdateCurrentCounter(Time.deltaTime / this.metronome.Counter) == true)
		{
			this.metronome.SetCounter();
			foreach(IPadController pc in this.padCtrls)
			{
				pc.ResetBeat();
			}
		}
		// Run all PadController
		foreach(IPadController pc in this.padCtrls)
		{
			pc.CheckTimeForBeat(this.beatCounter.CurrentCounter);
		}
	}

	public void GetPadControllers(IEnumerable<IPadController> pcs) { this.padCtrls = pcs; }
}

[Serializable]
public class BeatCounterContainer
{
	private IBeatCounter beatCounter = null;
	private float currentCounter = 0.0f;
	public float CurrentCounter { get { return this.currentCounter; } }

	public BeatCounterContainer (IBeatCounter beatCounter)
	{
		this.beatCounter = beatCounter;	
	}

	public bool UpdateCurrentCounter(float deltaTime)
	{
		this.currentCounter += deltaTime;
		if(this.currentCounter >= this.beatCounter.BarLength)
		{
			this.currentCounter -=  this.beatCounter.BarLength;
			return true;
		}
		return false;
	}

	public void ResetCurrentCounter() { this.currentCounter = 0.0f; }
}

[Serializable]
public class MetronomeContainer
{
	private Stopwatch timer = null;
	private float bpm = 60f;
	public float Bpm { get { return this.bpm; } }

	private float counter = 0;
	public float Counter { get { return this.counter; } }

	/// <summary>
	/// Gets the elapsed time in seconds.
	/// </summary>
	/// <value>The elapsed time.</value>
	public float ElapsedTime
	{ 
		get
		{ 
			return (float)this.timer.ElapsedMilliseconds / 1000f;
		} 
	}

	public MetronomeContainer ()
	{
		this.timer = new Stopwatch ();
		this.timer.Start ();
		SetCounter ();
	}

	public float SetBpm(int value)
	{
		if (Mathf.Abs (value) > 1) 
		{
			return this.counter;
		}
		float temp = this.bpm + value;
		this.bpm = Mathf.Clamp (temp, 20f, 200f);
		return this.bpm;
	}

	public float elapsedTime;
	public bool UpdateMetronome(float deltaTime)
	{
		elapsedTime += deltaTime;
		if (elapsedTime < (this.Counter)) { return false; }
		elapsedTime -= this.Counter; 
		return true;
	}

	public void SetCounter() { this.counter = 60f / this.bpm; }
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

