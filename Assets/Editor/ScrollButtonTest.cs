using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;

public class ScrollButtonTest
{
    private IScrollButton btn = null;
    private ScrollButtonContainer container = null;

    [SetUp]
    public void SetTest()
    {
        this.btn = Substitute.For<IScrollButton>();
        this.container = new ScrollButtonContainer(this.btn);
    }
    [Test]
    public void EditorTestFullScale()
    {
        float result = this.container.SetScale(0f, 300f);
        Assert.AreEqual(result, 1f);
    }
    [Test]
    public void EditorTestFullLowScale()
    {
        float result = this.container.SetScale(300f, 300f);
        Assert.AreEqual(result, 0.5f);
    }
    [Test]
    public void EditorTestMidScale()
    {
        float result = this.container.SetScale(150f, 300f);
        Assert.AreEqual(result, 0.75f);
    }
}
