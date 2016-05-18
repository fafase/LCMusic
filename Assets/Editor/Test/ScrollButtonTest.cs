using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System.Reflection;
using System;

public class ScrollButtonTest
{
    private IScrollButton btn = null;
    private ScrollButtonContainer container = null;
	private IScrollRectSnap scrollRectSnap = null;

	[TestFixtureSetUp]
    public void SetTest()
    {
		this.scrollRectSnap = Substitute.For<IScrollRectSnap>();
		this.btn = Substitute.For<IScrollButton>();
		this.btn.Init(scrollRectSnap);
		this.btn.ScrollRectSnapInstance.MinSize.Returns(0.5f);
        this.container = new ScrollButtonContainer(this.btn);
    }
    [Test]
    public void EditorTestFullScale()
    {
        float result = this.container.SetScale(0f, 300f);
        Assert.AreEqual(1f, result);
    }
    [Test]
    public void EditorTestFullLowScale()
    {
        float result = this.container.SetScale(300f, 300f);
        Assert.AreEqual(0.5f, result);
    }
    [Test]
    public void EditorTestMidScale()
    {
        float result = this.container.SetScale(150f, 300f);
        Assert.AreEqual(0.75f, result);
    }

	[Test]
	public void EditorTestNoMinSize()
	{		
		this.btn.ScrollRectSnapInstance.MinSize.Returns(1f);
		float result = this.container.SetScale(150f, 300f);
		Assert.AreEqual(1f, result);
	}
}
