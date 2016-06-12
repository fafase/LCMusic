using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public interface IBar
{
	IEnumerable<ILine> GetLines();
}
public sealed class BarController : MonoBehaviour, IBar 
{
	private LineController [] lines = null;
	private BarContainer bar = null;

	private void Awake () 
	{
		this.lines = this.gameObject.GetComponentsInChildren<LineController>();
		if(this.lines.Length != 4){ throw new NullReferenceException("Missing LineController object"); }

		this.bar = new BarContainer(this as IBar);
	}

	public IEnumerable<ILine> GetLines()
	{ 
		ICollection<ILine> list = new List<ILine>();
		foreach(LineController line in this.lines)
		{
			list.Add(line as ILine);
		}
		return list as IEnumerable<ILine>; 
	}

	public void CreateNoteWithPrefab(GameObject notePrefab, IEnumerable<Note>notes)
	{
		StartCoroutine(CreateNoteWithPrefabCoroutine(notePrefab, notes));
	}
	private IEnumerator CreateNoteWithPrefabCoroutine(GameObject notePrefab, IEnumerable<Note>notes)
	{
		// Requires a frame for UI to be drawn
		yield return null;
		float index = 0.0f;
		foreach(Note note in notes)
		{
			GameObject noteObj = (GameObject)Instantiate(notePrefab);
			KeyData keyData = this.bar.GetKeyDataFromKeyNote(note.key);
			RectTransform rt = ((LineController)keyData.line).GetComponent<RectTransform>();
			RectTransform noteRt = noteObj.GetComponent<RectTransform>();
			noteRt.sizeDelta = new Vector2(rt.sizeDelta.x / 4f * note.length, rt.sizeDelta.y);
			noteRt.SetParent(rt, false);
			float anchoredY = (keyData.onLine == false) ? noteRt.anchoredPosition.y + rt.sizeDelta.y / 2f : noteRt.anchoredPosition.y ; 
			noteRt.anchoredPosition = new Vector2(rt.sizeDelta.x / 4f * index , anchoredY);
			noteObj.GetComponent<Image>().color = note.KeyColor;
			index += 1.0f;
		}
	}
}

[Serializable]
public class BarContainer
{
	private IBar bar = null;
	private IDictionary <int, KeyData> lineDict = null;
	public 	IDictionary <int, KeyData> LineDict { get { return this.lineDict;  } }

	public BarContainer(IBar bar)  
	{
		this.bar = bar;

		SetDictionaryContent();
	}

	private void SetDictionaryContent()
	{
		this.lineDict = new Dictionary<int, KeyData>();
		IEnumerable<ILine> lines = this.bar.GetLines();
		foreach(ILine line in lines)
		{
			KeyNoteValue knv = line.GetKeyNoteValue();
			this.lineDict.Add(knv.a, new KeyData(line, true));
			this.lineDict.Add(knv.b, new KeyData(line, false));
		} 
	}

	public KeyData GetKeyDataFromKeyNote(int index)
	{
		KeyData returnValue = null;
		this.lineDict.TryGetValue(index, out returnValue);
		return returnValue;
	}
}

public class KeyData
{
	public readonly ILine line = null;
	public readonly bool onLine = false;
	public KeyData(ILine line, bool onLine)
	{
		this.line = line;
		this.onLine = onLine;
	}
}
