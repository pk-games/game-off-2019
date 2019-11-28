using UnityEngine;
using UnityEngine.UI;

public class FadeInOutText : MonoBehaviour
{
    public float fadeDuration = 1.5f;
    public float backgroundDuration = 1.5f;

    public Text text;
    public Image image;

    void Start()
    {
        Invoke("Fade", 0.5f);
    }

    public void Fade()
    {
        text.CrossFadeAlpha(0, fadeDuration, false);
        image.CrossFadeAlpha(0, backgroundDuration, false);
    }
}