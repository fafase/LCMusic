using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using LCHelper;

public interface IChooseExercise{}
public class ChooseExerciseController : MonoBehaviour, IChooseExercise
{
	[SerializeField] private Text styleName = null;

	private ChooseExerciseContainer chooseExercise = null;

	private void Awake()
	{
		if (this.styleName == null) { throw new NullReferenceException ("Missing styleName text");}
		this.chooseExercise = new ChooseExerciseContainer (this as IChooseExercise);
		Lesson lesson = Save.DeserializeFromPlayerPrefs<Lesson> (ConstString.CurrentData);
		if (lesson != null) 
		{
			this.styleName.text = lesson.name;
		}
	}

	private void OnDestroy()
	{
		this.chooseExercise = null;
	}
}

[Serializable]
public class ChooseExerciseContainer : IDisposable
{
	private IChooseExercise chooseExercise = null;
	public ChooseExerciseContainer(IChooseExercise chooseExercise)
	{
		this.chooseExercise = chooseExercise;
	}

	public void Dispose()
	{
		this.chooseExercise = null;
	}
}

