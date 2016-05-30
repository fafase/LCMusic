using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NUnit;
using NSubstitute;
using System;
using System.Reflection;

public class BeatCounterTest 
{
	private Metronome metronome = null;
	private IBeatCounter iBeatCounter = null;
	private IMetronome iMetronome = null;

	[TestFixtureSetUp]
	private void BeatContainer_Init()
	{
		this.iBeatCounter = Substitute.For<IBeatCounter>();
		this.iBeatCounter.BarLength.Returns(4f);
		this.iBeatCounter.Bpm.Returns(60f);
		this.iMetronome = Substitute.For<IMetronome>();
		this.metronome = new Metronome(4f);
	}
}
