using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using LCHelper;

public class SequenceButtonCreator : MonoBehaviour {

    [SerializeField]
    private RectTransform rt = null;
    [SerializeField]
    private GameObject prefabBtn;
    private float size = 80f;

    private void Awake()
    {
        string json = PlayerPrefs.GetString(AppController.JsonData, null);
        if (json == null) { return; }
        RootObject rootObject = JsonUtility.FromJson<RootObject>(json);

        if (rootObject == null || rootObject.sequences == null) {  return;  }

        SetContainerDimension(rootObject.sequences.Length);
        CreateButtons(rootObject.sequences );   
    }

    private void SetContainerDimension(int btnLength)
    {
        if (btnLength == 0) { return; }
        float dimension = this.size * btnLength;

        Vector2 anchoredPos = rt.anchoredPosition;
        Vector2 sizeDelta = rt.sizeDelta;

        anchoredPos.y = -dimension;
        sizeDelta.y = dimension;

        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;
    }

    private void CreateButtons(IEnumerable <Sequence> sequences)
    {
        FrontUIAction ftUIAction = FindObjectOfType<FrontUIAction>();
        if (ftUIAction == null) { return; }

        foreach (Sequence sequence in sequences)
        {
            GameObject obj = (GameObject)Instantiate(this.prefabBtn);
            Button btn = obj.GetComponent<Button>();
            if (btn == null) { continue; }
            Sequence currentSequence = sequence;
            

            obj.name = sequence.name;
            obj.transform.SetParent(this.rt, false);
            Text text = obj.GetComponentInChildren<Text>();
            string newName = sequence.name;
            if (sequence.chords == null || sequence.chords.Length == 0)
            {
                newName = newName + " (Not available)";
            }
            else
            {
                btn.onClick.AddListener(() =>
                {
                    Save.SerializeInPlayerPrefs(AppController.CurrentData, currentSequence);
                    ftUIAction.LoadScene("SequenceScene");
                });
            }
            text.text = newName;
        }
    }
}
