using UnityEngine;
using System;

public interface IScrollButton
{
    IScrollRectSnap ScrollRectSnapInstance { get; }
}

public class ScrollButton : MonoBehaviour , IScrollButton
{
    private ScrollButtonContainer scrollBtn = null;
    private IScrollRectSnap scrollRectSnap = null;
    public IScrollRectSnap ScrollRectSnapInstance { get { return this.scrollRectSnap; } }
    private RectTransform rt = null;
    public RectTransform RectTr { get { return this.rt; } }

    public void Init(IScrollRectSnap scrollRectSnap)
    {
        this.rt = this.gameObject.GetComponent<RectTransform>();
        this.scrollBtn = new ScrollButtonContainer(this as IScrollButton);
        this.scrollRectSnap = scrollRectSnap;
    }

    public void SetScale(float distance, float threshold)
    {
        float scale = this.scrollBtn.SetScale(distance, Mathf.Abs(threshold));
        this.transform.localScale = new Vector3(scale, scale, 1f);
    }
}

[Serializable]
public class ScrollButtonContainer
{
    private IScrollButton scrollBtn = null;

    public ScrollButtonContainer(IScrollButton scrollButton)
    {
        this.scrollBtn = scrollButton;
    }

    public float SetScale(float distance, float threshold)
    {
        return Mathf.Clamp( 1 - ((1 - this.scrollBtn.ScrollRectSnapInstance.MinSize) * ( distance / threshold )), this.scrollBtn.ScrollRectSnapInstance.MinSize, 1f);
    }
}

public class EmptyCollectionException : Exception
{
    public EmptyCollectionException(string message) : base ("Missing collection for " +message)
    {

    }
}
