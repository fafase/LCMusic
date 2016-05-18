using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Reflection;

public class BeatCounterTest 
{
	private BeatCounterContainer beatCounter = null;
	private IBeatCounter iBeatCounter = null;
	private IMetronome metronome = null;

	[TestFixtureSetUp]
	public void SetUp()
	{
		this.metronome = Substitute.For<IMetronome>();
		this.iBeatCounter = Substitute.For<IBeatCounter>();
		this.iBeatCounter.BarLength.Returns(4f);
		this.iBeatCounter.Bpm.Returns(60f);
		this.beatCounter = new BeatCounterContainer(iBeatCounter);
	}

	[SetUp]
	public void BeatCounterResetCounter()
	{
		this.beatCounter.ResetCurrentCounter();
	}

    [Test]
    public void BeatCounterAddTime()
    {
		this.beatCounter.UpdateCurrentCounter(2.0f);
		float result = this.beatCounter.CurrentCounter;
		Assert.AreEqual(2.0f, result);
    }

	[Test]
	public void BeatCounterResetTimeAfterBarLength()
	{
		this.beatCounter.UpdateCurrentCounter(3.0f);
		this.beatCounter.UpdateCurrentCounter(1.5f);
		float result = this.beatCounter.CurrentCounter;
		Assert.AreEqual(0.5f, result);
	}

	[Test]
	public void BeatCounterLoopTestBpm60()
	{
		this.iBeatCounter.Bpm.Returns(60f);
		for(int i = 0; i < 10; i++)
		{
			this.beatCounter.UpdateCurrentCounter(0.5f);
		}
		float result = this.beatCounter.CurrentCounter;
		Assert.AreEqual(1.0f, result);
	}

	[Test]
	public void BeatCounterLoopTestBpm100()
	{
		this.iBeatCounter.Bpm.Returns(100f); // Length 2.4s
		for(int i = 0; i < 120; i++) // Adds up to 2.64
		{
			this.beatCounter.UpdateCurrentCounter(0.022f);
		}
		float result = this.beatCounter.CurrentCounter;
		Assert.AreEqual(0.24f, result, 0.001f);
	}

	[Test]
	public void BeatCounterLoopTestBpm120()
	{
		this.iBeatCounter.Bpm.Returns(120f); // Length 2s
		for(int i = 0; i < 100; i++)
		{
			this.beatCounter.UpdateCurrentCounter(0.026f); // Adds up to 2.6f
		}
		float result = this.beatCounter.CurrentCounter;
		Assert.AreEqual(0.6f, result, 0.001f);
	}

	[Test]
	public void BeatCounterCheckElapsedOverFrame20ms()
	{
		for(int i = 0; i < 250; i++)
		{
			UpdateBeatCounter(0.02f);
		}
		float result = this.beatCounter.CurrentCounter;
		Assert.AreEqual(1.0f, result, 0.001f);
	}

	[Test]
	public void BeatCounterCheckElapsedOverFrame16ms()
	{
		for(int i = 0; i < 300; i++)
		{
			//SetDateTime(16);
			UpdateBeatCounter(0.016f);
		}
		float result = this.beatCounter.CurrentCounter;
		Assert.AreEqual(0.8f, result, 0.01f);
	}

	private void UpdateBeatCounter(float ms)
	{
		this.metronome.ElapsedTime.Returns(ms);
		float elapsedTime = this.metronome.ElapsedTime;
		this.beatCounter.UpdateCurrentCounter(elapsedTime);
	}
	private void SetDateTime(int elapsedTimeMs)
	{
		DateTime dt = DateTime.Now;
		TimeSpan ts = new TimeSpan(0,0,0,0,-elapsedTimeMs);
		dt = dt + ts;
		FieldInfo fi = this.beatCounter.GetType().GetField("previousDateTime",BindingFlags.NonPublic| BindingFlags.Instance);
		fi.SetValue(this.beatCounter, dt);
	}
}
