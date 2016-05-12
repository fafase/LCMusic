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

        this.rt.anchoredPosition = this.sequenceBtnCreator.SetAnchorDimension(this.rt.anchoredPosition);
        this.rt.sizeDelta = this.sequenceBtnCreator.SetAnchorDimension( this.rt.sizeDelta);

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
		Debug.Log (json);
        if (json == null) 
		{
			throw new NullReferenceException ("Missing json file");		 
		}
        this.rootObject = JsonUtility.FromJson<RootObject>(json);
    }

    public Vector2 SetAnchorDimension(Vector2 originalAnchorPos)
    {
        return SetDimension(originalAnchorPos, -1);
    }

    public Vector2 SetSizeDeltaDimension(Vector2 originalSizeDelta)
    {
        return SetDimension(originalSizeDelta, 1);
    }

    private Vector2 SetDimension(  Vector2 originalSize, int polarity)
    {
        if (this.rootObject == null || this.rootObject.lesson == null) { return originalSize; }
		int btnLength = this.rootObject.lesson.Length;
        if (btnLength == 0) { return originalSize; }
        float dimension = this.sequence.Size * btnLength;
        Vector2 size = originalSize;
        size.y = dimension * polarity;
        return originalSize;
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
