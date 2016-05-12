using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
public class InputTest
{
    private IInput input = null;
    private InputContainer inputContainer = null;

    [SetUp]
    public void SetTest()
    {
        this.input = Substitute.For<IInput>();
        this.inputContainer = new InputContainer(this.input);
    }
    [Test]
    public void EditorTest()
    {
        //Arrange

        //Act

        //Assert
    }
}
