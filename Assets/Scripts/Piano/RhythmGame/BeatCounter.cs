using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Diagnostics;

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

	private AudioSource audioSource = null;
	private MetronomeContainer metronome = null; 
	private BeatCounterContainer beatCounter = null;

	private float barLength = 0.0f;
	public float BarLength { get { return this.barLength; } }

	public float Bpm { get { return this.metronome.Bpm; } }
	private float timer = 0f;
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
		if(this.metronome.UpdateMetronome()== false) { return; }
		this.audioSource.Play ();
		OnBpm (new BpmBeatArg(this.barBeat++));			// Reset movement speed
	}

	private void UpdateBeat()
	{
		this.beatCounter.UpdateCurrentCounter(this.metronome.ElapsedTime);
	}
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
	private float previousElapsedTime = 0.0f;

	public void UpdateCurrentCounter(float elapsedTime)
	{
		float deltaTime = elapsedTime - this.previousElapsedTime;
		this.previousElapsedTime = elapsedTime;
		this.currentCounter += deltaTime;
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
}

[Serializable]
public class MetronomeContainer
{
	private Stopwatch timer = null;
	private int index = 0;
	private float bpm = 60f;
	public float Bpm { get { return this.bpm; } }

	private float counter = 0;
	public float Counter { get { return this.counter; } }
	private float offsetTimer = 0.0f;

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

	public void ResetTimer ()
	{
		SetCounter ();
		this.timer.Reset ();
		this.timer.Start ();
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

	public bool UpdateMetronome()
	{
		float elapsedMs = (float)this.timer.ElapsedMilliseconds;
		float elapsedTimeOffset = (elapsedMs / 1000f) - this.Counter * this.index;
		if (elapsedTimeOffset < (this.Counter)) { return false; }

		this.index++;
		return true;
	}

	private void SetCounter() { this.counter = 60f / this.bpm; }
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

