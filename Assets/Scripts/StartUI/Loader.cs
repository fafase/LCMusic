using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Loader : MonoBehaviour
{
    private Image image = null;
    private float speed = 1f;

    private void Awake()
    {
        this.image = this.gameObject.GetComponent<Image>();
        this.image.fillAmount = 0f;
    }

    private void Update()
    {
        this.image.fillAmount += Time.deltaTime * this.speed;
        if (IsFillZeroOrOne() == true)
        {
            this.image.fillClockwise = !this.image.fillClockwise;
            this.speed *= -1;
        }
    }
    private bool IsFillZeroOrOne()
    {
           return (this.image.fillAmount >= 1 || this.image.fillAmount <= 0);
    }
}
