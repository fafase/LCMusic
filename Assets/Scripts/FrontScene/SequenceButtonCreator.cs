using UnityEngine;
using UnityEngine.UI;
using LCHelper;
using System;
using System.Collections.Generic;

public interface ISequenceBtnCreator
{
    float Size { get; }
    GameObject PrefabBtn { get; }
}

public class SequenceButtonCreator : MonoBehaviour, ISequenceBtnCreator
{
    [SerializeField]
    private RectTransform rt = null;
    [SerializeField]
    private GameObject prefabBtn;
    public GameObject PrefabBtn { get { return this.prefabBtn; } }
    public float size = 100f;
    public float Size { get { return this.size; } }

    private SequenceBtnCreatorHelper sequenceBtnCreator = null;

    private void Awake()
    {
        this.sequenceBtnCreator = new SequenceBtnCreatorHelper(this as ISequenceBtnCreator);
		this.sequenceBtnCreator.SetRectTransform(this.rt);

        FrontUIAction ftUIAction = FindObjectOfType<FrontUIAction>();
        if (ftUIAction == null) { return; }
        this.sequenceBtnCreator.CreateButtons(ftUIAction as IUIAction, this.rt);   
    }

    private void OnDestroy()
    {
        this.sequenceBtnCreator = null;
    }
}

[Serializable]
public class SequenceBtnCreatorHelper : IDisposable
{
    private ISequenceBtnCreator sequence = null;
    private RootObject rootObject = null;

    public SequenceBtnCreatorHelper(ISequenceBtnCreator sequence)
    {
        this.sequence = sequence;
        string json = PlayerPrefs.GetString(ConstString.JsonData, null);
        if (json == null) 
		{
			throw new NullReferenceException ("Missing json file");		 
		}
        this.rootObject = JsonUtility.FromJson<RootObject>(json);
    }
		

	private float GetSize()
	{
		if (this.rootObject == null || this.rootObject.lesson == null) { return 0f; }
		int btnLength = this.rootObject.lesson.Length;
		if (btnLength == 0) { return 0f; }

		return this.sequence.Size * btnLength;
	}

	public void SetRectTransform(RectTransform rt)
	{
		float size = GetSize();
		Vector2 anchoredPosition = rt.anchoredPosition;
		anchoredPosition.y = size / 2f * -1;
		rt.anchoredPosition = anchoredPosition;

		Vector2 sizeDelta = rt.sizeDelta;
		sizeDelta.y = size;
		rt.sizeDelta = sizeDelta;
	}

    public void CreateButtons(IUIAction uiAction, RectTransform rt)
    {
		IEnumerable<Lesson> lessons = this.rootObject.lesson as IEnumerable<Lesson>;
        foreach (Lesson lesson in lessons)
        {
            GameObject obj = (GameObject)UnityEngine.Object.Instantiate(this.sequence.PrefabBtn);
            Button btn = obj.GetComponent<Button>();
            if (btn == null) { continue; }
            Lesson currentLesson = lesson;

			obj.name = currentLesson.name;
            obj.transform.SetParent(rt, false);
            Text text = obj.GetComponentInChildren<Text>();
			string newName = currentLesson.name;
			if (currentLesson.available == false )
            {
                newName = newName + " (Not available)";
            }
            else
            {
                btn.onClick.AddListener(() =>
                {
					Save.SerializeInPlayerPrefs(ConstString.CurrentData, currentLesson);
					uiAction.LoadScene("ChooseExercises");
                });
            }
            text.text = newName;
        }
    }

    public void Dispose()
    {
        this.sequence = null;
        this.rootObject = null;
    }
}
