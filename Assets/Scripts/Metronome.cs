using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Diagnostics;

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

public interface IMetronome
{
	event EventHandler <BpmBeatArg> RaiseBpm;
	event EventHandler <BpmArg> RaiseChangeBpm;
	float Bpm { get; }
	float GetElapedTime();
}

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour , IMetronome
{

	[SerializeField] private Text bpmText = null;
	private AudioSource audioSource = null;
	private MetronomeContainer metronome = null; 

	private float timer = 0f;

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

	private void Start()
	{
		if (this.bpmText == null) 
		{
			throw new NullReferenceException ("Missing bpmText");
		}
		this.metronome = new MetronomeContainer (this as IMetronome);
		this.audioSource = this.gameObject.GetComponent<AudioSource> ();
		this.bpmText.text = ((int)this.metronome.Bpm).ToString ();
	}
	private void Update()
	{
		if (this.metronome.GetElapsedTime() < (this.metronome.Counter * 1000f))
		{
			return;
		}
		this.audioSource.Play ();
		this.metronome.ResetTimer ();
		OnBpm (new BpmBeatArg(this.barBeat++));			// Reset movement speed
	}

	public void SetBpm(int value)
	{
		this.bpmText.text = ((int)this.metronome.SetBpm (value)).ToString (); 
		OnChangeBpm (new BpmArg(this.Bpm));
	}

	public float GetElapedTime()
	{
		return this.metronome.GetElapsedTime ();
	}
}

[Serializable]
public class MetronomeContainer
{
	private IMetronome metronome = null;

	private float bpm = 60f;
	public float Bpm { get { return this.bpm; } }

	private float counter = 0;
	public float Counter { get { return this.counter; } }

	private Stopwatch timer = null;

	public MetronomeContainer (IMetronome metronome)
	{
		this.timer = new Stopwatch ();
		this.timer.Start ();
		this.metronome = metronome;
		SetCounter ();
	}

	public float GetElapsedTime()
	{
		return (float)this.timer.ElapsedMilliseconds;
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

	private void SetCounter()
	{
		this.counter = 60f /  this.bpm;
	}
}


