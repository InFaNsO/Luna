using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenTransition : MonoBehaviour
{
    public Image Fade;
    public float duration = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        Fade.canvasRenderer.SetAlpha(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        Fade.CrossFadeAlpha(1.0f, duration, false);
    }

    public void FadeOut()
    {
        Fade.CrossFadeAlpha(0.0f, duration, false);
    }
}
