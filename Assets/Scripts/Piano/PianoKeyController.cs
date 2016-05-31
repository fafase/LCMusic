using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IPianoKeyController
{
	event EventHandler<KeyDownEventArg> RaiseKeyDown;
    void PlayPianoKey(float pitch);
	void StopPianoKey();
    float Pitch { get; }
	Color OriginalColor { get; }
	Image PianoKeyImage { get; }
}

public class KeyDownEventArg:System.EventArgs
{
	public readonly Button currentButton = null;
	public readonly IPianoKeyController pianoKeyCtrl = null;
	public KeyDownEventArg(IPianoKeyController pianoKeyCtrl, Button button)
	{
		this.pianoKeyCtrl = pianoKeyCtrl;
		this.currentButton = button;
	}
}

[RequireComponent(typeof(Button), typeof(Image))]
public class PianoKeyController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler ,IPianoKeyController
{
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    private float pitch = 1.0f;
    public float Pitch { get { return this.pitch; } }

    private AudioSource audioSource = null;
    private PianoKey pianoKey = null;

	private Button currentButton = null;
	private Color originalColor = Color.white;
	public Color OriginalColor { get { return this.originalColor; } }

	private Image pianoKeyImage = null;
	public Image PianoKeyImage { get { return this.pianoKeyImage; } }
	private bool isKeyDown = false;

    private void Awake()
    {
        this.audioSource = this.gameObject.AddComponent<AudioSource>();
        this.audioSource.clip = this.clip;
        this.pianoKey = new PianoKey(this as IPianoKeyController);
		this.currentButton = this.gameObject.GetComponent<Button>();
		this.pianoKeyImage = this.gameObject.GetComponent<Image>();
		this.originalColor = this.pianoKeyImage.color;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
		if(this.isKeyDown == true){ return; }
		this.isKeyDown = true;
        this.pianoKey.PlayPianoKey();
		OnKeyDown(new KeyDownEventArg(this as IPianoKeyController, this.currentButton));
    }

	public void OnPointerExit (PointerEventData eventData)
	{
		this.isKeyDown = false;
		this.pianoKey.StopPianoKey();
	}

    public void PlayPianoKey(float pitch)
    {
        this.audioSource.Stop();
        this.audioSource.pitch = pitch;
        this.audioSource.Play();
    }

	public void StopPianoKey()
	{
		this.audioSource.Stop();
	}

	public event EventHandler<KeyDownEventArg> RaiseKeyDown;
	protected void OnKeyDown(KeyDownEventArg arg)
	{
		if(RaiseKeyDown != null)
		{
			RaiseKeyDown(this, arg);
		}
	}
}

[Serializable]
public class PianoKey
{
    private float pitch = 1.0f;
    private IPianoKeyController pianoKey = null;

    public PianoKey(IPianoKeyController pianoKey)
    {
        this.pianoKey = pianoKey;
        this.pitch = this.pianoKey.Pitch;
    }

    public void PlayPianoKey()
    {
        this.pianoKey.PlayPianoKey(this.pitch);
    }

	public void StopPianoKey()
	{
		this.pianoKey.StopPianoKey();
	}
}
