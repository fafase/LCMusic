using UnityEngine;
using UnityEngine.EventSystems;

public class PickController : MonoBehaviour , IPointerExitHandler , IPointerEnterHandler
{
	[SerializeField] private StringController strCtrl = null;
	private bool pointerDown = false;

	private void Start()
	{
		if (this.strCtrl == null) {
			Debug.LogError ("[LCMusic] Missing str controller");
		}
	}

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData)
	{
		pointerDown = true;
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData)
	{
		if (pointerDown == true) 
		{
			this.strCtrl.PlayCurrentFret ();
		}
		pointerDown = false;
	}
	#endregion
}
