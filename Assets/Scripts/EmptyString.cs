using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EmptyString : MonoBehaviour 
{
	private Text stringText = null;
    private EmptyStringContainer emptyStringContainer = null;

    private void Start()
	{
		this.stringText = this.gameObject.GetComponentInChildren<Text> ();
		if (this.stringText == null) 
		{
			Debug.LogError ("[Player] Missing Text");
			return;
		}

		Button button = this.gameObject.GetComponent<Button> ();
		button.onClick.AddListener (ChangeState);

        this.emptyStringContainer = new EmptyStringContainer();
        this.stringText.text = this.emptyStringContainer.SetState(0);
    }
	private void OnDestroy()
	{
		Button button = this.gameObject.GetComponent<Button> ();
		if (button == null)   // Button may already be destroyed
		{
			return;
		}
		button.onClick.RemoveAllListeners ();
        this.emptyStringContainer = null;
	}

	public void ChangeState () 
	{
        this.stringText.text = this.emptyStringContainer.ChangeState();
	}

    public void SetState(int newIndex)
    {
        this.stringText.text = this.emptyStringContainer.SetState(newIndex);
    }

	public bool IsStringSetOn()
	{
        return this.emptyStringContainer.IsStringSetOn();
	}
}

[Serializable]
public class EmptyStringContainer :IDisposable
{
    private int index = 0;
    private string[] states = new string[] { "X", "O" };

    public EmptyStringContainer() { }

    public string ChangeState()
    {
        if (++index == this.states.Length)
        {
            index = 0;
        }
        return this.states[index];
    }

    public string SetState(int newIndex)
    {
        Func<int, bool> IsNewIndexNotValid = (value) =>
        {
            return (value > 1 || value < 0);
        };

        if (IsNewIndexNotValid(newIndex) == true) { return null; }
        index = newIndex;
        return this.states[index];
    }

    public bool IsStringSetOn()
    {
        return this.index == 1;
    }

    public void Dispose()
    {
        this.states = null;
    }
}
