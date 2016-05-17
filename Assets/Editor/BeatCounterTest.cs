using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;

public class BeatCounterTest 
{
	private BeatCounterContainer beatCounter = null;
	private IBeatCounter iBeatCounter = null;

	[TestFixtureSetUp]
	public void SetUp()
	{
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
		float result = this.beatCounter.UpdateCurrentCounter(2.0f);
		Assert.AreEqual(2.0f, result);
    }

	[Test]
	public void BeatCounterResetTimeAfterBarLength()
	{
		this.beatCounter.UpdateCurrentCounter(3.0f);
		float result = this.beatCounter.UpdateCurrentCounter(1.5f);
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
		float result = this.beatCounter.UpdateCurrentCounter(0.5f);
		Assert.AreEqual(1.5f, result);
	}

	[Test]
	public void BeatCounterLoopTestBpm100()
	{
		this.iBeatCounter.Bpm.Returns(100f); // Length 2.4s
		float result = 0.0f;
		for(int i = 0; i < 120; i++) // Adds up to 2.64
		{
			result = this.beatCounter.UpdateCurrentCounter(0.022f);
		}

		Assert.AreEqual(0.24f, result, 0.001f);
	}

	[Test]
	public void BeatCounterLoopTestBpm120()
	{
		this.iBeatCounter.Bpm.Returns(120f); // Length 2s
		float result = 0.0f;
		for(int i = 0; i < 100; i++)
		{
			result = this.beatCounter.UpdateCurrentCounter(0.026f); // Adds up to 2.6f
		}
		Assert.AreEqual(0.6f, result, 0.001f);
	}
		
	[Test]
	public void BeatCounterCheckPeriodBar60()
	{
		this.iBeatCounter.Bpm.Returns(60f);
		float result = this.beatCounter.GetPeriodBarInSec();
		Assert.AreEqual(4.0f, result);
	} 
	[Test]
	public void BeatCounterCheckPeriodBar120()
	{
		this.iBeatCounter.Bpm.Returns(120f);
		float result = this.beatCounter.GetPeriodBarInSec();
		Assert.AreEqual(2.0f, result);
	} 

}
