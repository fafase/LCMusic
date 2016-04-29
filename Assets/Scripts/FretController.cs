using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class FretController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    private FretContainer fretContainer = null;

	private void Start()
	{
        this.fretContainer = new FretContainer( this.transform.parent.GetComponent<IStringControllerCons>() , 
            this.gameObject.name);
	}

    private void OnDestroy()
    {
        this.fretContainer = null;
    }

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData)
	{
		this.fretContainer.ResetCurrentFret ();
	}
	#endregion

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData)
	{
		this.fretContainer.SetCurrentFret ();
	}
	#endregion
}

[Serializable]
public class FretContainer : IDisposable
{
    private IStringControllerCons stringCtrl = null;
    private uint fret = 0;
    public uint Fret { get { return this.fret; } }

    public FretContainer( IStringControllerCons newStringCtrl, string name)
    {
        if (newStringCtrl == null)
        {
            Debug.LogError("[LCMusic] Missing string controller in parent");
            return;
        }
        this.stringCtrl = newStringCtrl;
        this.fret = RetrieveFretNumberInName(name);
    }

    private uint RetrieveFretNumberInName(string name)
    {
        char lastChar = name[name.Length - 2];
        uint result = uint.Parse(lastChar.ToString()) + 1;
        return result;
    }

    public void SetCurrentFret()
    {
        this.stringCtrl.SetCurrentFret(this.fret);
    }
    public void ResetCurrentFret()
    {
        this.stringCtrl.SetCurrentFret(0);
    }
        
    public void Dispose()
    {
        this.stringCtrl = null;
    }
}
