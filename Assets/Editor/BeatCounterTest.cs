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
		float result = this.beatCounter.SetCurrentCounter(2.0f);
		Assert.AreEqual(2.0f, result);
    }

	[Test]
	public void BeatCounterResetTimeAfterBarLength()
	{
		this.beatCounter.SetCurrentCounter(3.0f);
		float result = this.beatCounter.SetCurrentCounter(1.5f);
		Assert.AreEqual(0.5f, result);
	}

	[Test]
	public void BeatCounterLoopTestA()
	{
		for(int i = 0; i < 10; i++)
		{
			this.beatCounter.SetCurrentCounter(0.5f);
		}
		float result = this.beatCounter.SetCurrentCounter(0.5f);
		Assert.AreEqual(1.5f, result);
	}
	[Test]
	public void BeatCounterLoopTestB()
	{
		for(int i = 0; i < 20; i++)
		{
			this.beatCounter.SetCurrentCounter(0.25f);
		}
		float result = this.beatCounter.SetCurrentCounter(0.25f);
		Assert.AreEqual(1.25f, result);
	}

	[Test]
	public void BeatCounterCheckPeriodBar60()
	{
		this.iBeatCounter.Bpm.Returns(60f);
		float result = this.beatCounter.GetPeriodBar();
		Assert.AreEqual(4.0f, result);
	} 
	[Test]
	public void BeatCounterCheckPeriodBar120()
	{
		this.iBeatCounter.Bpm.Returns(120f);
		float result = this.beatCounter.GetPeriodBar();
		Assert.AreEqual(2.0f, result);
	} 

}
