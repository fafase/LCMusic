using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SequenceButtonCreator : MonoBehaviour {

    [SerializeField]
    private RectTransform rt = null;
    [SerializeField]
    private GameObject prefabBtn;

    private void Awake()
    {
        string json = PlayerPrefs.GetString(AppController.JsonData, null);
        if (json == null) { return; }
         RootObject rootObject = JsonUtility.FromJson<RootObject>(json);

        if (rootObject == null || rootObject.sequences == null)
        {
            return;
        }
        
        int btnLength = rootObject.sequences.Length;
        if (btnLength == 0) {  return; }
        float dimension = 83f * btnLength;

        Vector2 anchoredPos = rt.anchoredPosition;
        Vector2 sizeDelta = rt.sizeDelta;

        anchoredPos.y = -dimension;
        sizeDelta.y = dimension;

        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;
        foreach (Sequence sequence in rootObject.sequences)
        {
            GameObject obj = (GameObject)Instantiate(this.prefabBtn);
            obj.name = sequence.name;
            obj.transform.SetParent(this.rt, false);
            Text text = obj.GetComponentInChildren<Text>();
            string newName = sequence.name;
            if (sequence.chords == null || sequence.chords.Length == 0)
            {
                newName = newName + " (Not available)";
            }
            text.text = newName;
        }
    }
}
