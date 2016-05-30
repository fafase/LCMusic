using UnityEngine;
using System.Collections;
using NSubstitute;
using NUnit.Framework;
using System;

public class PlayerPrefsLessonTest 
{
	[TestFixtureSetUp]
	public void SetUp()
	{
		PlayerPrefs.DeleteAll();
	}

	[Test]
	public void PlayerPrefsSetLesson_NoLesson()
	{
		bool result = PlayerPrefsWithAds.CheckPlayerPrefsForLessonWithId("lesson5");
		Assert.AreEqual(false, result);
	} 

	[Test]
	public void PlayerPrefsSetLesson_LessonAdded()
	{
		PlayerPrefsWithAds.SetPlayerPrefsForLessonWithId("lesson1", DateTime.Now);
		bool result = PlayerPrefsWithAds.CheckPlayerPrefsForLessonWithId("lesson1");
		Assert.AreEqual(true, result);
	}

	[Test]
	public void PlayerPrefsSetLesson_CheckLessonAddedRecent()
	{
		PlayerPrefsWithAds.SetPlayerPrefsForLessonWithId("lesson2", DateTime.Now);
		bool result = PlayerPrefsWithAds.CheckPlayerPrefsForLessonWithId("lesson2");

		if(result == true)
		{
			bool checkTime = PlayerPrefsWithAds.CheckDateTimeForLessonWithId("lesson2");
			Assert.AreEqual(true, checkTime);
		}
	}

	[Test]
	public void PlayerPrefsSetLesson_CheckLessonAddedOld()
	{
		DateTime old = DateTime.Now.AddHours(-3);
		PlayerPrefsWithAds.SetPlayerPrefsForLessonWithId("lesson3", old);
		bool result = PlayerPrefsWithAds.CheckPlayerPrefsForLessonWithId("lesson3");

		if(result == true)
		{
			bool checkTime = PlayerPrefsWithAds.CheckDateTimeForLessonWithId("lesson3");
			Assert.AreEqual(false, checkTime);
		}
	}

	[Test]
	public void PlayerPrefsSetLesson_CheckOldRemoval()
	{
		DateTime old = DateTime.Now.AddHours(-3);
		PlayerPrefsWithAds.SetPlayerPrefsForLessonWithId("lesson10", old);
		bool result = PlayerPrefsWithAds.CheckPlayerPrefsForLessonWithId("lesson10");

		if(result == true)
		{
			bool checkTime = PlayerPrefsWithAds.CheckDateTimeForLessonWithId("lesson10");
			if(checkTime == false)
			{
				string data = PlayerPrefs.GetString("lesson10", null);
				Assert.AreEqual(true, string.IsNullOrEmpty(data));
			}
		}
	}
}
