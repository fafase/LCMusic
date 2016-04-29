using UnityEngine;
using LCHelper;
using UnityEngine.UI;
using System;


public interface IStringController
{
    AudioSource Audio { get; }
    EmptyString EmptyStringObj { get; }

}

public interface IStringControllerCons
{
    void SetCurrentFret(uint newFret);
    void PlayCurrentFret();
    void SetButtonFret(int i);
    void ResetButtonFret();
}

[RequireComponent(typeof(AudioSource))]
public class StringController : MonoBehaviour , IStringController, IStringControllerCons
{
	private AudioSource audioSource = null;
    public AudioSource Audio { get { return this.audioSource; } }

    [SerializeField] private EmptyString emptyString = null;
    public EmptyString EmptyStringObj { get { return this.emptyString; } }

    public Image[] fretButton;

    private StringObject stringObj = null;

    private void Awake()
	{
		this.audioSource = this.gameObject.GetComponent<AudioSource> ();
        if (this.emptyString == null)
        {
            Debug.LogError("[LCMusic] Missing EmptyString " + this.name);
        }
        this.stringObj = new StringObject(this as IStringController);
    }
	public void SetCurrentFret(uint newFret)
	{
        this.stringObj.SetCurrentFret(newFret);
    }

	public void PlayCurrentFret()
	{
        this.stringObj.PlayCurrentFret();
	}
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayCurrentFret();
        }
    }
#endif
    public void SetButtonFret(int i)
    {
        ResetButtonFret();
        this.fretButton[i - 1].color = Color.gray;
    }

    public void ResetButtonFret()
    {
        foreach (Image image in this.fretButton)
        {
            image.color = Color.white;
        }
    }
}
[Serializable]
public class StringObject
{
    private uint currentFret = 0;
    private IStringController stringController = null;

    public StringObject(IStringController stringCtrl)
    {
        this.stringController = stringCtrl;
    }

    public void SetCurrentFret(uint newFret)
    {
        this.currentFret = newFret;
        LCAudioPlayer.SetPitch(this.stringController.Audio, this.currentFret);
    }

    public void PlayCurrentFret()
    {
        if (this.currentFret == 0)
        {
            if (this.stringController.EmptyStringObj.IsStringSetOn() == false)
            {
                return;
            }
        }
        LCAudioPlayer.PlaySound(this.stringController.Audio, this.currentFret);
    }
}

