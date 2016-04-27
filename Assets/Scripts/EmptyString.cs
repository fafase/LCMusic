using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EmptyString : MonoBehaviour
{
	private string [] states = new string[] { "X", "O" }; 
	private int index = 0;
	private Text stringText = null;

	private void Start()
	{
		this.stringText = this.gameObject.GetComponentInChildren<Text> ();
		if (this.stringText == null) 
		{
			Debug.LogError ("[Player] Missing Text");
			return;
		}
		this.stringText.text = states [index];

		Button button = this.gameObject.GetComponent<Button> ();
		button.onClick.AddListener (ChangeState);
	}
	private void OnDestroy()
	{
		Button button = this.gameObject.GetComponent<Button> ();
		if (button == null)   // Button may already be destroyed
		{
			return;
		}
		button.onClick.AddListener (ChangeState);
	}
	public void ChangeState () 
	{
		if (++index == this.states.Length) 
		{
			index = 0;
		}
		this.stringText.text = this.states [index];
	}

    public void SetState(int newIndex)
    {
        if (newIndex > 1 || newIndex < 0) { return; }
        index = newIndex;
        this.stringText.text = this.states[index];
    }

	public bool IsStringSetOn()
	{
		return this.index == 1;
	}
}
