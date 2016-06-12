using UnityEngine;
using System.Collections;
using NSubstitute;
using NUnit.Framework;
using System;

public class MusicalScoreTest 
{
	IMusicalScore scoreController = null;
	ScoreContainer scoreContainer = null;

	[TestFixtureSetUp]
	public void SetUp()
	{
		this.scoreController = Substitute.For<IMusicalScore>();
		this.scoreContainer = new ScoreContainer(this.scoreController);
	}

	[Test]
	public void Test()
	{

	} 
}
